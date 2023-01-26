import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'augur-number-input',
  templateUrl: './augur-number-input.component.html',
  styleUrls: ['./augur-number-input.component.css']
})
export class AugurNumberInputComponent {

  constructor() { }

  @Input() value: string;
  @Input() placeholder: string;
  @Input() disabled: boolean;

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
