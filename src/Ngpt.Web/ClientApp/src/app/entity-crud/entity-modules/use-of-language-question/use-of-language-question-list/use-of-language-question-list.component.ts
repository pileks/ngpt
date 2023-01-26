import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { GridDataSource } from '@augur';
import { UseOfLanguageQuestionsController } from '@src/app/web-api-controllers/use-of-language-questions/use-of-language-questions-controller';
import { UseOfLanguageQuestionGridModel } from '@src/app/web-api-controllers/use-of-language-questions/models/use-of-language-question-grid-model';
import { GridFilterEntityResolverService } from '@src/app/services/grid-filter-entity-resolver.service';
import { UseOfLanguageQuestionTypeDefinition } from '@src/app/enums/use-of-language-question-type';
import { GridFilterEnumResolverService } from '@src/app/services/grid-filter-enum-resolver.service';

@Component({
    selector: 'app-use-of-language-question-list',
    templateUrl: './use-of-language-question-list.component.html',
    styleUrls: ['./use-of-language-question-list.component.css']
})
export class UseOfLanguageQuestionListComponent {
    constructor(
        public entityResolver: GridFilterEntityResolverService,
        public enumResolver: GridFilterEnumResolverService,
        private router: Router,
        private useOfLanguageQuestionsController: UseOfLanguageQuestionsController,
    ) { }

    questionTypes = UseOfLanguageQuestionTypeDefinition;

    gridDataSource = new GridDataSource<UseOfLanguageQuestionGridModel>(
        model => this.useOfLanguageQuestionsController.grid(model)
    );

    open(item: UseOfLanguageQuestionGridModel): void {
        this.router.navigate(['/use-of-language-question/update', item.id]);
    }

    openInNewTab(item: UseOfLanguageQuestionGridModel) {
        const url = this.router.serializeUrl(
            this.router.createUrlTree(['/use-of-language-question/update', item.id])
        );
        console.log(url);
        window.open(url, '_blank');
    }

    create() {
        this.router.navigate(['/use-of-language-question/create']);
    }
}