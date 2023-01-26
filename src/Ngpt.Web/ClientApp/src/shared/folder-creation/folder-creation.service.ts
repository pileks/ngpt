import { Injectable } from '@angular/core';
import { Project } from '@src/app/entities/project';
import { FolderStructureController } from '@src/app/web-api-controllers/folder-structure/folder-structure-controller';
import { ProjectFoldersUpdateModel } from '@src/app/web-api-controllers/folder-structure/models/project-folders-update-model';
import { TransnationalMeetingFoldersGetModel } from '@src/app/web-api-controllers/folder-structure/models/transnational-meeting-folders-get-model';
import { TransnationalMeetingFoldersUpdateModel } from '@src/app/web-api-controllers/folder-structure/models/transnational-meeting-folders-update-model';

import * as moment from 'moment';

@Injectable({
    providedIn: 'root'
})
export class FolderCreationService {
    constructor(private folderStructureController: FolderStructureController) {}

    folderCreationProgress: { isLoading: boolean; message: string; finishedLoading: boolean };

    async createProjectFolders(
        folderCreationProgress: { isLoading: boolean; message: string; finishedLoading: boolean },
        project: Project,
        oauthToken: string
    ) {
        this.folderCreationProgress = folderCreationProgress;

        this.folderCreationProgress.isLoading = true;
        this.folderCreationProgress.finishedLoading = false;

        project = await this.updateProjectFolders(project, oauthToken);

        await this.updateMeetingFolders(
            project.id,
            project.transnationalMeetingsFolderId,
            oauthToken
        );

        this.folderCreationProgress.isLoading = false;
        this.folderCreationProgress.finishedLoading = true;
        this.setMessage('Folders are up to date');

        return project;
    }

    private async updateProjectFolders(project: Project, oauthToken: string): Promise<Project> {
        this.setMessage(
            `WARNING: Do not navigate away from this page while folders are being created.\nCreating folders for ${project.title} project.`
        );

        let modifiedProject = await this.driveCreateProjectFolders(project, oauthToken);

        if (JSON.stringify(project) !== JSON.stringify(modifiedProject)) {
            project = { ...modifiedProject };

            let model: ProjectFoldersUpdateModel = {
                projectId: modifiedProject.id,
                rootFolderId: modifiedProject.rootFolderId,
                projectManagementFolderId: modifiedProject.projectManagementFolderId,
                transnationalMeetingsFolderId: modifiedProject.transnationalMeetingsFolderId,
                disseminationFolderId: modifiedProject.disseminationFolderId,
                disseminationDocumentsFolderId: modifiedProject.disseminationDocumentsFolderId
            };

            await this.folderStructureController.assignProjectFolders(model);
        }

        return project;
    }

    private async updateMeetingFolders(
        projectId: number,
        transnationalMeetingsFolderId: string,
        oauthToken: string
    ): Promise<void> {
        let meetings = await this.folderStructureController.getAllTransnationalMeetingFolders(
            projectId
        );

        for (let meeting of meetings) {
            this.setMessage(
                `WARNING: Do not navigate away from this page while folders are being created.\nCreating folders for ${meeting.title} transnational meeting.`
            );

            let modifiedMeeting = await this.driveCreateTransnationalMeetingFolders(
                meeting,
                transnationalMeetingsFolderId,
                oauthToken
            );

            if (JSON.stringify(meeting) !== JSON.stringify(modifiedMeeting)) {
                meeting = { ...modifiedMeeting };

                let model: TransnationalMeetingFoldersUpdateModel = {
                    transnationalMeetingId: modifiedMeeting.id,
                    rootFolderId: modifiedMeeting.rootFolderId,
                    meetingDetailsFolderId: modifiedMeeting.meetingDetailsFolderId,
                    photosFolderId: modifiedMeeting.photosFolderId,
                    travelAndAccommodationFolderId: modifiedMeeting.travelAndAccommodationFolderId,
                    presentationsFolderId: modifiedMeeting.presentationsFolderId
                };

                await this.folderStructureController.assignTransnationalMeetingFolders(model);
            }
        }
    }

    private async driveCreateProjectFolders(
        project: Project,
        oauthToken: string
    ): Promise<Project> {
        let modifiedProject = { ...project };
        let transnationalMeetingsFolderIdPromise: gapi.client.Request<gapi.client.drive.File>;
        let disseminationFolderIdPromise: gapi.client.Request<gapi.client.drive.File>;

        if (!modifiedProject.rootFolderId) {
            let response = await gapi.client.drive.files.create({
                resource: {
                    name: project.title,
                    mimeType: 'application/vnd.google-apps.folder'
                },
                oauth_token: oauthToken,
                fields: 'id'
            });
            modifiedProject.rootFolderId = response.result.id;
        }

        if (!modifiedProject.projectManagementFolderId) {
            let response = await gapi.client.drive.files.create({
                resource: {
                    name: 'Project Management',
                    mimeType: 'application/vnd.google-apps.folder',
                    parents: [modifiedProject.rootFolderId]
                },
                oauth_token: oauthToken,
                fields: 'id'
            });
            modifiedProject.projectManagementFolderId = response.result.id;
        }

        if (!modifiedProject.transnationalMeetingsFolderId) {
            transnationalMeetingsFolderIdPromise = gapi.client.drive.files.create({
                resource: {
                    name: 'Transnational Meetings',
                    mimeType: 'application/vnd.google-apps.folder',
                    parents: [modifiedProject.projectManagementFolderId]
                },
                oauth_token: oauthToken,
                fields: 'id'
            });
        }

        if (!modifiedProject.disseminationFolderId) {
            disseminationFolderIdPromise = gapi.client.drive.files.create({
                resource: {
                    name: 'Dissemination',
                    mimeType: 'application/vnd.google-apps.folder',
                    parents: [modifiedProject.projectManagementFolderId]
                },
                oauth_token: oauthToken,
                fields: 'id'
            });
        }

        await Promise.all([
            transnationalMeetingsFolderIdPromise,
            disseminationFolderIdPromise
        ]).then(promises => {
            for (let i = 0; i < promises.length; i++) {
                if (promises[i]) {
                    switch (i) {
                        case 0:
                            modifiedProject.transnationalMeetingsFolderId = promises[i].result.id;
                            break;
                        case 1:
                            modifiedProject.disseminationFolderId = promises[i].result.id;
                            break;
                    }
                }
            }
        });

        if (!modifiedProject.disseminationDocumentsFolderId) {
            let response = await gapi.client.drive.files.create({
                resource: {
                    name: 'Documents',
                    mimeType: 'application/vnd.google-apps.folder',
                    parents: [modifiedProject.disseminationFolderId]
                },
                oauth_token: oauthToken,
                fields: 'id'
            });
            modifiedProject.disseminationDocumentsFolderId = response.result.id;
        }

        return modifiedProject;
    }

    private async driveCreateTransnationalMeetingFolders(
        meeting: TransnationalMeetingFoldersGetModel,
        transnationalMeetingsFolderId: string,
        oauthToken: string
    ): Promise<TransnationalMeetingFoldersGetModel> {
        let meetingDetailsFolderIdPromise: gapi.client.Request<gapi.client.drive.File>;
        let photosFolderIdPromise: gapi.client.Request<gapi.client.drive.File>;
        let travelAndAccommodationFolderIdPromise: gapi.client.Request<gapi.client.drive.File>;
        let presentationsFolderIdPromise: gapi.client.Request<gapi.client.drive.File>;

        let modifiedMeeting = { ...meeting };
        let date = moment(meeting.from).format('YYYY_MM');

        if (!modifiedMeeting.rootFolderId) {
            let response = await gapi.client.drive.files.create({
                resource: {
                    name: `${date}-${modifiedMeeting.title}`,
                    mimeType: 'application/vnd.google-apps.folder',
                    parents: [transnationalMeetingsFolderId]
                },
                oauth_token: oauthToken,
                fields: 'id'
            });
            modifiedMeeting.rootFolderId = response.result.id;
        }

        if (!modifiedMeeting.meetingDetailsFolderId) {
            meetingDetailsFolderIdPromise = gapi.client.drive.files.create({
                resource: {
                    name: 'Agenda, activities, signatures, certificates',
                    mimeType: 'application/vnd.google-apps.folder',
                    parents: [modifiedMeeting.rootFolderId]
                },
                oauth_token: oauthToken,
                fields: 'id'
            });
        }

        if (!modifiedMeeting.photosFolderId) {
            photosFolderIdPromise = gapi.client.drive.files.create({
                resource: {
                    name: 'Photos',
                    mimeType: 'application/vnd.google-apps.folder',
                    parents: [modifiedMeeting.rootFolderId]
                },
                oauth_token: oauthToken,
                fields: 'id'
            });
        }

        if (!modifiedMeeting.travelAndAccommodationFolderId) {
            travelAndAccommodationFolderIdPromise = gapi.client.drive.files.create({
                resource: {
                    name: 'Travel and Accommodation',
                    mimeType: 'application/vnd.google-apps.folder',
                    parents: [modifiedMeeting.rootFolderId]
                },
                oauth_token: oauthToken,
                fields: 'id'
            });
        }

        if (!modifiedMeeting.presentationsFolderId) {
            presentationsFolderIdPromise = gapi.client.drive.files.create({
                resource: {
                    name: 'Presentations',
                    mimeType: 'application/vnd.google-apps.folder',
                    parents: [modifiedMeeting.rootFolderId]
                },
                oauth_token: oauthToken,
                fields: 'id'
            });
        }

        await Promise.all([
            meetingDetailsFolderIdPromise,
            photosFolderIdPromise,
            travelAndAccommodationFolderIdPromise,
            presentationsFolderIdPromise
        ]).then(promises => {
            for (let i = 0; i < promises.length; i++) {
                if (promises[i]) {
                    switch (i) {
                        case 0:
                            modifiedMeeting.meetingDetailsFolderId = promises[i].result.id;
                            break;
                        case 1:
                            modifiedMeeting.photosFolderId = promises[i].result.id;
                            break;
                        case 2:
                            modifiedMeeting.travelAndAccommodationFolderId = promises[i].result.id;
                            break;
                        case 3:
                            modifiedMeeting.presentationsFolderId = promises[i].result.id;
                            break;
                    }
                }
            }
        });

        return modifiedMeeting;
    }

    private setMessage(message: string): void {
        this.folderCreationProgress.message = message;
    }
}
