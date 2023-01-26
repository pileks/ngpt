import { Injectable } from '@angular/core';
import { LoggedInUserInfoProvider } from '@platform/providers/logged-in-user-info-provider';
import { PermissionModel } from '@platform/models/permission-model';

@Injectable({
    providedIn: 'root'
})
export class LoggedInRolePermissionsProvider {
    constructor(private loggedInUserInfoProvider: LoggedInUserInfoProvider) {
    }

    get permissions(): PermissionModel[] {
        return this.loggedInUserInfoProvider.user.permissions;
    }
}
