import { Component, OnInit, OnDestroy, ViewEncapsulation } from '@angular/core';
import { Router, NavigationStart, NavigationEnd, NavigationError, NavigationCancel } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthenticationService } from '@platform';
import { LoggedInUserInfoProvider } from '@platform';
import { ServiceWorkerUpdateService } from '@platform';
import {TranslateService} from '@ngx-translate/core';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css'],
    encapsulation: ViewEncapsulation.None
})
export class AppComponent implements OnInit, OnDestroy {
    constructor(private router: Router, private authService: AuthenticationService, 
        private loggedInUserProvider: LoggedInUserInfoProvider,
        private serviceWorkerUpdateService: ServiceWorkerUpdateService,
        private translate: TranslateService) {
        // this language will be used as a fallback when a translation isn't found in the current language
        translate.setDefaultLang('en');

        // the lang to use, if the lang isn't available, it will use the current loader to get them
        translate.use('en');
        
        this.routerEventsSubscription = this.router.events.subscribe(event => {
            switch (true) {
                case event instanceof NavigationStart: {
                    this.isRouteLoading = true;
                    break;
                }

                case event instanceof NavigationEnd:
                case event instanceof NavigationCancel:
                case event instanceof NavigationError: {
                    this.isRouteLoading = false;
                    break;
                }
                default: {
                    break;
                }
            }
        });

        this.serviceWorkerUpdateService.monitorForUpdates();
    }
    
    isRouteLoading: boolean;
    
    private routerEventsSubscription: Subscription;

    async ngOnInit() {
        await this.authService.fetchAndLoadLoggedInUser();
    }

    ngOnDestroy() {
        if (this.routerEventsSubscription) {
            this.routerEventsSubscription.unsubscribe();
        }
    }

    isUserSignedIn() {
        return this.loggedInUserProvider.isUserLoggedIn;
    }
}
