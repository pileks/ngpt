import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { ReadingQuestion } from '@src/app/entities/reading-question';
import { ReadingQuestionAnswer } from '@src/app/entities/reading-question-answer';
import { ReadingListeningQuestionAnswerPickerComponent } from '@src/app/players/reading-listening-question-answer-picker/reading-listening-question-answer-picker.component';
import IResettable from '@src/app/players/resettable-question-player';

@Component({
    selector: 'app-reading-question-player',
    templateUrl: './reading-question-player.component.html',
    styleUrls: ['./reading-question-player.component.css']
})
export class ReadingQuestionPlayerComponent implements OnInit, IResettable {

    constructor() { }

    @ViewChild(ReadingListeningQuestionAnswerPickerComponent) picker: IResettable;

    @Output() answerChange: EventEmitter<boolean> = new EventEmitter();
    @Input() question: ReadingQuestion;

    ngOnInit(): void {
        if (!this.question) {
            throw new Error('Attribute "question" is required.')
        }
    }

    onAnswerChange(answer: ReadingQuestionAnswer) {
        this.answerChange.emit(answer?.isCorrect ?? false);
    }

    reset(): void {
        this.picker.reset();
        this.answerChange.emit(false);
    }
}
