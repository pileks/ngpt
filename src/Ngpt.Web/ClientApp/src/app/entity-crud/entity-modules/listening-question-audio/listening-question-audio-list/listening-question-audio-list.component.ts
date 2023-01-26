import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { GridDataSource } from '@augur';
import { ListeningQuestionAudiosController } from '@src/app/web-api-controllers/listening-question-audios/listening-question-audios-controller';
import { ListeningQuestionAudio } from '@src/app/entities/listening-question-audio';
import { EntityListWithScopeFilterComponent } from '@src/app/entity-crud/generic-components/entity-list-with-scope-filter-component';
import { ListeningQuestionAudioGridModel } from '@src/app/web-api-controllers/listening-question-audios/models/listening-question-audio-grid-model';
import { GridFilterEntityResolverService } from '@src/app/services/grid-filter-entity-resolver.service';

@Component({
    selector: 'app-listening-question-audio-list',
    templateUrl: './listening-question-audio-list.component.html',
    styleUrls: ['./listening-question-audio-list.component.css']
})
export class ListeningQuestionAudioListComponent extends EntityListWithScopeFilterComponent<ListeningQuestionAudio> {
    constructor(
        public entityResolver: GridFilterEntityResolverService,
        private router: Router,
        private listeningQuestionAudiosController: ListeningQuestionAudiosController
    ) {
        super(listeningQuestionAudiosController);
    }

    gridDataSource = new GridDataSource<ListeningQuestionAudioGridModel>(
        model => this.listeningQuestionAudiosController.grid(model)
    );

    open(item: ListeningQuestionAudioGridModel): void {
        this.router.navigate(['/listening-question-audio/update', item.id]);
    }

    openInNewTab(item: ListeningQuestionAudioGridModel) {
        const url = this.router.serializeUrl(
            this.router.createUrlTree(['/listening-question-audio/update', item.id])
        );

        window.open(url, '_blank');
    }

    create() {
        this.router.navigate(['/listening-question-audio/create']);
    }
}