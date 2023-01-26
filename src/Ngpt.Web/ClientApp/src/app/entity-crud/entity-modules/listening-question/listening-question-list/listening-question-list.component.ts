import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { GridDataSource } from '@augur';
import { ListeningQuestionsController } from '@src/app/web-api-controllers/listening-questions/listening-questions-controller';
import { ListeningQuestion } from '@src/app/entities/listening-question';
import { EntityListWithScopeFilterComponent } from '@src/app/entity-crud/generic-components/entity-list-with-scope-filter-component';
import { GridFilterEntityResolverService } from '@src/app/services/grid-filter-entity-resolver.service';
import { ListeningQuestionGridModel } from '@src/app/web-api-controllers/listening-questions/models/listening-question-grid-model';

@Component({
    selector: 'app-listening-question-list',
    templateUrl: './listening-question-list.component.html',
    styleUrls: ['./listening-question-list.component.css']
})
export class ListeningQuestionListComponent extends EntityListWithScopeFilterComponent<ListeningQuestion> {
    constructor(
        public entityResolver: GridFilterEntityResolverService,
        private router: Router,
        private listeningQuestionsController: ListeningQuestionsController
    ) {
        super(listeningQuestionsController);
    }

    gridDataSource = new GridDataSource<ListeningQuestionGridModel>(
        model => this.listeningQuestionsController.grid(model)
    );

    open(item: ListeningQuestionGridModel): void {
        this.router.navigate(['/listening-question/update', item.id]);
    }

    openInNewTab(item: ListeningQuestionGridModel) {
        const url = this.router.serializeUrl(
            this.router.createUrlTree(['/listening-question/update', item.id])
        );

        window.open(url, '_blank');
    }

    create() {
        this.router.navigate(['/listening-question/create']);
    }
}

