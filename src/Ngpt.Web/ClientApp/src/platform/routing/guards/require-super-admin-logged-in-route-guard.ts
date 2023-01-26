import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { LoggedInUserInfoProvider } from '@platform/providers/logged-in-user-info-provider';
import { AuthenticationService } from '@platform/authentication/authentication.service';

@Injectable({
    providedIn: 'root'
})
export class RequireSuperAdminLoggedInRouteGuard implements CanActivate {
    constructor(private loggedInUserInfoProvider: LoggedInUserInfoProvider, private router: Router, private authService: AuthenticationService) {}

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

    private canAccess(nextRoute: ActivatedRouteSnapshot): boolean {
        if(!this.loggedInUserInfoProvider.isUserLoggedIn) {
            this.authService.clearStoredUserAndToken();

            this.router.navigate(['/login'], { queryParams: { url: nextRoute.url } });

            return false;
        }

        if (this.loggedInUserInfoProvider.user.isSuperAdmin) {
            return true;
        }
        else {
            return false;
        }
    }
}
