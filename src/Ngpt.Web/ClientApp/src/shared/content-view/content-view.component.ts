import { Component, OnInit, Input, EventEmitter, Output, ContentChild, TemplateRef } from '@angular/core';

@Component({
  selector: 'app-content-view',
  templateUrl: './content-view.component.html',
  styleUrls: ['./content-view.component.css']
})
export class ContentViewComponent {

  constructor() { }

  @Input() loading: boolean;
  @Input() hideHeaderWhileLoading: boolean;
  @ContentChild('header', { static: true }) headerTemplate: TemplateRef<any>;
  @ContentChild('body', { static: true }) bodyTemplate: TemplateRef<any>;
}
