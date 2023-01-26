import { Component, Input, OnInit } from '@angular/core';
import { MultipleChoiceQuestion } from '@src/app/entities/multiple-choice-question';
import { MultipleChoiceQuestionAnswerPart } from '@src/app/entities/multiple-choice-question-answer-part';
import { MultipleChoiceQuestionPart } from '@src/app/entities/multiple-choice-question-part';
import { MultipleChoiceQuestionTextPart } from '@src/app/entities/multiple-choice-question-text-part';
import { MutlipleChoiceQuestionAnswerPartOption } from '@src/app/entities/mutliple-choice-question-answer-part-option';
import { AbstractControl, ControlValueAccessor, NgControl, NG_VALUE_ACCESSOR, NG_VALIDATORS, Validator, ValidationErrors } from '@angular/forms';

@Component({
    selector: 'app-multiple-choice-question-editor',
    templateUrl: './multiple-choice-question-editor.component.html',
    styleUrls: ['./multiple-choice-question-editor.component.css'],
    providers: [{
        provide: NG_VALUE_ACCESSOR,
        useExisting: MultipleChoiceQuestionEditorComponent,
        multi: true
    },
    {
        provide: NG_VALIDATORS,
        multi: true,
        useExisting: MultipleChoiceQuestionEditorComponent
    }]
})
export class MultipleChoiceQuestionEditorComponent implements ControlValueAccessor, Validator {
    constructor() { }

    @Input() editorDisabled: boolean = false;

    question: MultipleChoiceQuestion = new MultipleChoiceQuestion({
        parts: [
            new MultipleChoiceQuestionPart({
                ordinal: 0,
                textPart: new MultipleChoiceQuestionTextPart({
                    text: 'asdf'
                }),
                answerPart: null,
            })
        ]
    });

    get isLastPartText(): boolean {
        return this.question && this.question.parts.length && !this.question.parts[this.question.parts.length - 1].answerPart;
    }
    get isLastPartAnswer(): boolean {
        return this.question && this.question.parts.length && !this.question.parts[this.question.parts.length - 1].textPart;
    }

    addTextPart() {
        var part = new MultipleChoiceQuestionPart({
            ordinal: this.question.parts.length
        });

        part.textPart = new MultipleChoiceQuestionTextPart({
            text: ''
        });

        this.question.parts.push(part);
        this.updateChanges();
    }

    addAnswerPart() {
        var part = new MultipleChoiceQuestionPart({
            ordinal: this.question.parts.length
        });

        part.answerPart = new MultipleChoiceQuestionAnswerPart({
            options: [
                new MutlipleChoiceQuestionAnswerPartOption({
                    text: '',
                    isCorrect: true
                }),
                new MutlipleChoiceQuestionAnswerPartOption({
                    text: '',
                    isCorrect: false
                })
            ]
        });

        this.question.parts.push(part);
        this.updateChanges();
    }

    addPart() {
        var part = new MultipleChoiceQuestionPart({
            ordinal: this.question.parts.length
        });

        if (this.isLastPartText) {
            part.answerPart = new MultipleChoiceQuestionAnswerPart({
                options: [
                    new MutlipleChoiceQuestionAnswerPartOption({
                        text: '',
                        isCorrect: true
                    }),
                    new MutlipleChoiceQuestionAnswerPartOption({
                        text: '',
                        isCorrect: false
                    })
                ]
            });
        } else {
            part.textPart = new MultipleChoiceQuestionTextPart({
                text: ''
            });
        }

        this.question.parts.push(part);
        this.updateChanges();
    }

    removePart() {
        this.question.parts.pop();
        this.updateChanges();
    }

    get canRemovePart(): boolean {
        return this.question && this.question.parts.length > 0;
    }

    addOption(answerPart: MultipleChoiceQuestionAnswerPart) {
        answerPart.options.push(new MutlipleChoiceQuestionAnswerPartOption({
            text: '',
            isCorrect: false
        }));
        this.updateChanges();
    }

    removeOption(answerPart: MultipleChoiceQuestionAnswerPart) {
        answerPart.options.pop();
        this.updateChanges();
    }

    canRemoveOptions(answerPart: MultipleChoiceQuestionAnswerPart) {
        return answerPart.options.length > 2;
    }

    canAddOptions(answerPart: MultipleChoiceQuestionAnswerPart) {
        return answerPart.options.length < 4;
    }

    sortedParts() {
        return this.question.parts.sort((a, b) => (a.ordinal - b.ordinal));
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

        if (!this.question.parts.length) {
            return { questionPartsMustExist: true };
        }

        for (let part of this.question.parts) {
            if (part.textPart) {
                if (!part.textPart.text) {
                    return { questionTextPartMustHaveText: true };
                }
            }

            if (part.answerPart) {
                if (!part.answerPart.options.length) {
                    return { questionAnswerPartMustHaveOptions: true };
                }

                if (!part.answerPart.options.some(x => x.isCorrect)) {
                    return { questionAnswerPartMustHaveCorrectOption: true };
                }

                for (let option of part.answerPart.options) {
                    if (!option.text) {
                        return { questionAnswerPartOptionMustHaveText: true };
                    }
                }
            }
        }

        return null;
    }
}
