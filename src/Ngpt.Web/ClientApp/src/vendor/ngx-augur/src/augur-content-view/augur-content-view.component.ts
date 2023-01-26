import { Component, OnInit, Input, EventEmitter, Output, ContentChild, TemplateRef } from '@angular/core';

@Component({
  selector: 'augur-content-view',
  templateUrl: './augur-content-view.component.html',
  styleUrls: ['./augur-content-view.component.css']
})
export class AugurContentViewComponent implements OnInit {

  constructor() { }

  @Input() loading: boolean;
  @Input() hideHeaderWhileLoading: boolean;
  @ContentChild('header', { static: true }) headerTemplate: TemplateRef<any>;
  @ContentChild('body', { static: true }) bodyTemplate: TemplateRef<any>;
 
  ngOnInit() {
  }

}
