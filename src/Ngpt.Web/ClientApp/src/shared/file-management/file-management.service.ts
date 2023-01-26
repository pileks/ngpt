import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { GoogleDriveFile } from '@src/app/entities/google-drive-file';

@Injectable({
    providedIn: 'root'
})
export class FileManagementService {
    constructor(private http: HttpClient) {}

    async uploadFile(file: File, folderId: string, authToken: string) {
        let res = await this.postFileMetadata(file, folderId, authToken);

        let res2 = await this.postFile(res.headers.get('location'), file);

        let fileInfo = new GoogleDriveFile();
        fileInfo.fileId = res2.id;
        fileInfo.name = res2.name;
        fileInfo.webViewLink = res2.webViewLink;

        return fileInfo;
    }

    async deleteFile(fileId: string, authToken: string) {
        await gapi.client.drive.files.delete({
            fileId: fileId,
            oauth_token: authToken
        });
    }

    private postFileMetadata(file: File, folderId: string, authToken: string) {
        const url = 'https://www.googleapis.com/upload/drive/v3/files';

        const headers = new HttpHeaders({
            Authorization: `Bearer ${authToken}`,
            'Content-Type': 'application/json; charset=UTF-8'
        });

        const params = { uploadType: 'resumable', fields: ['id,name,webViewLink'] };

        const fileMetadata: gapi.client.drive.File = {
            name: file.name,
            mimeType: file.type,
            parents: [folderId]
        };

        return this.http
            .post<any>(url, fileMetadata, {
                headers: headers,
                params: params,
                observe: 'response'
            })
            .toAugurHttpRequest<any>();
    }

    private postFile(url: string, file: File) {
        return this.http
            .put<any>(url, file, { observe: 'body' })
            .toAugurHttpRequest<any>();
    }
}
