import { Component, OnInit, Input, Type, ViewChild, ContentChildren, QueryList, TemplateRef, ContentChild, ElementRef } from '@angular/core';

@Component({
  selector: 'augur-table',
  templateUrl: './augur-table.component.html',
  styleUrls: ['./augur-table.component.css']
})
export class AugurTableComponent implements OnInit {

  constructor() {

  }

  @ContentChildren('columnTemplate') columnTemplates: QueryList<TemplateRef<any>>;
  @Input() data: any[];
  @Input() columns: any;
  @Input() config: any;
  @Input() rowClasses: string;
  @Input() actionsColumnClasses: string;
  @Input() initNewItem: Function;
  @Input() shouldHideHead: boolean;
  @Input() shouldDisableDelete: boolean;
  @ViewChild('item') content: ElementRef;
  
  get hasCustomItemContent(): boolean {
    return !!this.content;
  }

  public properties: string[] = [];

  public currentCount = 0;
  public dataSource: number;


  ngOnInit() {
    for(let prop in this.columns) {
      this.properties.push(prop);
    }
  }

  public onRowClicked(entity: any): void {
  }

  public removeEntity(entity: any): void {
    this.data.splice(this.data.indexOf(entity), 1);
  }

  public add(): void {
    if(this.initNewItem) {
      this.data.push(this.initNewItem());
    } else {
      this.data.push({});
    }
  }
}
