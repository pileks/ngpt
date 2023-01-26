import { Component, Inject } from '@angular/core';
import { AugurNavigationMenuItem } from '@augur';
import { NavigationMenuDefinition } from '@shared/navigation-menu-definition';
import { I_AUGUR_NAVIGATION_MENU_DEFINITION, IAugurNavigationMenuDefinition } from '@src/vendor/ngx-augur/src/i-augur-navigation-menu-definition';

@Component({
    selector: 'app-navigation-menu',
    templateUrl: './navigation-menu.component.html',
    styleUrls: ['./navigation-menu.component.css']
})
export class NavigationMenuComponent {
    constructor(@Inject(I_AUGUR_NAVIGATION_MENU_DEFINITION) private menuDefinition: NavigationMenuDefinition) {
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
