import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { ReadingQuestion } from '@src/app/entities/reading-question';
import { ReadingQuestionAnswer } from '@src/app/entities/reading-question-answer';
import { ReadingQuestionText } from '../../entities/reading-question-text';

@Component({
    selector: 'app-reading-test-player',
    templateUrl: './reading-test-player.component.html',
    styleUrls: ['./reading-test-player.component.css']
})
export class ReadingTestPlayerComponent implements OnInit {
    constructor() { }

    @Output() onComplete: EventEmitter<number> = new EventEmitter();
    @Input() questions: ReadingQuestion[];
    @Input() text: ReadingQuestionText;

    correctAnswers: boolean[];
    isSubmitted: boolean = false;

    ngOnInit(): void {
        if (!this.text) {
            throw new Error('Attribute "text" is required.')
        }

        this.correctAnswers = this.questions.map(x => false);
    }

    onAnswerChange(answer: ReadingQuestionAnswer, questionIdx: number) {
        this.correctAnswers[questionIdx] = answer.isCorrect;
        console.log(this.correctAnswers);
    }

    complete(): void {
        this.onComplete.emit(this.correctAnswers.filter(x => x).length);
        this.isSubmitted = true;
    }
}
