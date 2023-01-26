import { Component, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EntityUpdateComponent } from '@augur';
import { LevelsController } from '@src/app/web-api-controllers/levels/levels-controller';
import { ReadingQuestionsController } from '@src/app/web-api-controllers/reading-questions/reading-questions-controller';
import { ReadingQuestionTextsController } from '@src/app/web-api-controllers/reading-question-texts/reading-question-texts-controller';
import { ReadingQuestion } from '@src/app/entities/reading-question';
import { ReadingQuestionAnswerType, ReadingQuestionAnswerTypeDefinition } from '@src/app/enums/reading-question-answer-type';
import { ReadingQuestionAnswer } from '@src/app/entities/reading-question-answer';
import { LanguagesController } from '@src/app/web-api-controllers/languages/languages-controller';
import { ReadingQuestionPlayerComponent } from '@src/app/players/reading-question-player/reading-question-player.component';
import IResettable from '@src/app/players/resettable-question-player';

@Component({
    selector: 'app-reading-question-update',
    templateUrl: './reading-question-update.component.html',
    styleUrls: ['./reading-question-update.component.css']
})
export class ReadingQuestionUpdateComponent extends EntityUpdateComponent<ReadingQuestion> {

    constructor(
        route: ActivatedRoute,
        router: Router,
        public readingQuestionTextsController: ReadingQuestionTextsController,
        public levelsController: LevelsController,
        public languagesController: LanguagesController,
        private readingQuestionsController: ReadingQuestionsController,
    ) {
        super('reading-question', route, router, readingQuestionsController);
    }

    @ViewChild(ReadingQuestionPlayerComponent) previewWindow: IResettable;

    previewQuestion: ReadingQuestion;
    isPreviewAnsweredCorrectly: boolean = false;

    public getEntityIdFromRoute() {
        return this.route.snapshot.paramMap.get('questionId');
    }

    get textId(): number { return Number(this.route.snapshot.paramMap.get('id')); }

    async ngOnInit(): Promise<void> {
        await super.ngOnInit();

        if (!this.id) {
            this.entity.text = await this.readingQuestionTextsController.get(this.textId);
        }
    }

    onPreviewAnswerChange(value: boolean) {
        this.isPreviewAnsweredCorrectly = value;
    }

    preview() {
        this.previewQuestion = JSON.parse(JSON.stringify(this.entity));
        if (this.previewWindow) {
            this.previewWindow.reset();
        }
    }

    initNewEntity() {
        this.entity = new ReadingQuestion({
            answerType: ReadingQuestionAnswerType.Text,
            textId: this.textId
        });
    }

    answerTypes = Object.values(ReadingQuestionAnswerTypeDefinition);

    addAnswer() {
        let answer = new ReadingQuestionAnswer({
            isCorrect: false
        });

        this.entity.answers.push(answer);
        this.recalculateAnswerOrdinals();
    }

    removeAnswer(answer: ReadingQuestionAnswer) {
        const index = this.entity.answers.indexOf(answer);
        this.entity.answers.splice(index, 1);
        this.recalculateAnswerOrdinals();
    }

    recalculateAnswerOrdinals() {
        for (var i = 0; i < this.entity.answers.length; i++) {
            this.entity.answers[i].ordinal = i;
        }
    }

    get areAnswersText(): boolean {
        return this.entity.answerType === ReadingQuestionAnswerType.Text;
    }

    get areAnswersImages(): boolean {
        return this.entity.answerType === ReadingQuestionAnswerType.Image;
    }

    get canAddAnswer(): boolean {
        return this.entity.answers.length < 4;
    }

    get isAtLeastOneAnswerCorrect(): boolean {
        return this.entity.answers.some(x => x.isCorrect);
    }

    submit() {
        if (this.entity.id) {
            this.update();
        } else {
            this.create();
        }
    }

    onCreate() {
        this.router.navigate(['reading-question-text/update', this.textId]);
    }

    onUpdate() {
        this.router.navigate(['reading-question-text/update', this.textId]);
    }

    onDelete() {
        this.router.navigate(['reading-question-text/update', this.textId]);
    }

    cancel() {
        this.router.navigate(['reading-question-text/update', this.textId]);
    }
}
