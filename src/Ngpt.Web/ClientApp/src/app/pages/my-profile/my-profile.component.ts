import { Component, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { IAugurHttpRequest } from '@augur';
import { MyProfileController } from '@src/app/web-api-controllers/my-profile/my-profile-controller';
import { MyProfileInfoModel } from '@src/app/web-api-controllers/my-profile/models/my-profile-info-model';
import { Subscription } from 'rxjs';
import { AccountInfosController } from '@src/app/web-api-controllers/account-infos/account-infos-controller';

@Component({
    selector: 'app-my-profile',
    templateUrl: './my-profile.component.html',
    styleUrls: ['./my-profile.component.css'],
    encapsulation: ViewEncapsulation.None
})
export class MyProfileComponent implements OnInit, OnDestroy {
    constructor(
        private readonly myProfileController: MyProfileController,
        private accountInfosController: AccountInfosController
    ) { }

    profile: MyProfileInfoModel;
    accountInfoEditor = {
        name: {
            isEditingEnabled: false,
            request: null,
            get isLoading(): boolean {
                return this.request && this.request.isLoading;
            },
            isValid(name) {
                return name && name.trim();
            }
        }
    };

    private authStateSubscription: Subscription;
    loggedIn: boolean;
    isAuthorized: boolean;

    get isProfileLoading(): boolean {
        return this.request && this.request.isLoading;
    }

    private request: IAugurHttpRequest;

    async ngOnInit() {
        this.request = this.myProfileController.getMyProfileInfo();

        this.profile = await this.request;
    }

    ngOnDestroy(): void {
        if (this.authStateSubscription) {
            this.authStateSubscription.unsubscribe();
        }
    }

    async updateName() {
        this.accountInfoEditor.name.request = this.accountInfosController.updateNameForLoggedInUser(
            { name: this.profile.accountInfo.name }
        );

        const response = await this.accountInfoEditor.name.request;
        this.profile.accountInfo.name = response.name;

        this.accountInfoEditor.name.isEditingEnabled = false;
    }
}
