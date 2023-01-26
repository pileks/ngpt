import { NgbDateParserFormatter, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { Injectable, Inject } from '@angular/core';
import * as moment from 'moment';
import { DATE_DISPLAY_FORMAT } from './date-display-format';

export function toInteger(value: any): number {
    return parseInt(`${value}`, 10);
}

export function toString(value: any): string {
    return value !== undefined && value !== null ? `${value}` : '';
}

export function getValueInRange(value: number, max: number, min = 0): number {
    return Math.max(Math.min(value, max), min);
}

export function isString(value: any): value is string {
    return typeof value === 'string';
}

export function isNumber(value: any): value is number {
    return !isNaN(toInteger(value));
}

export function isInteger(value: any): value is number {
    return typeof value === 'number' && isFinite(value) && Math.floor(value) === value;
}

export function isDefined(value: any): boolean {
    return value !== undefined && value !== null;
}

export function padNumber(value: number) {
    if (isNumber(value)) {
        return `0${value}`.slice(-2);
    } else {
        return '';
    }
}

@Injectable({
    providedIn: 'root'
})
export class NgbDateCustomParserFormatter extends NgbDateParserFormatter {
    constructor(@Inject(DATE_DISPLAY_FORMAT) private dateDisplayFormat: string) {
        super();
    }
    
    parse(value: string): NgbDateStruct {
        if (value) {
            const dateParts = value.trim().split('/');
            if (dateParts.length === 1 && isNumber(dateParts[0])) {
                return { day: toInteger(dateParts[0]), month: null, year: null };
            } else if (dateParts.length === 2 && isNumber(dateParts[0]) && isNumber(dateParts[1])) {
                return { day: toInteger(dateParts[0]), month: toInteger(dateParts[1]), year: null };
            } else if (dateParts.length === 3 && isNumber(dateParts[0]) && isNumber(dateParts[1]) && isNumber(dateParts[2])) {
                return { day: toInteger(dateParts[0]), month: toInteger(dateParts[1]), year: toInteger(dateParts[2]) };
            }
        }
        return null;
    }

    format(date: NgbDateStruct): string {
        return date
            ? moment()
                  .date(date.day)
                  .month(date.month - 1)
                  .year(date.year)
                  .format(this.dateDisplayFormat)
            : '';
    }
}
