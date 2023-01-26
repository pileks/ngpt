import { InjectionToken } from '@angular/core';
import { AugurNavigationMenuItem } from './augur-navigation-menu-item';

export let I_AUGUR_NAVIGATION_MENU_DEFINITION = new InjectionToken<IAugurNavigationMenuDefinition>('augur.iAugurNavigationMenuDefinition');

export interface IAugurNavigationMenuDefinition {
    title: string;
    iconClass: string;
    menuItems: AugurNavigationMenuItem[];
}