import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { SingleAnswerQuestion } from '@src/app/entities/single-answer-question';
import { SingleAnswerQuestionAnswer } from '@src/app/entities/single-answer-question-answer';
import { ReadingListeningQuestionAnswerPickerComponent } from '@src/app/players/reading-listening-question-answer-picker/reading-listening-question-answer-picker.component';
import IResettable from '@src/app/players/resettable-question-player';

@Component({
    selector: 'app-single-answer-question-player',
    templateUrl: './single-answer-question-player.component.html',
    styleUrls: ['./single-answer-question-player.component.css']
})
export class SingleAnswerQuestionPlayerComponent implements OnInit, IResettable {

    constructor() { }

    @ViewChild(ReadingListeningQuestionAnswerPickerComponent) picker: IResettable;

    @Output() answerChange: EventEmitter<boolean> = new EventEmitter();
    @Input() question: SingleAnswerQuestion;

    @Input() instruction: string = '';

    ngOnInit(): void {
        if (!this.question) {
            throw new Error('Attribute "question" is required.')
        }
    }

    onAnswerChange(answer: SingleAnswerQuestionAnswer) {
        this.answerChange.emit(answer?.isCorrect ?? false);
    }

    reset(): void {
        this.picker.reset();
        this.answerChange.emit(false);
    }
}
