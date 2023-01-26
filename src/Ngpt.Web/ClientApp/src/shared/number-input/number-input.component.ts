import { Component, Input } from '@angular/core';
import { AbstractControl, ControlValueAccessor, NG_VALIDATORS, NG_VALUE_ACCESSOR, ValidationErrors, Validator } from '@angular/forms';

@Component({
    selector: 'app-number-input',
    templateUrl: './number-input.component.html',
    styleUrls: ['./number-input.component.css'],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: NumberInputComponent,
            multi: true
        },
        {
            provide: NG_VALIDATORS,
            useExisting: NumberInputComponent,
            multi: true
        }
    ]
})
export class NumberInputComponent implements ControlValueAccessor, Validator {
    constructor() {

    }

    @Input()
    label: string = '';

    @Input()
    name: string = '';

    @Input()
    step: number = 0;

    @Input()
    disabled: boolean = false;

    @Input()
    alignment: 'left'|'right' = 'left';

    @Input()
    display: 'normal'|'tabular' = 'normal';

    @Input()
    helpText: string = '';

    value: number;

    onChange = (value: number) => { };

    onTouched = () => { };

    get innerId(): string { return `${this.name}-inner`; }

    updateChanges(): void {
        this.onChange(this.value);
    }

    writeValue(value: number): void {
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