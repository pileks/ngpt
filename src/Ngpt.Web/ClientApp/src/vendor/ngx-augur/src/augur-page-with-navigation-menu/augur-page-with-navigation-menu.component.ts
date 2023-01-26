import { Component, ContentChild, TemplateRef, ViewContainerRef, ComponentFactoryResolver, ViewChild } from '@angular/core';

@Component({
    selector: 'augur-page-with-navigation-menu',
    templateUrl: './augur-page-with-navigation-menu.component.html',
    styleUrls: ['./augur-page-with-navigation-menu.component.css']
})
export class AugurPageWithNavigationMenuComponent {
    constructor() {}

    @ContentChild('navigation', { static: true }) navigationTemplate: TemplateRef<any>;
    @ContentChild('content', { static: true }) contentTemplate: TemplateRef<any>;
}