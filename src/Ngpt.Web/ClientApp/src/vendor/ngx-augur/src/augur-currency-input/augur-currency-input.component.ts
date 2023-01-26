import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'augur-currency-input',
  templateUrl: './augur-currency-input.component.html',
  styleUrls: ['./augur-currency-input.component.css']
})
export class AugurCurrencyInputComponent {

  constructor() {
  }

  @Input() value: string;
  @Input() placeholder: string;
  @Input() disabled: boolean;
  @Input() precision: number;
  @Input() groupSeparator: string;
  @Input() decimalSeparator: string;

  @Output() onChange = new EventEmitter();

  modelValue: any;
  @Output() modelChange = new EventEmitter();

  @Input()
  get model() {
   return this.modelValue;
  }
  set model(val) {
    this.modelValue = val;
    this.modelChange.emit(this.model);
  }
}
