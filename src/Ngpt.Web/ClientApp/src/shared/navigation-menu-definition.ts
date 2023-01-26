import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { AugurNavigationMenuItem } from '@augur';
import { IAugurNavigationMenuDefinition } from '@augur';
import { AuthenticationService } from '@platform';

@Injectable({
    providedIn: 'root'
})
export class NavigationMenuDefinition implements IAugurNavigationMenuDefinition {

    title: string = 'Ngpt';
    iconClass: string = 'logo';

    menuItems: AugurNavigationMenuItem[] = [];

    constructor(
        private authenticationService: AuthenticationService,
        private router: Router
    ) {
        this.menuItems = [
            new AugurNavigationMenuItem({
                text: 'navigationMenu.home',
                faIcon: 'home',
                route: '/home',
                requireUserLoggedIn: true
            }),
            new AugurNavigationMenuItem({
                text: 'navigationMenu.about',
                faIcon: 'info',
                route: '/about',
                requireUserLoggedIn: true,
                separatorAfter: true
            }),
            new AugurNavigationMenuItem({
                text: 'navigationMenu.formTest',
                faIcon: 'info',
                route: '/form-test',
                requireUserLoggedIn: true
            }),
            new AugurNavigationMenuItem({
                text: 'navigationMenu.useOfLanguageQuestions',
                faIcon: 'info',
                route: '/use-of-language-question/list',
                requireUserLoggedIn: true
            }),
            new AugurNavigationMenuItem({
                text: 'navigationMenu.readingQuestionTexts',
                faIcon: 'info',
                route: '/reading-question-text/list',
                requireUserLoggedIn: true
            }),
            new AugurNavigationMenuItem({
                text: 'navigationMenu.listeningAudio',
                faIcon: 'info',
                route: '/listening-question-audio/list',
                requireUserLoggedIn: true
            }),
            new AugurNavigationMenuItem({
                text: 'navigationMenu.myProfile',
                faIcon: 'user',
                route: '/my-profile',
                requireUserLoggedIn: true,
                separatorAfter: true
            }),
            new AugurNavigationMenuItem({
                text: 'navigationMenu.organizationUsers',
                faIcon: 'users',
                route: '/organization-user/list',
                requireOrgAdminLoggedIn: true
            }),
            new AugurNavigationMenuItem({
                text: 'navigationMenu.users',
                faIcon: 'users',
                route: '/user/list',
                requireSuperAdminLoggedIn: true
            }),
            new AugurNavigationMenuItem({
                text: 'navigationMenu.instructions',
                faIcon: 'wrench',
                route: '/instruction/list',
                requireSuperAdminLoggedIn: true
            }),
            // new AugurNavigationMenuItem({
            //     text: 'Settings',
            //     faIcon: 'wrench',
            //     route: '/settings',
            //     requireSuperAdminLoggedIn: true,
            //     separatorAfter: true
            // }),
            new AugurNavigationMenuItem({
                text: 'navigationMenu.signOut',
                isFixedToBottom: true,
                faIcon: 'sign-out-alt',
                requireUserLoggedIn: true,
                click: async () => {
                    await this.authenticationService.signOut();

                    this.router.navigate(['/login']);
                },
                separatorAfter: true
            })
        ];
    }
}
