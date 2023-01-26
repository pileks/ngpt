import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AugurHttpRequest } from '@augur';
import { AuthenticationService, RegistrationController, SignInResponseModel } from '@platform';
import { RegistrationScreen } from '@src/app/pages/registration/registration-screen';

@Component({
    selector: 'app-registration',
    templateUrl: './registration.component.html',
    styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {
    constructor(
        private registrationController: RegistrationController,
        private router: Router,
        private route: ActivatedRoute,
        private authService: AuthenticationService
    ) {}

    RegistrationScreen = RegistrationScreen;
    currentScreen: RegistrationScreen = RegistrationScreen.Info;
    registrationModel = new RegistrationModel();
    password: string;
    isEmailTaken: boolean;
    registerRequest: AugurHttpRequest<SignInResponseModel>;

    get isLoading(): boolean {
        return this.registerRequest && this.registerRequest.isLoading;
    }

    ngOnInit() {
    }

    async register() {
        this.isEmailTaken = false;

        this.registerRequest = this.registrationController.register(
            this.registrationModel
        );

        this.registerRequest = this.authService.processSignInRequest(this.registerRequest);

        await this.registerRequest;

        if (!this.registerRequest.hasErrors) {
            this.router.navigate(['/']);
        } else {
            this.isEmailTaken = true;

            this.currentScreen = RegistrationScreen.Info;
        }
    }

    next() {
        this.currentScreen = RegistrationScreen.Consent;
    }

    back() {
        this.currentScreen = RegistrationScreen.Info;
    }

    isInfoValid() {
        return (
            this.registrationModel.email &&
            this.registrationModel.email.includes('@') &&
            this.registrationModel.password &&
            this.registrationModel.confirmedPassword &&
            this.registrationModel.password === this.registrationModel.confirmedPassword &&
            this.registrationModel.name &&
            this.registrationModel.name.trim()
        );
    }

    isConsentValid() {
        return this.registrationModel.hasAcceptedTermsAndPrivacyPolicy;
    }
}

class RegistrationModel {
    email: string;
    password: string;
    confirmedPassword: string;

    name: string;
    tenantName: string;

    hasAcceptedTermsAndPrivacyPolicy: boolean;
}
