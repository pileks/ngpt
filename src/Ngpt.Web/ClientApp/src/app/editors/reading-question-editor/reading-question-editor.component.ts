import { Component, OnInit } from '@angular/core';
import { ReadingQuestion } from '@src/app/entities/reading-question';
import { ReadingQuestionAnswer } from '@src/app/entities/reading-question-answer';
import { ReadingQuestionText } from '@src/app/entities/reading-question-text';
import { ReadingQuestionAnswerType, ReadingQuestionAnswerTypeDefinition } from '@src/app/enums/reading-question-answer-type';
import { ReadingQuestionTextsController } from '@src/app/web-api-controllers/reading-question-texts/reading-question-texts-controller';

@Component({
    selector: 'app-reading-question-editor',
    templateUrl: './reading-question-editor.component.html',
    styleUrls: ['./reading-question-editor.component.css']
})
export class ReadingQuestionEditorComponent implements OnInit {
    

    constructor(public readingQuestionTextsController: ReadingQuestionTextsController) {
    }

    async ngOnInit() {
    }

    question: ReadingQuestion = new ReadingQuestion({
        text: new ReadingQuestionText({
            title: 'Bacon ipsum',
            text: 'Bacon ipsum dolor amet tri-tip tail hamburger biltong bacon. Spare ribs ribeye corned beef filet mignon ham fatback. Turducken jerky spare ribs, tenderloin boudin bacon corned beef jowl. Prosciutto jowl shankle ball tip chuck rump biltong t-bone doner, picanha boudin. Pork chop jowl flank tongue, ham kielbasa filet mignon ground round meatball short ribs.'
        }),
        answerType: ReadingQuestionAnswerType.Text,
        answers: [new ReadingQuestionAnswer({
            text: '',
            ordinal: 0
        })]
    });

    onTextSelected(text) {
        this.question.text = text;
    }
    
    listAnswerTypes = Object.values(ReadingQuestionAnswerTypeDefinition);

    addAnswer() {
        let answer = new ReadingQuestionAnswer({
            isCorrect: false
        });

        this.question.answers.push(answer);
    }

    removeAnswer(answer: ReadingQuestionAnswer) {
        const index = this.question.answers.indexOf(answer);
        this.question.answers.splice(index, 1);
    }

    get areAnswersText(): boolean {
        return this.question.answerType === ReadingQuestionAnswerType.Text;
    }

    get areAnswersImages(): boolean {
        return this.question.answerType === ReadingQuestionAnswerType.Image;
    }

    get canAddAnswer(): boolean {
        return this.question.answers.length < 4;
    }
}
