import { Component, Input, OnInit } from '@angular/core';
import { SingleGapQuestion } from '@src/app/entities/single-gap-question';
import { AbstractControl, ControlValueAccessor, NgControl, NG_VALUE_ACCESSOR, NG_VALIDATORS, Validator, ValidationErrors } from '@angular/forms';
import { SingleGapQuestionAnswer } from '@src/app/entities/single-gap-question-answer';
import { InstructionsController } from '@src/app/web-api-controllers/instructions/instructions-controller';
import { Language } from '@src/app/entities/language';
import { UseOfLanguageQuestionType } from '@src/app/enums/use-of-language-question-type';

@Component({
    selector: 'app-single-gap-question-editor',
    templateUrl: './single-gap-question-editor.component.html',
    styleUrls: ['./single-gap-question-editor.component.css'],
    providers: [{
        provide: NG_VALUE_ACCESSOR,
        useExisting: SingleGapQuestionEditorComponent,
        multi: true
    },
    {
        provide: NG_VALIDATORS,
        multi: true,
        useExisting: SingleGapQuestionEditorComponent
    }]
})
export class SingleGapQuestionEditorComponent implements ControlValueAccessor, Validator {

    constructor(public instructionsController: InstructionsController) {
    }

    @Input() name: string = '';
    @Input() instruction: string = '';
    @Input() editorDisabled: boolean = false;

    question: SingleGapQuestion = new SingleGapQuestion({
        answers: [
            new SingleGapQuestionAnswer({ isCaseSensitive: false })
        ]
    });

    addAnswer() {
        this.question.answers.push(this.getAnswerTemplate());
        this.onModelChange();
    }

    removeAnswer(answer: SingleGapQuestionAnswer) {
        var index = this.question.answers.indexOf(answer);
        this.question.answers.splice(index, 1);
        this.onModelChange();
    }

    private getAnswerTemplate(): SingleGapQuestionAnswer {
        return new SingleGapQuestionAnswer({ isCaseSensitive: false });
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

        if (!this.question.answers || !this.question.answers.length) {
            return { atLeastOneAnswerIsRequired: true };
        }

        if (this.question.answers.some(x => !x.text)) {
            return { allAnswersNeedToHaveText: true };
        }

        if (!this.question.textAfter) {
            return { textAfterIsRequired: true };
        }

        return null;
    }
}
