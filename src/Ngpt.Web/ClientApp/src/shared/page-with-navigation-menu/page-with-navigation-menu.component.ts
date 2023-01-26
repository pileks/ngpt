import { Component, ContentChild, TemplateRef, ViewEncapsulation } from '@angular/core';
import { Inject } from '@angular/core';
import { NavigationMenuDefinition } from '@shared/navigation-menu-definition';
import { AugurNavigationMenuItem } from '@src/vendor/ngx-augur/src/augur-navigation-menu-item';
import { I_AUGUR_NAVIGATION_MENU_DEFINITION } from '@src/vendor/ngx-augur/src/i-augur-navigation-menu-definition';

@Component({
    selector: 'app-page-with-navigation-menu',
    templateUrl: './page-with-navigation-menu.component.html',
    styleUrls: ['./page-with-navigation-menu.component.css']
})

export class PageWithNavigationMenuComponent {
    constructor(@Inject(I_AUGUR_NAVIGATION_MENU_DEFINITION) private menuDefinition: NavigationMenuDefinition) {
    }

    @ContentChild('content', { static: true }) contentTemplate: TemplateRef<any>;

    get menuItems(): AugurNavigationMenuItem[] {
        return this.menuDefinition.menuItems.filter((item) => {
            return item.showIf === undefined || item.showIf;
        });
    }

    get title(): string {
        return this.menuDefinition.title;
    }

    get iconClass(): string {
        return this.menuDefinition.iconClass;
    }

    isExpanded = false;
    isCollapsing = false;
    isExpanding = false;

    collapse() {
        this.isExpanded = false;
    }

    expand() {
        this.isExpanded = true;
    }

    toggle() {
        this.isExpanded = !this.isExpanded;
    }

    onFocusOut() {
        this.isExpanded = false;
    }
}
