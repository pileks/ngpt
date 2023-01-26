import { Component, Inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { EntityListDataSource } from '@augur';
import { User } from '@platform';
import { UsersController } from '@platform';

@Component({
    selector: 'app-user-list',
    templateUrl: './user-list.component.html',
    styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {
    constructor(
        private usersController: UsersController,
        protected router: Router
    ) {
    }

    dataSource = new EntityListDataSource<User, User>(
        this.usersController,
        null
    );
    columnFilter: any = {};

    columns = {
        email: {
            title: 'Email',
        },
        isActive: {
            title: 'Active',
        },
        isSuperAdmin: {
            title: 'Super admin',
        },
        isOrgAdmin: {
            title: 'Organization admin',
        }
    };

    onRowClicked(entity: User): void {
        this.router.navigate(['/user/update', entity.id]);
    }

    async ngOnInit() {
        this.dataSource.loadData(this.columnFilter);
    }
}