import { Component, OnInit, Input, EventEmitter, Output, ContentChild, TemplateRef } from '@angular/core';

@Component({
    selector: 'augur-collapsible-card',
    templateUrl: './augur-collapsible-card.component.html',
    styleUrls: ['./augur-collapsible-card.component.css']
})
export class AugurCollapsibleCardComponent implements OnInit {

    constructor() { }

    @ContentChild('header', { static: true }) headerTemplate: TemplateRef<any>;
    @ContentChild('body', { static: true }) bodyTemplate: TemplateRef<any>;

    private isCollapsed: boolean = true;
    @Output() collapsedChange = new EventEmitter();

    @Input()
    get collapsed() {
        return this.isCollapsed;
    }
    set collapsed(val) {
        this.isCollapsed = val;
        this.collapsedChange.emit(this.isCollapsed);
    }

    ngOnInit() {
    }

    toggleCollapse() {
        this.collapsed = !this.collapsed;
    }

}
