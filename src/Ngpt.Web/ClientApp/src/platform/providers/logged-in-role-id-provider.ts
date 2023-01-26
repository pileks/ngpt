import { Injectable } from '@angular/core';
import { LoggedInUserInfoProvider } from '@platform/providers/logged-in-user-info-provider';

@Injectable({
    providedIn: 'root'
})
export class LoggedInRoleIdProvider {
    constructor(private loggedInUserInfoProvider: LoggedInUserInfoProvider) {
    }

    get roleId(): number {
        return this.loggedInUserInfoProvider.user.roleId;
    }
}
