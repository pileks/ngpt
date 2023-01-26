import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EntityUpdateComponent } from '@augur';
import { ReadingQuestionText } from '@src/app/entities/reading-question-text';
import { ReadingQuestionTextsController } from '@src/app/web-api-controllers/reading-question-texts/reading-question-texts-controller';
import { LanguagesController } from '@src/app/web-api-controllers/languages/languages-controller';
import { LevelsController } from '@src/app/web-api-controllers/levels/levels-controller';
import { ReadingQuestion } from '@src/app/entities/reading-question';

@Component({
    selector: 'app-reading-question-text-update',
    templateUrl: './reading-question-text-update.component.html',
    styleUrls: ['./reading-question-text-update.component.css']
})
export class ReadingQuestionTextUpdateComponent extends EntityUpdateComponent<ReadingQuestionText> {

    constructor(
        route: ActivatedRoute,
        router: Router,
        public languagesController: LanguagesController,
        public levelsController: LevelsController,
        private readingQuestionTextsController: ReadingQuestionTextsController,
    ) {

        super('reading-question-text', route, router, readingQuestionTextsController);
    }

    get isApproved(): boolean {
        return this.entity?.id && this.entity.approved;
    }

    submit() {
        if (this.entity.id) {
            this.update();
        } else {
            this.create();
        }
    }

    protected initNewEntity(): void {
        this.entity = new ReadingQuestionText();
    }

    onCreate() {
        this.router.navigate(['reading-question-text/update', this.entity.id]);
    }

    isValid(): boolean {
        return !!this.entity.languageId;
    }

    cancel() {
        this.navigateToList();
    }

    addQuestion() {
        this.router.navigate(['reading-question-text/update', this.id, 'questions', 'create']);
    }

    updateQuestion(question: ReadingQuestion) {
        if (this.isApproved) {
            return;
        }

        this.router.navigate(['reading-question-text/update', this.id, 'questions', question.id]);
    }

    async toggleApproval() {
        if (this.entity.id) {
            this.entity.approved = await this.readingQuestionTextsController.toggleApproval(this.entity);

            if (this.entity.approved) {
                this.navigateToList();
            }
        }
    }

    private navigateToList() {
        this.router.navigate(['reading-question-text/list']);
    }
}
