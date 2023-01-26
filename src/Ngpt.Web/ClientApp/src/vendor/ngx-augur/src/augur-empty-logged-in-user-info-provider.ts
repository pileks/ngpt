import { IAugurLoggedInUserInfoProvider } from './i-augur-logged-in-user-info-provider';
import { Injectable } from '@angular/core';
import { IAugurLoggedInUserInfo } from './i-augur-logged-in-user-info';
import { Subject } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class AugurEmptyLoggedInUserInfoProvider implements IAugurLoggedInUserInfoProvider {
    user: IAugurLoggedInUserInfo;
    isUserInfoLoading: boolean;
    isUserLoggedIn: boolean;
    $user: Subject<IAugurLoggedInUserInfo>;
}
