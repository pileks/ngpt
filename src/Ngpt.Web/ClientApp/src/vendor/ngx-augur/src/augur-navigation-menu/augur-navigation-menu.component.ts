import { Component, Inject } from '@angular/core';
import { AugurNavigationMenuItem } from '../augur-navigation-menu-item';
import { I_AUGUR_NAVIGATION_MENU_DEFINITION, IAugurNavigationMenuDefinition } from '../i-augur-navigation-menu-definition';

@Component({
    selector: 'augur-navigation-menu',
    templateUrl: './augur-navigation-menu.component.html',
    styleUrls: ['./augur-navigation-menu.component.css']
})
export class AugurNavigationMenuComponent {
    constructor(@Inject(I_AUGUR_NAVIGATION_MENU_DEFINITION) private menuDefinition: IAugurNavigationMenuDefinition) {
    }

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

    collapse() {
        this.isExpanded = false;
    }

    toggle() {
        this.isExpanded = !this.isExpanded;
    }

    onFocusOut() {
        this.isExpanded = false;
    }
}
