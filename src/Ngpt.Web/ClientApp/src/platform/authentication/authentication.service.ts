import { Injectable } from '@angular/core';
import { AugurLocalStorageManager } from '@augur';
import { AugurHttpRequest } from '@augur';
import { LoggedInUserInfoProvider } from '@platform/providers/logged-in-user-info-provider';
import { AuthenticationController } from '@platform/web-api-controllers/identity/authentication/authentication-controller';
import { SignInModel } from '@platform/web-api-controllers/identity/authentication/models/sign-in-model';
import { SignInResponseModel } from '@platform/models/sign-in-response-model';

@Injectable({
    providedIn: 'root'
})
export class AuthenticationService {
    constructor(
        private authenticationController: AuthenticationController,
        private localStorageManager: AugurLocalStorageManager,
        private loggedInUserProvider: LoggedInUserInfoProvider
    ) {}

    signIn(model: SignInModel) {
        this.clearStoredUserAndToken();

        var request = this.authenticationController.signIn(model);

        return this.processSignInRequest(request);
    }
    
    processSignInRequest(request: AugurHttpRequest<SignInResponseModel>) {
        this.loggedInUserProvider.isUserInfoLoading = true;

        request.promise = request.promise.then((model: SignInResponseModel) => {
            if (!request.hasErrors) {
                this.localStorageManager.store(model.token, 'token');
                this.localStorageManager.store(model.user, 'user');

                this.loggedInUserProvider.loadUserInfo(model.user);
            }

            return model;
        });

        return request;
    }

    signOut() {
        return this.authenticationController.signOut().promise.then(async r => {
            this.clearStoredUserAndToken();
        });
    }

    async fetchAndLoadLoggedInUser() {
        this.loggedInUserProvider.isUserInfoLoading = true;

        const request = this.authenticationController.getLoggedInUserInfo();

        request.promise.then(
            user => {
                if (user) {
                    this.localStorageManager.store(user, 'user');

                    this.loggedInUserProvider.loadUserInfo(user);
                } else {
                    this.clearStoredUserAndToken();
                }
            },
            () => {
                this.clearStoredUserAndToken();
            }
        );
    }

    clearStoredUserAndToken() {
        this.loggedInUserProvider.unloadUserInfo();
        this.localStorageManager.clear();
    }
}
