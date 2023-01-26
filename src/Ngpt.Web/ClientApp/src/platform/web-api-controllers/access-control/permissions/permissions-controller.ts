/*
    This is an autogenerated file.
    Remove this comment if you don't want your changes to this file to be overwritten.
*/
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { AugurHttpRequest, IAugurHttpRequest } from '@augur';
import { UpdatePermissionsModel } from '@platform/web-api-controllers/access-control/permissions/models/update-permissions-model';
import { PermissionComponentModel } from '@platform/web-api-controllers/access-control/permissions/models/permission-component-model';

@Injectable({
    providedIn: 'root'
})
export class PermissionsController {

    constructor(protected http: HttpClient, @Inject('BASE_URL')protected baseUrl: string) {
        
        this.getPermissionsForEdit = this.getPermissionsForEdit.bind(this);
        this.updatePermissions = this.updatePermissions.bind(this);
    }

    controllerRoute: string = 'api/permissions';
    
    getPermissionsForEdit(roleId: number): AugurHttpRequest<PermissionComponentModel[]> {
        const queryParams = new HttpParams()
            .set('roleId', roleId ? roleId.toString() : '');
        
        return this.http.get(this.baseUrl + this.controllerRoute + `/getPermissionsForEdit`, { params: queryParams })
            .toAugurHttpRequest<PermissionComponentModel[]>();
    }
    updatePermissions(model: UpdatePermissionsModel): AugurHttpRequest<PermissionComponentModel[]> {
        
        return this.http.post(this.baseUrl + this.controllerRoute + `/updatePermissions`, model ? model : {})
            .toAugurHttpRequest<PermissionComponentModel[]>();
    }
}