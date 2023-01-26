import { IAugurNavigationMenuDefinition } from './i-augur-navigation-menu-definition';
import { AugurNavigationMenuItem } from './augur-navigation-menu-item';
import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class AugurEmptyNavigationMenuDefinition implements IAugurNavigationMenuDefinition {
    title: string;
    iconClass: string;
    menuItems: AugurNavigationMenuItem[];
}
