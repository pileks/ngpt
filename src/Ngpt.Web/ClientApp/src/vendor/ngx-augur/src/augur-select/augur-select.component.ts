import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'augur-select',
  templateUrl: './augur-select.component.html',
  styleUrls: ['./augur-select.component.css']
})
export class AugurSelectComponent<TEntity> implements OnInit {

  constructor() { }

  @Input() identifier: string = 'id';
  @Input() labelFrom: string;
  @Input() items: any[];
  @Input() list: any[];

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

  public options: any[];

  ngOnInit() {
    if(this.items) {
      this.options = this.items.map((e: any) => ({
        value: e[this.identifier],
        label: e[this.labelFrom]
      }));
    } else {
      this.options = this.list.map((e: any) => ({
        value: e,
        label: e
      }));
    }
  }
}
