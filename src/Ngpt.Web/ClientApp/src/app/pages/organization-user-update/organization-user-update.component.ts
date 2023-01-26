import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AugurHttpRequest, EntityUpdateComponent } from '@augur';
import { RouteParams } from '@shared';
import { AccountInfo, LoggedInUserInfoProvider } from '@platform';
import { User } from '@platform';
import { OrganizationUsersController } from '@src/app/web-api-controllers/organization-users/organization-users-controller';
import { OrganizationUserModel } from '@src/app/web-api-controllers/organization-users/models/organization-user-model';

@Component({
    templateUrl: './organization-user-update.component.html',
    styleUrls: ['./organization-user-update.component.css']
})
export class OrganizationUserUpdateComponent implements OnInit {

    constructor(private route: ActivatedRoute, private router: Router,
        private organizationUsersController: OrganizationUsersController,
        private loggedInUserProvider: LoggedInUserInfoProvider,
        private routeParams: RouteParams) {

    }

    get isEntityLoading(): boolean {
        return this.request && this.request.isLoading;
    }

    get isThisUserLoggedInUser(): boolean {
        return this.loggedInUserProvider.user.id === Number(this.route.snapshot.paramMap.get('id'));
    }

    protected initNewEntity() {
        this.entity = new OrganizationUserModel();
      }

    isValid() {
        return this.entity.email;
    }

    submit() {
        if (this.entity.id) {
            this.update();
        } else {
            this.create();
        }
    }

    
    @Input() id: number;

    @Input() entity: OrganizationUserModel;

    public onDeleteWarning: string = 'Are you sure you want to delete this record?';
    
    protected request: AugurHttpRequest<OrganizationUserModel>;

    async ngOnInit() {
        this.id = this.id
        ? this.id
        : this.route.snapshot.paramMap.get('id')
            ? Number(this.route.snapshot.paramMap.get('id'))
            : null;

        await this.loadEntity();
    }

    public async loadEntity() {
        if (this.id && !this.entity) {
            this.entity = await this.get();
        }
        else if (!this.entity) {
            this.initNewEntity();
        }
    }

    protected async get(): Promise<OrganizationUserModel> {
        this.request = this.organizationUsersController.get(this.id);

        return this.request.promise;
    }

    public async create() {
        this.request = this.organizationUsersController.create(this.entity);
        
        await this.request.promise;

        this.router.navigate(['/organization-user/list']);
    }

    public async update() {
        this.request = this.organizationUsersController.update(this.entity);

        await this.request.promise;

        this.onUpdate();
    }

    protected onUpdate() {
        this.router.navigate(['/organization-user/list']);
    }

    protected async delete() {
        if(confirm(this.onDeleteWarning)) {
        await this.organizationUsersController.delete(this.id).promise;

        this.router.navigate(['/organization-user/list']);
        }
    }
}
