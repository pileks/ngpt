import { IAugurLoggedInUserInfoProvider } from '@augur';
import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { take } from 'rxjs/operators';
import { LoggedInUserInfo } from '@platform/models/logged-in-user-info';

@Injectable({
    providedIn: 'root'
})
export class LoggedInUserInfoProvider implements IAugurLoggedInUserInfoProvider {
    constructor() {
        this.$user = new Subject<LoggedInUserInfo>();
    }

    user: LoggedInUserInfo;
    isUserInfoLoading: boolean;
    isUserLoggedIn: boolean;
    $user: Subject<LoggedInUserInfo>;

    loadUserInfo(user?: LoggedInUserInfo) {
        if(user) {
            this.isUserInfoLoading = false;

            this.user = user;

            this.isUserLoggedIn = true;
            
            this.$user.next(this.user);
        }
        else {
            this.unloadUserInfo();
        }
    }

    unloadUserInfo() {
        this.isUserInfoLoading = false;
       
        this.isUserLoggedIn = false;
       
        this.$user.next(null);
    }

    onNextUserLoad(): Promise<LoggedInUserInfo> {
        return this.$user.pipe(take(1)).toPromise();
    }
}
