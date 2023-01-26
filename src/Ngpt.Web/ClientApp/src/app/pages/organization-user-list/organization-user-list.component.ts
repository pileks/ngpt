import { Component, Inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { EntityListDataSource } from '@augur';
import { User } from '@platform';
import { UsersController } from '@platform';
import { OrganizationUsersController } from '@src/app/web-api-controllers/organization-users/organization-users-controller';
import { UserCreateModel } from '@src/app/web-api-controllers/organization-users/models/user-create-model';

@Component({
    templateUrl: './organization-user-list.component.html',
    styleUrls: ['./organization-user-list.component.css']
})
export class OrganizationUserListComponent implements OnInit {
    constructor(
        private organizationUsersController: OrganizationUsersController,
        protected router: Router
    ) {
    }

    dataSource = new EntityListDataSource<User, any>(
      null,
      this.organizationUsersController.list,
    );
    columnFilter: any = {};

    columns = {
        email: {
            title: 'Email',
        },
        name: {
            title: 'Name',
        },
        isOrgAdmin: {
            title: 'Admin',
        }
    };

    onRowClicked(entity: User): void {
        this.router.navigate(['/organization-user/update', entity.id]);
    }

    create() {
        this.router.navigate(['/organization-user/create']);
    }

    async ngOnInit() {
        this.dataSource.loadData(this.columnFilter);
    }
}