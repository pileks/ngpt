import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EntityUpdateComponent } from '@augur';
import { RouteParams } from '@shared';
import { LoggedInUserInfoProvider } from '@platform';
import { User } from '@platform';
import { UsersController } from '@platform';

@Component({
    selector: 'app-user-update',
    templateUrl: './user-update.component.html',
    styleUrls: ['./user-update.component.css']
})
export class UserUpdateComponent extends EntityUpdateComponent<User> {

    constructor(route: ActivatedRoute, router: Router,
        private usersController: UsersController,
        private loggedInUserProvider: LoggedInUserInfoProvider,
        private routeParams: RouteParams) {

        super('user', route, router, usersController);
    }

    get isEntityLoading(): boolean {
        return this.request && this.request.isLoading;
    }

    get isThisUserLoggedInUser(): boolean {
        return this.loggedInUserProvider.user.id === Number(this.route.snapshot.paramMap.get('id'));
    }

    isValid() {
        return this.entity.email;
    }
}
