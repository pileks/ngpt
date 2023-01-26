import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MultipleChoiceQuestion } from '@src/app/entities/multiple-choice-question';
import { MultipleChoiceQuestionAnswerPart } from '@src/app/entities/multiple-choice-question-answer-part';
import { MultipleChoiceQuestionPart } from '@src/app/entities/multiple-choice-question-part';
import { MutlipleChoiceQuestionAnswerPartOption } from '@src/app/entities/mutliple-choice-question-answer-part-option';
import IResettable from '@src/app/players/resettable-question-player';

@Component({
    selector: 'app-multiple-choice-question-player',
    templateUrl: './multiple-choice-question-player.component.html',
    styleUrls: ['./multiple-choice-question-player.component.css']
})
export class MultipleChoiceQuestionPlayerComponent implements OnInit, IResettable {

    constructor() { }

    @Output() answerChange: EventEmitter<boolean> = new EventEmitter();
    @Input() question: MultipleChoiceQuestion;
    @Input() instruction: string = '';

    parts: MultipleChoiceQuestionPartWithAnswer[];

    reset(): void {
        this.initialize();
    }

    initialize() {
        this.parts = this.question.parts.map((x): MultipleChoiceQuestionPartWithAnswer => ({
            questionPart: x,
            answer: null
        }));
        //this.answerChange.emit(this.areAllAnswersCorrect());
    }

    ngOnInit(): void {
        if (!this.question) {
            throw new Error('Attribute "question" is required.')
        }

        this.initialize();
    }

    onChange() {
        this.answerChange.emit(this.areAllAnswersCorrect());
    }

    areAllAnswersCorrect() {
        return this.parts
            .filter(x => !!x.questionPart?.answerPart)
            .every(x => this.isAnswerCorrect(x));
    }

    isAnswerCorrect(part: MultipleChoiceQuestionPartWithAnswer): boolean {
        return part.answer && part.answer.isCorrect;
    }
}

type MultipleChoiceQuestionPartWithAnswer = {
    questionPart: MultipleChoiceQuestionPart,
    answer: MutlipleChoiceQuestionAnswerPartOption
}