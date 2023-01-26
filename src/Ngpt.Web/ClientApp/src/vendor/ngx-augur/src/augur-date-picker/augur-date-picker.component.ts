import { Component, Input, EventEmitter, Output, OnInit, forwardRef } from '@angular/core';
import * as moment from 'moment';
import { NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
    selector: 'augur-date-picker',
    templateUrl: './augur-date-picker.component.html',
    styleUrls: ['./augur-date-picker.component.css'],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => AugurDatePickerComponent),
            multi: true
        }
    ]
})
export class AugurDatePickerComponent implements ControlValueAccessor {
    @Output() onSelect = new EventEmitter();
    @Output() onUnselect = new EventEmitter();

    private modelValue: Date;
    @Output() modelChange = new EventEmitter();

    @Input()
    get model(): Date {
        return this.modelValue;
    }
    set model(val: Date) {
        if (val) {
            let date = moment(val);

            this.localModelValue = new NgbDate(date.year(), date.month() + 1, date.date());
        } else {
            this.localModelValue = undefined;
        }

        this.modelValue = val;

        this.modelChange.emit(this.model);

        if (val) {
            this.onSelect.emit(this.model);
        } else {
            this.onUnselect.emit();
        }

        this.onChange(this.modelValue);
        this.onTouch(this.modelValue);
    }

    private localModelValue: NgbDate;

    get localModel(): NgbDate {
        return this.localModelValue;
    }
    set localModel(val: NgbDate) {
        this.localModelValue = val;

        this.model = val
            ? moment()
                  .year(this.localModelValue.year)
                  .month(this.localModelValue.month - 1)
                  .date(this.localModelValue.day)
                  .toDate()
            : null;
    }

    @Input() defaultToToday: boolean;
    @Input() placeholder: string;
    @Input() disabled: boolean;
    @Input() required: boolean;
    @Input() classes: string;

    ngOnInit() {
        setTimeout(() => {
            if (this.defaultToToday && !this.model) {
                this.model = moment().toDate();
            }
        });
    }

    public removeSelection() {
        this.localModel = null;
    }

    onChange: any = () => {};
    onTouch: any = () => {};
    get value() {
        return this.model;
    }
    set value(val) {
        this.model = val;
    }

    writeValue(value: any) {
        this.value = value;
    }
    registerOnChange(fn: any) {
        this.onChange = fn;
    }
    registerOnTouched(fn: any) {
        this.onTouch = fn;
    }
}
