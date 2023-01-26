import { Component, Input } from '@angular/core';
import { AbstractControl, ControlValueAccessor, NG_VALIDATORS, NG_VALUE_ACCESSOR, ValidationErrors, Validator } from '@angular/forms';
import { SingleAnswerQuestion } from '@src/app/entities/single-answer-question';
import { SingleAnswerQuestionAnswer } from '@src/app/entities/single-answer-question-answer';
import { SingleAnswerQuestionAnswerType, SingleAnswerQuestionAnswerTypeDefinition } from '@src/app/enums/single-answer-question-answer-type';

@Component({
    selector: 'app-single-answer-question-editor',
    templateUrl: './single-answer-question-editor.component.html',
    styleUrls: ['./single-answer-question-editor.component.css'],
    providers: [{
        provide: NG_VALUE_ACCESSOR,
        useExisting: SingleAnswerQuestionEditorComponent,
        multi: true
    },
    {
        provide: NG_VALIDATORS,
        multi: true,
        useExisting: SingleAnswerQuestionEditorComponent
    }]
})
export class SingleAnswerQuestionEditorComponent implements ControlValueAccessor, Validator {

    constructor() { }

    @Input() editorDisabled: boolean = false;

    question: SingleAnswerQuestion = new SingleAnswerQuestion({
        questionText: null,
        answers: [],
        answerType: SingleAnswerQuestionAnswerType.Text
    });

    listAnswerTypes = Object.values(SingleAnswerQuestionAnswerTypeDefinition);

    addAnswer() {
        let answer = new SingleAnswerQuestionAnswer({
            isCorrect: false
        });

        this.question.answers.push(answer);
        this.onModelChange();
    }

    removeAnswer(answer: SingleAnswerQuestionAnswer) {
        const index = this.question.answers.indexOf(answer);
        this.question.answers.splice(index, 1);
        this.onModelChange();
    }

    get areAnswersText(): boolean {
        return this.question.answerType === SingleAnswerQuestionAnswerType.Text;
    }

    get areAnswersImages(): boolean {
        return this.question.answerType === SingleAnswerQuestionAnswerType.Image;
    }

    get canAddAnswer(): boolean {
        return this.question.answers.length < 4;
    }

    onModelChange() {
        this.onChange(this.question);
    }

    //NgModel and validation
    onChange = (value) => { };

    onTouched = () => { };

    updateChanges() {
        this.onChange(this.question);
    }

    writeValue(value): void {
        this.question = value;
    }

    registerOnChange(fn): void {
        this.onChange = fn;
    }

    registerOnTouched(fn): void {
        this.onTouched = fn;
    }

    validate(control: AbstractControl): ValidationErrors | null {
        if (!this.question) {
            return null;
        }

        if (!this.question.questionText || !this.question.questionText.length) {
            return { textRequired: true };
        }

        if (!this.question.answers.length) {
            return { atLeastOneAnswerRequired: true };
        }

        if (this.question.answerType == SingleAnswerQuestionAnswerType.Text) {
            if (this.question.answers.some(x => !x.text || !x.text.length)) {
                return { allAnswersRequireText: true };
            }
        } else if (this.question.answerType == SingleAnswerQuestionAnswerType.Image) {
            if (this.question.answers.some(x => !x.imageId)) {
                return { allAnswersRequireImage: true };
            }

        }

        if (!this.question.answers.some(x => x.isCorrect)) {
            return { atLeastOneCorrectAnswerRequired: true };
        }

        return null;
    }
}