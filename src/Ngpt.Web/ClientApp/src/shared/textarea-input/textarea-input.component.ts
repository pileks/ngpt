import { Attribute, Component, EventEmitter, Input, OnInit, Optional, Output, Self } from '@angular/core';
import { AbstractControl, ControlValueAccessor, NgControl, NG_VALUE_ACCESSOR, NG_VALIDATORS, Validator, ValidationErrors } from '@angular/forms';

@Component({
    selector: 'app-textarea-input',
    templateUrl: './textarea-input.component.html',
    styleUrls: ['./textarea-input.component.css'],
    providers: [{
        provide: NG_VALUE_ACCESSOR,
        useExisting: TextAreaInputComponent,
        multi: true
    },
    {
        provide: NG_VALIDATORS,
        multi: true,
        useExisting: TextAreaInputComponent
    }]
})
export class TextAreaInputComponent implements ControlValueAccessor, Validator {

    constructor(@Attribute('name') public name: string) {

    }

    @Input() label: string = '';
    @Input() required: boolean;
    @Input() rows: number = 2;
    @Input() disabled: boolean = false;

    ngOnInit(): void {
    }

    value: string;

    onChange = (value) => { };

    onTouched = () => { };

    get innerId(): string { return `${this.name}-inner`; }
    get isRequired(): boolean { return this.required !== undefined; }

    updateChanges() {
        this.onChange(this.value);
    }

    writeValue(value): void {
        this.value = value;
    }

    registerOnChange(fn): void {
        this.onChange = fn;
    }

    registerOnTouched(fn): void {
        this.onTouched = fn;
    }

    validate(control: AbstractControl): ValidationErrors {
        return {};
    }
}
