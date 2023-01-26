import { Component, Input } from '@angular/core';
import { AbstractControl, ControlValueAccessor, NG_VALIDATORS, NG_VALUE_ACCESSOR, ValidationErrors, Validator } from '@angular/forms';
import { IAngularMyDpOptions, IMyDate, IMyDateModel } from 'angular-mydatepicker';
import { dayjs } from '@shared/dayjs/dayjs';
import { CalendarToggleEvent } from '@shared/date-picker/calendar-toggle-event';

@Component({
    selector: 'app-date-picker',
    templateUrl: './date-picker.component.html',
    styleUrls: ['./date-picker.component.css'],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: DatePickerComponent,
            multi: true
        },
        {
            provide: NG_VALIDATORS,
            useExisting: DatePickerComponent,
            multi: true
        }
    ]
})
export class DatePickerComponent implements ControlValueAccessor, Validator {
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

    @Input()
    endOfDay: boolean = false;

    @Input()
    helpText: string = '';

    @Input()
    get disableDates(): IMyDate[] { return this._disableDates; };
    set disableDates(disableDates: IMyDate[]) {
        this._disableDates = disableDates;
        this.dpOptions.disableDates = this._disableDates;
    }

    private _disableDates: IMyDate[] = [];

    value: Date;

    localValue: IMyDateModel;

    dpOptions: IAngularMyDpOptions = {
        dateRange: false,
        dateFormat: 'dd.mm.yyyy.',
        disableDates: this.disableDates,
        stylesData: {
            selector: 'augurDp',
            styles: '.augurDp .myDpDisabled { background: #E5E7EB; color: #6B7280;}'
        }
    };

    get innerId(): string { return `${this.name}-inner`; }

    onCalendarToggle(event: number): void {
        if (event !== CalendarToggleEvent.CalendarOpened) {
            this.onTouched();
        }
    }

    onDateChange(model: IMyDateModel): void {
        if (!model) {
            this.value = null;
        } else {
            let dayModel = dayjs(model.singleDate.jsDate).utc(true);

            if (this.endOfDay) {
                dayModel = dayModel.endOf('day');
            }

            this.value = dayModel.toDate();
        }

        this.updateChanges();
    }

    updateChanges(): void {
        this.onChange(this.value);
    }

    onChange = (value: Date) => { };

    onTouched = () => { };

    writeValue(value: Date): void {
        if (value) {
            this.value = value;

            let dayModel = dayjs.utc(value);

            this.localValue = {
                isRange: false,
                singleDate: {
                    date: {
                        day: dayModel.date(),
                        month: dayModel.month() + 1,
                        year: dayModel.year()
                    }
                }
            };
        }
    }

    registerOnChange(fn: (value: any) => void): void {
        this.onChange = fn;
    };

    registerOnTouched(fn: () => void): void {
        this.onTouched = fn;
    };

    validate(control: AbstractControl): ValidationErrors {
        return {};
    }
}