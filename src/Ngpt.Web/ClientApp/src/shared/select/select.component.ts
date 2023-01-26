import { Component, ContentChild, Input, TemplateRef, ViewChild } from '@angular/core';
import { NG_VALUE_ACCESSOR, NG_VALIDATORS, AbstractControl, ValidationErrors, ControlValueAccessor, Validator, NgModel } from '@angular/forms';

@Component({
    selector: 'app-select',
    templateUrl: './select.component.html',
    styleUrls: ['./select.component.css'],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: SelectComponent,
            multi: true
        },
        {
            provide: NG_VALIDATORS,
            useExisting: SelectComponent,
            multi: true
        }
    ]
})
export class SelectComponent implements ControlValueAccessor, Validator {
    constructor() {

    }

    @ContentChild('option')
    optionTemplate: TemplateRef<HTMLElement>;

    @Input()
    label: string = '';

    @Input()
    name: string = '';

    @Input()
    items: any[];

    @Input()
    itemKey: string;

    @Input()
    disabled: boolean = false;

    @Input()
    clearOnSelect: boolean = false;

    value: any;

    @ViewChild('select')
    private select: NgModel;

    onChange = (value: any) => { };

    onTouched = () => { };

    get innerId(): string { return `${this.name}-inner`; }

    updateChanges(): void {
        this.onChange(this.value);

        if (this.clearOnSelect) {
            this.select.control.setValue(undefined, {
                emitViewToModelChange: false
            });
        }
    }

    writeValue(value: any): void {
        this.value = value;
    }

    registerOnChange(fn: (value: any) => void): void {
        this.onChange = fn;
    }

    registerOnTouched(fn: () => void): void {
        this.onTouched = fn;
    }

    validate(control: AbstractControl): ValidationErrors {
        return {};
    }
}