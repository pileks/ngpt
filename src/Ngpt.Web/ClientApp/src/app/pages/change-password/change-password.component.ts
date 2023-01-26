import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { IAugurHttpRequest } from '@augur';
import { ChangePasswordController } from '@platform';

@Component({
    selector: 'app-change-password',
    templateUrl: './change-password.component.html',
    styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent {
    constructor(private changePasswordController: ChangePasswordController, private router: Router) {

        this.model = {
            oldPassword: '',
            newPassword: '',
            confirmNewPassword: ''
        };
    }
    
    request: IAugurHttpRequest;
    isOldPasswordInvalid: boolean;
    model: { oldPassword: string; newPassword: string, confirmNewPassword: string };
    isPasswordSuccessfullyChanged: boolean;

    get isChangePasswordRequestInProgress(): boolean {
        return this.request && this.request.isLoading;
    }

    async changePassword() {
        this.request = this.changePasswordController.changePassword(this.model);

        await this.request;

        if(this.request.hasErrors) {
            this.isOldPasswordInvalid = true;
        }
        else {
            this.isPasswordSuccessfullyChanged = true;
        }
    }

    isValid() {
        return this.model.oldPassword && 
            this.model.newPassword && 
            this.model.confirmNewPassword &&
            this.model.newPassword === this.model.confirmNewPassword;
    }
}
