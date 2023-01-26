import { Component, Input } from '@angular/core';
import { AbstractControl, ControlValueAccessor, NG_VALIDATORS, NG_VALUE_ACCESSOR, ValidationErrors, Validator } from '@angular/forms';

@Component({
    selector: 'app-text-input',
    templateUrl: './text-input.component.html',
    styleUrls: ['./text-input.component.css'],
    providers: [{
        provide: NG_VALUE_ACCESSOR,
        useExisting: TextInputComponent,
        multi: true
    },
    {
        provide: NG_VALIDATORS,
        useExisting: TextInputComponent,
        multi: true,
    }]
})
export class TextInputComponent implements ControlValueAccessor, Validator {
    constructor() {

    }


    @Input()
    label: string = '';

    @Input()
    name: string = '';

    @Input()
    placeholder: string = '';

    @Input()
    disabled: boolean = false;

    value: string;

    onChange = (value: string) => { };

    onTouched = () => { };

    get innerId(): string { return `${this.name}-inner`; }

    updateChanges(): void {
        this.onChange(this.value);
    }

    writeValue(value: string): void {
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