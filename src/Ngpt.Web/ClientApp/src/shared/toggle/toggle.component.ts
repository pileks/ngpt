import { Component, Input } from '@angular/core';
import { ControlValueAccessor } from '@angular/forms';
import { Validator, ValidationErrors } from '@angular/forms';
import { AbstractControl, NG_VALUE_ACCESSOR, NG_VALIDATORS } from '@angular/forms';
import { Attribute } from '@angular/core';

@Component({
    selector: 'app-toggle',
    templateUrl: './toggle.component.html',
    styleUrls: ['./toggle.component.css'],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            multi: true,
            useExisting: ToggleComponent
        },
        {
            provide: NG_VALIDATORS,
            multi: true,
            useExisting: ToggleComponent
        }
    ]
})
export class ToggleComponent implements ControlValueAccessor, Validator {

    isSelected = false;

    @Input()
    label: string = '';

    onChange = (quantity) => { };

    onTouched = () => { };

    touched = false;

    disabled = false;

    toggle() {
        this.markAsTouched();
        if (!this.disabled) {
            this.isSelected = !this.isSelected;
            this.onChange(this.isSelected);
        }
    }

    writeValue(value: boolean) {
        this.isSelected = value;
    }

    registerOnChange(onChange: any) {
        this.onChange = onChange;
    }

    registerOnTouched(onTouched: any) {
        this.onTouched = onTouched;
    }

    markAsTouched() {
        if (!this.touched) {
            this.onTouched();
            this.touched = true;
        }
    }

    setDisabledState(disabled: boolean) {
        this.disabled = disabled;
    }

    validate(control: AbstractControl): ValidationErrors {
        return {};
    }

}
