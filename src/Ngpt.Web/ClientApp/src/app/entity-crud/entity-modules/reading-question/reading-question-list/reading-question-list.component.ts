import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { GridDataSource } from '@augur';
import { ReadingQuestionsController } from '@src/app/web-api-controllers/reading-questions/reading-questions-controller';
import { ReadingQuestion } from '@src/app/entities/reading-question';
import { EntityListWithScopeFilterComponent } from '@src/app/entity-crud/generic-components/entity-list-with-scope-filter-component';
import { ReadingQuestionGridModel } from '@src/app/web-api-controllers/reading-questions/models/reading-question-grid-model';
import { GridFilterEntityResolverService } from '@src/app/services/grid-filter-entity-resolver.service';

@Component({
    selector: 'app-reading-question-list',
    templateUrl: './reading-question-list.component.html',
    styleUrls: ['./reading-question-list.component.css']
})
export class ReadingQuestionListComponent extends EntityListWithScopeFilterComponent<ReadingQuestion> {
    constructor(
        public entityResolver: GridFilterEntityResolverService,
        private readingQuestionsController: ReadingQuestionsController,
        private router: Router
    ) {
        super(readingQuestionsController);
    }

    gridDataSource = new GridDataSource<ReadingQuestionGridModel>(
        model => this.readingQuestionsController.grid(model)
    );

    open(item: ReadingQuestionGridModel): void {
        this.router.navigate(['/reading-question/update', item.id]);
    }

    openInNewTab(item: ReadingQuestionGridModel) {
        const url = this.router.serializeUrl(
            this.router.createUrlTree(['/reading-question/update', item.id])
        );

        window.open(url, '_blank');
    }

    create() {
        this.router.navigate(['/reading-question/create']);
    }
}