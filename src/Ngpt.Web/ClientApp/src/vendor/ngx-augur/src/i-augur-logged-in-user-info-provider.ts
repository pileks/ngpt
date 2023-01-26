import { InjectionToken } from '@angular/core';
import { IAugurLoggedInUserInfo } from './i-augur-logged-in-user-info';
import { Subject } from 'rxjs';

export const I_AUGUR_LOGGED_IN_USER_INFO_PROVIDER = new InjectionToken<IAugurLoggedInUserInfoProvider>('augur.iAugurLoggedInUserInfoProvider');

export interface IAugurLoggedInUserInfoProvider {
    user: IAugurLoggedInUserInfo;
    isUserInfoLoading: boolean;
    isUserLoggedIn: boolean;
    $user: Subject<IAugurLoggedInUserInfo>;
}