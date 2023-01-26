import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'augur-text-input',
  templateUrl: './augur-text-input.component.html',
  styleUrls: ['./augur-text-input.component.css']
})
export class AugurTextInputComponent {

  constructor() { }

  @Input() value: string;
  @Input() placeholder: string;
  @Input() disabled: boolean;
  @Input() type: string;

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
