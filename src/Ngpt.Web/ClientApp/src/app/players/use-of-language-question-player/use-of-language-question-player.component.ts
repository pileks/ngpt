import { EventEmitter, Input, Output } from '@angular/core';
import { ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { UseOfLanguageQuestion } from '@src/app/entities/use-of-language-question';
import { UseOfLanguageQuestionType } from '@src/app/enums/use-of-language-question-type';
import { DragDropQuestionPlayerComponent } from '@src/app/players/drag-drop-question-player/drag-drop-question-player.component';
import { MultipleChoiceQuestionPlayerComponent } from '@src/app/players/multiple-choice-question-player/multiple-choice-question-player.component';
import { SingleAnswerQuestionPlayerComponent } from '@src/app/players/single-answer-question-player/single-answer-question-player.component';
import { SingleGapQuestionPlayerComponent } from '@src/app/players/single-gap-question-player/single-gap-question-player.component';
import IResettable from '@src/app/players/resettable-question-player';

@Component({
    selector: 'app-use-of-language-question-player',
    templateUrl: './use-of-language-question-player.component.html',
    styleUrls: ['./use-of-language-question-player.component.css']
})
export class UseOfLanguageQuestionPlayerComponent implements OnInit, IResettable {

    @Output() answerChange: EventEmitter<boolean> = new EventEmitter();

    @ViewChild(SingleGapQuestionPlayerComponent) singleGapQuestion: IResettable;
    @ViewChild(MultipleChoiceQuestionPlayerComponent) multipleChoiceQuestion: IResettable;
    @ViewChild(DragDropQuestionPlayerComponent) dragDropQuestion: IResettable;
    @ViewChild(SingleAnswerQuestionPlayerComponent) singleAnswerQuestion: IResettable;

    constructor() { }

    reset(): void {
        if (this.singleGapQuestion) {
            this.singleGapQuestion.reset();
        }
        if (this.multipleChoiceQuestion) {
            this.multipleChoiceQuestion.reset();
        }
        if (this.dragDropQuestion) {
            this.dragDropQuestion.reset();
        }
        if (this.singleAnswerQuestion) {
            this.singleAnswerQuestion.reset();
        }
    }

    ngOnInit(): void {

    }

    @Input() question: UseOfLanguageQuestion;

    useOfLanguageQuestionType = UseOfLanguageQuestionType;

    onAnswerChange(isCorrect) {
        console.log(isCorrect);
        this.answerChange.emit(isCorrect);
    }
}
