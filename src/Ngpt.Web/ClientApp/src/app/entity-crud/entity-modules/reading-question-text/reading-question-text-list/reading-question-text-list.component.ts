import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { GridDataSource } from '@augur';
import { ReadingQuestionText } from '@src/app/entities/reading-question-text';
import { EntityListWithScopeFilterComponent } from '@src/app/entity-crud/generic-components/entity-list-with-scope-filter-component';
import { ReadingQuestionTextGridModel } from '@src/app/web-api-controllers/reading-question-texts/models/reading-question-text-grid-model';
import { GridFilterEntityResolverService } from '@src/app/services/grid-filter-entity-resolver.service';
import { ReadingQuestionTextsController } from '@src/app/web-api-controllers/reading-question-texts/reading-question-texts-controller';

@Component({
    selector: 'app-reading-question-text-list',
    templateUrl: './reading-question-text-list.component.html',
    styleUrls: ['./reading-question-text-list.component.css']
})
export class ReadingQuestionTextListComponent extends EntityListWithScopeFilterComponent<ReadingQuestionText> {
    constructor(
        public entityResolver: GridFilterEntityResolverService,
        private router: Router,
        private readingQuestionTextsController: ReadingQuestionTextsController
    ) {
        super(readingQuestionTextsController);
    }

    gridDataSource = new GridDataSource<ReadingQuestionTextGridModel>(
        model => this.readingQuestionTextsController.grid(model)
    );

    open(item: ReadingQuestionTextGridModel): void {
        this.router.navigate(['/reading-question-text/update', item.id]);
    }

    openInNewTab(item: ReadingQuestionTextGridModel) {
        const url = this.router.serializeUrl(
            this.router.createUrlTree(['/reading-question-text/update', item.id])
        );

        window.open(url, '_blank');
    }

    create() {
        this.router.navigate(['/reading-question-text/create']);
    }
}