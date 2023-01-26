import { Injectable } from '@angular/core';
import { LoggedInRolePermissionsProvider } from '@platform/providers/logged-in-role-permissions-provider';
import { LoggedInUserInfoProvider } from '@platform/providers/logged-in-user-info-provider';
import { Components } from '@platform/access-control/components';


@Injectable({
    providedIn: 'root'
})
export class PermissionsService {
    
    constructor(private permissionsProvider: LoggedInRolePermissionsProvider, private loggedInUserInfoProvider: LoggedInUserInfoProvider) {
    }
    
    canAccessByName(componentName) {
        if (this.loggedInUserInfoProvider.user.isSuperAdmin) {
            return true;
        }

        return this.permissionsProvider.permissions.some( p => {
                return p.component === componentName &&
                       !p.activity &&
                       p.isAllowed;
            });
    };

    canPerformByName(componentName, activityName) {
        if (this.loggedInUserInfoProvider.user.isSuperAdmin) {
            return true;
        }

        return this.canAccessByName(componentName) &&
            this.permissionsProvider.permissions.some((p) => {
                    return p.component === componentName &&
                        p.activity === activityName &&
                        p.isAllowed;
                });
    };

    canAccessComponent(component) {
        return this.canAccessByName(component._componentName);
    };

    canPerformActivity(activity) {
        return this.canPerformByName(activity._componentName, activity._activityName);
    };
    
    canAccess(componentExpr: (components: Components) => any) {
        return this.canAccessByName(componentExpr(new Components())._componentName);
    };

    canPerform(activityExpr: (components: Components) => any) {
        var activity = activityExpr(new Components());

        return this.canPerformByName(activity._componentName, activity._activityName);
    };
}
