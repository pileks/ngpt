import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'augur-loading-indicator',
  templateUrl: './augur-loading-indicator.component.html',
  styleUrls: ['./augur-loading-indicator.component.css']
})
export class AugurLoadingIndicatorComponent {

  constructor() { }

  @Input() loading: boolean;
  @Input() classes: string;

}
