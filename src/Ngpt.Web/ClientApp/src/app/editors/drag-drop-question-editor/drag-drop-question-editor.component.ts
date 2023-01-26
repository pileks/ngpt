import { Component, Input, OnInit } from '@angular/core';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { DragDropQuestion } from '@src/app/entities/drag-drop-question';
import { DragDropQuestionPart } from '@src/app/entities/drag-drop-question-part';
import { AbstractControl, ControlValueAccessor, NgControl, NG_VALUE_ACCESSOR, NG_VALIDATORS, Validator, ValidationErrors } from '@angular/forms';

@Component({
    selector: 'app-drag-drop-question-editor',
    templateUrl: './drag-drop-question-editor.component.html',
    styleUrls: ['./drag-drop-question-editor.component.css'],
    providers: [{
        provide: NG_VALUE_ACCESSOR,
        useExisting: DragDropQuestionEditorComponent,
        multi: true
    },
    {
        provide: NG_VALIDATORS,
        multi: true,
        useExisting: DragDropQuestionEditorComponent
    }]
})
export class DragDropQuestionEditorComponent implements ControlValueAccessor, Validator {

    constructor() { }

    @Input() name: string = '';
    @Input() editorDisabled: boolean = false;

    question: DragDropQuestion = new DragDropQuestion({
        parts: [
            new DragDropQuestionPart({
                text: '',
                ordinal: 0,
                isDraggable: false,
            })
        ]
    });

    drop(event: CdkDragDrop<DragDropQuestionPart[]>) {
        moveItemInArray(this.question.parts, event.previousIndex, event.currentIndex);
        this.recalculateOrdinals();
        this.updateChanges();
    }

    recalculateOrdinals() {
        for (let i = 0; i < this.question.parts.length; i++) {
            this.question.parts[i].ordinal = i;
        }
    }

    removePart(part: DragDropQuestionPart) {
        const index = this.question.parts.indexOf(part);
        this.question.parts.splice(index, 1);
        this.recalculateOrdinals();
        this.updateChanges();
    }

    addPart() {
        this.question.parts.push(new DragDropQuestionPart({
            text: '',
            isDraggable: false
        }));
        this.recalculateOrdinals();
        this.updateChanges();
    }

    sortedParts() {
        return this.question.parts.sort((a, b) => (a.ordinal - b.ordinal));
    }

    get canRemovePart(): boolean {
        return this.question.parts.length > 1;
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

        if (!this.question.parts.some(x => x.isDraggable)) {
            return { questionMustHaveDraggablePart: true };
        }

        for (let part of this.question.parts) {
            if (!part.text) {
                return { questionPartsMustHaveText: true };
            }
        }

        return null;
    }
}
