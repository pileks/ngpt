/*
    This is an autogenerated file.
    Remove this comment if you don't want your changes to this file to be overwritten.
*/
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { AugurHttpRequest, IAugurHttpRequest } from '@augur';
import { OrganizationUserModel } from '@src/app/web-api-controllers/organization-users/models/organization-user-model';

@Injectable({
    providedIn: 'root'
})
export class OrganizationUsersController {

    constructor(protected http: HttpClient, @Inject('BASE_URL')protected baseUrl: string) {
        
        this.create = this.create.bind(this);
        this.update = this.update.bind(this);
        this.get = this.get.bind(this);
        this.delete = this.delete.bind(this);
        this.list = this.list.bind(this);
    }

    controllerRoute: string = 'api/organizationUsers';
    
    create(entity: OrganizationUserModel): AugurHttpRequest<OrganizationUserModel> {
        
        return this.http.post(this.baseUrl + this.controllerRoute + `/create`, entity ? entity : {})
            .toAugurHttpRequest<OrganizationUserModel>();
    }
    update(entity: OrganizationUserModel): AugurHttpRequest<OrganizationUserModel> {
        
        return this.http.post(this.baseUrl + this.controllerRoute + `/update`, entity ? entity : {})
            .toAugurHttpRequest<OrganizationUserModel>();
    }
    get(id: number): AugurHttpRequest<OrganizationUserModel> {
        
        return this.http.get(this.baseUrl + this.controllerRoute + `/get/${id}`)
            .toAugurHttpRequest<OrganizationUserModel>();
    }
    delete(id: number): IAugurHttpRequest {
        
        return this.http.delete(this.baseUrl + this.controllerRoute + `/${id}`)
            .toAugurHttpRequest();
    }
    list(pageNumber: number, pageSize: number, searchQuery: string, columnFilter: any): AugurHttpRequest<HttpResponse<any[]>> {
        const queryParams = new HttpParams()
            .set('pageNumber', pageNumber ? pageNumber.toString() : '')
            .set('pageSize', pageSize ? pageSize.toString() : '')
            .set('searchQuery', searchQuery ? searchQuery.toString() : '');
        
        return this.http.post(this.baseUrl + this.controllerRoute + `/list`, columnFilter ? columnFilter : {}, { params: queryParams, observe: 'response' })
            .toAugurHttpRequest<HttpResponse<any[]>>();
    }
}