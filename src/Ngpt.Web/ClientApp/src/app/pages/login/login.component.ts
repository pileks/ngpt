import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AugurHttpRequest } from '@augur';
import { RouteParams } from '@shared';
import { AuthenticationService } from '@platform';
import { SignInModel } from '@platform';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})
export class LoginComponent {
    constructor(
        public authService: AuthenticationService,
        private router: Router,
        private routeParams: RouteParams
    ) {}

    signInDetails: SignInDetails = new SignInDetails();
    signInRequest: AugurHttpRequest<any>;

    get url(): string {
        return this.routeParams.getQueryParam('url');
    }

    get hasLoginFailed(): boolean {
        return this.signInRequest && this.signInRequest.hasErrors;
    }

    get isLoading(): boolean {
        return this.signInRequest && this.signInRequest.isLoading;
    }

    async signIn() {
        this.signInRequest = this.authService.signIn(this.signInDetails);

        await this.signInRequest;

        if (!this.hasLoginFailed) {
            this.navigateAway();
        }
    }

    navigateAway() {
        if (this.url) {
            this.router.navigate([this.url]);
        } else {
            this.router.navigate(['/']);
        }
    }
}

class SignInDetails extends SignInModel {
    isValid() {
        return this.email && this.password;
    }
}
