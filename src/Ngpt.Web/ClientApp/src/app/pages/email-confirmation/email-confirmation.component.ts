import { HttpResponse } from '@angular/common/http';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { IAugurHttpRequest, AugurHttpRequest } from '@augur';
import { EmailVerificationController, LoggedInUserInfoProvider, AuthenticationService, ConfirmEmailVerificationCodeResultModel, SendEmailVerificationCodeResultModel, EmailVerificationErrorType, emailVerificationConfig } from '@platform';
import { RouteParams } from '@shared';

@Component({
    selector: 'app-email-confirmation',
    templateUrl: './email-confirmation.component.html',
    styleUrls: ['./email-confirmation.component.css']
})
export class EmailConfirmationComponent implements OnInit {
    constructor(private emailVerificationController: EmailVerificationController, private router: Router, private routeParams: RouteParams,
        private loggedInUserInfoProvider: LoggedInUserInfoProvider, private authService: AuthenticationService) {
    }

    get url(): string {
        return this.routeParams.getQueryParam('url');
    }

    get email(): string {
        return this.loggedInUserInfoProvider.user.email;
    }

    emailVerificationCode: string;
    isCodeInvalid: boolean;
    hasCompletedVerification: boolean;
    maximumAttemptsReached: boolean;
    dateWhenResendIsAvailable?: Date;

    get canResendEmail(): boolean {
        return this.dateWhenResendIsAvailable && new Date(this.dateWhenResendIsAvailable) <= new Date();
    }

    get isLoading(): boolean {
      return this.request && this.request.isLoading ||
        this.confirmEmailVerificationCodeRequest && this.confirmEmailVerificationCodeRequest.isLoading ||
        this.sendEmailVerificationCodeRequest && this.sendEmailVerificationCodeRequest.isLoading ||
        this.resendEmailVerificationCodeRequest && this.resendEmailVerificationCodeRequest.isLoading ||
        this.loggedInUserInfoProvider.isUserInfoLoading;
    }

    private request: IAugurHttpRequest;
    private confirmEmailVerificationCodeRequest: AugurHttpRequest<HttpResponse<ConfirmEmailVerificationCodeResultModel>>;
    private sendEmailVerificationCodeRequest: AugurHttpRequest<SendEmailVerificationCodeResultModel>;
    private resendEmailVerificationCodeRequest: AugurHttpRequest<SendEmailVerificationCodeResultModel>;

    async ngOnInit() {
        if(this.loggedInUserInfoProvider.user.hasVerifiedEmail) {
            this.navigateAway();
        }
        else {
            this.sendEmailVerificationCodeRequest = this.emailVerificationController.sendEmailVerificationCode();

            const response = await this.sendEmailVerificationCodeRequest;

            this.maximumAttemptsReached = response.maximumAttemptsReached;
            this.dateWhenResendIsAvailable = response.dateWhenResendIsAvailable;
        }
    }

    async confirmEmailVerificationCode() {
        this.isCodeInvalid = false;

        this.confirmEmailVerificationCodeRequest = this.emailVerificationController.confirmEmailVerificationCode({ code: this.emailVerificationCode.trim() });

        this.emailVerificationCode = '';

        const response = await this.confirmEmailVerificationCodeRequest;

        this.isCodeInvalid = response.body.hasErrors;

        if(!this.isCodeInvalid) {

            this.hasCompletedVerification = true;

            this.maximumAttemptsReached = false;
        }
        else if(response.body.errorType === EmailVerificationErrorType.MaximumAttemptsReached) {
            this.maximumAttemptsReached = true;
        }
        else {
            this.maximumAttemptsReached = false;
        }
    }

    navigateAway() {
        if(this.url) {
            this.router.navigate([this.url]);
        }
        else {
            this.router.navigate(['/']);
        }
    }

    isValid() {
        return this.emailVerificationCode &&
            this.emailVerificationCode.trim().length === emailVerificationConfig.verificationCodeLength;
    }

    async resendEmailVerificationCode() {
        this.isCodeInvalid = false;

        this.resendEmailVerificationCodeRequest = this.emailVerificationController.resendEmailVerificationCode();

        const response = await this.resendEmailVerificationCodeRequest;

        this.maximumAttemptsReached = false;
        this.dateWhenResendIsAvailable = response.dateWhenResendIsAvailable;
    }

    async logout() {
        await this.authService.signOut();

        this.router.navigate(['/login']);
    }
}
