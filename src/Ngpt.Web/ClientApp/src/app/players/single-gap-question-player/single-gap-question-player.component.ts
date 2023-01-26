import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { SingleGapQuestion } from '@src/app/entities/single-gap-question';
import IResettable from '@src/app/players/resettable-question-player';

@Component({
    selector: 'app-single-gap-question-player',
    templateUrl: './single-gap-question-player.component.html',
    styleUrls: ['./single-gap-question-player.component.css']
})
export class SingleGapQuestionPlayerComponent implements OnInit, IResettable {

    constructor() { }

    @Output() answerChange: EventEmitter<boolean> = new EventEmitter();
    @Input() question: SingleGapQuestion;
    @Input() instruction: string = '';

    answer: string;

    get anyCaseSensitiveAnswers(): boolean {
        return this.question.answers.some(x => x.isCaseSensitive);
    }

    reset(): void {
        this.initialize();
    }

    onAnswerChange() {
        this.answerChange.emit(this.isAnswerCorrect());
    }

    isAnswerCorrect() {
        for (let answer of this.question.answers) {
            const userAnswer = answer.isCaseSensitive ? this.answer : this.answer.toLowerCase();
            const questionAnswer = answer.isCaseSensitive ? answer.text : answer.text.toLowerCase();

            if (userAnswer === questionAnswer) {
                return true;
            }
        }

        return false;
    }

    ngOnInit(): void {
        if (!this.question) {
            throw new Error('Attribute "question" is required.');
        }

        this.initialize();
    }

    initialize() {
        this.answer = '';
        //this.answerChange.emit(this.isAnswerCorrect());
    }
}
