import { Component, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EntityUpdateComponent } from '@augur';
import { LevelsController } from '@src/app/web-api-controllers/levels/levels-controller';
import { UseOfLanguageQuestionsController } from '@src/app/web-api-controllers/use-of-language-questions/use-of-language-questions-controller';
import { UseOfLanguageQuestion } from '@src/app/entities/use-of-language-question';
import { UseOfLanguageQuestionType, UseOfLanguageQuestionTypeDefinition } from '@src/app/enums/use-of-language-question-type';
import { SingleGapQuestion } from '@src/app/entities/single-gap-question';
import { MultipleChoiceQuestion } from '@src/app/entities/multiple-choice-question';
import { DragDropQuestion } from '@src/app/entities/drag-drop-question';
import { SingleGapQuestionPlayerComponent } from '@src/app/players/single-gap-question-player/single-gap-question-player.component';
import { LanguagesController } from '@src/app/web-api-controllers/languages/languages-controller';
import IResettable from '@src/app/players/resettable-question-player';
import { MultipleChoiceQuestionPlayerComponent } from '@src/app/players/multiple-choice-question-player/multiple-choice-question-player.component';
import { DragDropQuestionPlayerComponent } from '@src/app/players/drag-drop-question-player/drag-drop-question-player.component';
import { InstructionsController } from '@src/app/web-api-controllers/instructions/instructions-controller';
import { Instruction } from '@src/app/entities/instruction';
import { SingleAnswerQuestion } from '@src/app/entities/single-answer-question';
import { SingleAnswerQuestionAnswerType } from '@src/app/enums/single-answer-question-answer-type';
import { SingleAnswerQuestionPlayerComponent } from '@src/app/players/single-answer-question-player/single-answer-question-player.component';
import { EntitySelectComponent } from '@shared/entity-select/entity-select.component';

@Component({
    selector: 'app-use-of-language-question-update',
    templateUrl: './use-of-language-question-update.component.html',
    styleUrls: ['./use-of-language-question-update.component.css']
})
export class UseOfLanguageQuestionUpdateComponent extends EntityUpdateComponent<UseOfLanguageQuestion> {

    constructor(
        route: ActivatedRoute,
        router: Router,
        public useOfLanguageQuestionsController: UseOfLanguageQuestionsController,
        public levelsController: LevelsController,
        public languagesController: LanguagesController,
        public instructionsController: InstructionsController
    ) {
        super('use-of-language-question', route, router, useOfLanguageQuestionsController);
    }

    @ViewChild(SingleGapQuestionPlayerComponent) singleGapPreviewWindow: IResettable;
    @ViewChild(MultipleChoiceQuestionPlayerComponent) multipleChoicePreviewWindow: IResettable;
    @ViewChild(DragDropQuestionPlayerComponent) dragDropPreviewWindow: IResettable;
    @ViewChild(SingleAnswerQuestionPlayerComponent) singleAnswerPreviewWindow: IResettable;

    @ViewChild('instructionSelect') instructionSelect: EntitySelectComponent<Instruction>;

    previewQuestion: SingleGapQuestion | MultipleChoiceQuestion | DragDropQuestion;
    isPreviewAnsweredCorrectly: boolean = false;

    get isApproved(): boolean {
        return this.entity?.id && this.entity.approved;
    }

    onPreviewAnswerChange(value: boolean) {
        this.isPreviewAnsweredCorrectly = value;
    }

    instructionsListFn = (pageNumber, pageSize, searchQuery, columnFilter) => this.instructionsController.listForQuestionTypeAndLanguage(this.entity.languageId, this.entity.type,
        pageNumber, pageSize, searchQuery, columnFilter);

    preview() {
        switch (this.entity.type) {
            case UseOfLanguageQuestionType.SingleGap:
                this.previewQuestion = JSON.parse(JSON.stringify(this.entity.singleGapQuestion));
                break;
            case UseOfLanguageQuestionType.MultipleChoice:
                this.previewQuestion = JSON.parse(JSON.stringify(this.entity.multipleChoiceQuestion));
                break;
            case UseOfLanguageQuestionType.DragDrop:
                this.previewQuestion = JSON.parse(JSON.stringify(this.entity.dragDropQuestion));
                break;
            case UseOfLanguageQuestionType.SingleAnswer:
                this.previewQuestion = JSON.parse(JSON.stringify(this.entity.singleAnswerQuestion));
                break;
            default:
        }

        if (this.singleGapPreviewWindow) {
            this.singleGapPreviewWindow.reset();
        }
        if (this.multipleChoicePreviewWindow) {
            this.multipleChoicePreviewWindow.reset();
        }
        if (this.dragDropPreviewWindow) {
            this.dragDropPreviewWindow.reset();
        }
        if (this.singleAnswerPreviewWindow) {
            this.singleAnswerPreviewWindow.reset();
        }
    }

    initNewEntity() {
        this.entity = new UseOfLanguageQuestion({
            type: UseOfLanguageQuestionType.SingleGap
        });

        this.initializeQuestions();
    }

    questionTypes = UseOfLanguageQuestionType;

    initializeQuestions() {
        this.entity.singleGapQuestion = new SingleGapQuestion();
        this.entity.multipleChoiceQuestion = new MultipleChoiceQuestion();
        this.entity.dragDropQuestion = new DragDropQuestion();
        this.entity.singleAnswerQuestion = new SingleAnswerQuestion({ answerType: SingleAnswerQuestionAnswerType.Text });
    }

    listTypes = Object.values(UseOfLanguageQuestionTypeDefinition);

    submit() {
        if (this.entity.id) {
            this.update();
        } else {
            this.create();
        }
    }

    async toggleApproval() {
        if (this.entity.id) {
            this.entity.approved = await this.useOfLanguageQuestionsController.toggleApproval(this.entity);

            if (this.entity.approved) {
                this.navigateToList();
            }
        }
    }

    isValid(): boolean {
        return !!this.entity.languageId;
    }

    cancel() {
        this.navigateToList();
    }

    onTypeChange() {
        this.resetInstruction();
        this.initializeQuestions();
    }

    onLanguageChange() {
        this.resetInstruction();
    }

    resetInstruction() {
        if (this.instructionSelect) {
            this.instructionSelect.removeSelection();
        }
    }

    onInstructionChange(instruction: Instruction) {
        this.entity.instruction = instruction;
    }

    onInstructionUnselect() {
        this.entity.instruction = null;
    }

    private navigateToList() {
        this.router.navigate(['use-of-language-question/list']);
    }
}
