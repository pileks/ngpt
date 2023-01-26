import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router, Route } from '@angular/router';
import { Observable } from 'rxjs';
import { LoggedInUserInfoProvider } from '@platform/providers/logged-in-user-info-provider';
import { AuthenticationService } from '@platform/authentication/authentication.service';

@Injectable({
    providedIn: 'root'
})
export class RequireUserWithVerifiedEmailLoggedInRouteGuard implements CanActivate {
    constructor(protected loggedInUserInfoProvider: LoggedInUserInfoProvider, protected router: Router, protected authService: AuthenticationService) {}

    canActivate(nextRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
        if (this.loggedInUserInfoProvider.isUserInfoLoading) {
            return this.loggedInUserInfoProvider.onNextUserLoad().then(
                user => {
                    return this.canAccess(nextRoute);
                },
                () => {
                    this.router.navigate(['/login'], { queryParams: { url: nextRoute.url } });

                    return false;
                }
            );
        } else {
            return this.canAccess(nextRoute);
        }
    }

    protected canAccess(nextRoute: ActivatedRouteSnapshot): boolean {
        if (this.loggedInUserInfoProvider.isUserLoggedIn) {
            if (this.loggedInUserInfoProvider.user.hasVerifiedEmail) {
                return true;
            } else {
                this.router.navigate(['/email-confirmation']);

                return false;
            }
        } else {
            this.authService.clearStoredUserAndToken();
          
            this.router.navigate(['/login'], { queryParams: { url: nextRoute.url } });

            return false;
        }
    }
}
