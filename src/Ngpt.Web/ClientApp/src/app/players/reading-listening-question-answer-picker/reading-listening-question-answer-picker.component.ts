import { Component, ElementRef, EventEmitter, Input, OnInit, Output, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { ListeningQuestion } from '@src/app/entities/listening-question';
import { ListeningQuestionAnswer } from '@src/app/entities/listening-question-answer';
import { ReadingQuestion } from '@src/app/entities/reading-question';
import { ReadingQuestionAnswer } from '@src/app/entities/reading-question-answer';
import { SingleAnswerQuestionAnswer } from '@src/app/entities/single-answer-question-answer';
import IResettable from '@src/app/players/resettable-question-player';

type Question = ListeningQuestion | ReadingQuestion;
type Answer = ListeningQuestionAnswer | ReadingQuestionAnswer | SingleAnswerQuestionAnswer | null;

@Component({
    selector: 'app-reading-listening-question-answer-picker',
    templateUrl: './reading-listening-question-answer-picker.component.html',
    styleUrls: ['./reading-listening-question-answer-picker.component.css']
})
export class ReadingListeningQuestionAnswerPickerComponent implements OnInit, IResettable {

    constructor() { }

    @ViewChildren('test') test: QueryList<ElementRef>;

    @Output() answerChange: EventEmitter<Answer> = new EventEmitter();
    @Input() question: Question;

    selectedAnswer: Answer;
    
    ngOnInit(): void {
        if (!this.question) {
            throw new Error('Attribute "question" is required.')
        }
    }

    onAnswerChange(answer: Answer) {
        this.answerChange.emit(answer);
        this.selectedAnswer = answer;
    }

    isAnswerSelected(answer: Answer){
        return this.selectedAnswer === answer;
    }

    reset(): void {
        for (let e of this.test.toArray()) {
            e.nativeElement.checked = false;
        }
        
        this.selectedAnswer = null;

        this.answerChange.emit(null);
    }
}