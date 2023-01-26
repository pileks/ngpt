import { Injectable } from '@angular/core';
import { GridFilterEntityEndpointDefinition, IGridFilterEntityResolver } from '@augur';
import { UsersController } from '@platform';
import { TenantsController } from '@platform/web-api-controllers/identity/tenants/tenants-controller';
import { LanguagesController } from '@src/app/web-api-controllers/languages/languages-controller';
import { LevelsController } from '@src/app/web-api-controllers/levels/levels-controller';
import { ListeningQuestionAudiosController } from '@src/app/web-api-controllers/listening-question-audios/listening-question-audios-controller';
import { ReadingQuestionTextsController } from '@src/app/web-api-controllers/reading-question-texts/reading-question-texts-controller';

@Injectable({
    providedIn: 'root'
})
export class GridFilterEntityResolverService implements IGridFilterEntityResolver {

    constructor(
        private languagesController: LanguagesController,
        private levelsController: LevelsController,
        private usersController: UsersController,
        private tenantsController: TenantsController,
        private readingQuestionTextsController: ReadingQuestionTextsController,
        private listeningQuestionAudiosController: ListeningQuestionAudiosController
    ) {
    }

    resolve(entityType: string): GridFilterEntityEndpointDefinition {
        switch (entityType) {
            case 'Language':
                return new GridFilterEntityEndpointDefinition({
                    endpoint: (pageNumber, pageSize, searchQuery, columnFilter) => this.languagesController.list(pageNumber, pageSize, searchQuery, columnFilter),
                    map: (entity) => entity.name
                });
            case 'Level':
                return new GridFilterEntityEndpointDefinition({
                    endpoint: (pageNumber, pageSize, searchQuery, columnFilter) => this.levelsController.list(pageNumber, pageSize, searchQuery, columnFilter),
                    map: (entity) => entity.title
                });
            case 'User':
                return new GridFilterEntityEndpointDefinition({
                    endpoint: (pageNumber, pageSize, searchQuery, columnFilter) => this.usersController.listForGridFilter(pageNumber, pageSize, searchQuery, columnFilter),
                    map: (entity) => entity.name
                });
            case 'Tenant':
                return new GridFilterEntityEndpointDefinition({
                    endpoint: (pageNumber, pageSize, searchQuery, columnFilter) => this.tenantsController.listForGridFilter(pageNumber, pageSize, searchQuery, columnFilter),
                    map: (entity) => entity.displayName
                });
            case 'ReadingQuestionText':
                return new GridFilterEntityEndpointDefinition({
                    endpoint: (pageNumber, pageSize, searchQuery, columnFilter) => this.readingQuestionTextsController.list(pageNumber, pageSize, searchQuery, columnFilter),
                    map: (entity) => entity.title
                });
            case 'ListeningQuestionAudio':
                return new GridFilterEntityEndpointDefinition({
                    endpoint: (pageNumber, pageSize, searchQuery, columnFilter) => this.listeningQuestionAudiosController.list(pageNumber, pageSize, searchQuery, columnFilter),
                    map: (entity) => entity.title
                });
        }

        return null;
    }
}
