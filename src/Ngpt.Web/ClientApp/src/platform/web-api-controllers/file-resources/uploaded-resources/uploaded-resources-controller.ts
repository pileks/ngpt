/*
    This is an autogenerated file.
    Remove this comment if you don't want your changes to this file to be overwritten.
*/
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { AugurEntityController, AugurHttpRequest, IAugurHttpRequest } from '@augur';
import { UploadedResource } from '@platform/entities/uploaded-resource';

@Injectable({
    providedIn: 'root'
})
export class UploadedResourcesController extends AugurEntityController<UploadedResource> {

    constructor(protected http: HttpClient, @Inject('BASE_URL')protected baseUrl: string) {
        super('api/uploadedResources', http, baseUrl);
        
        this.download = this.download.bind(this);
        this.preview = this.preview.bind(this);
    }
    
    download(resourceId: number): IAugurHttpRequest {
        const queryParams = new HttpParams()
            .set('resourceId', resourceId ? resourceId.toString() : '');
        
        return this.http.get(this.baseUrl + this.controllerRoute + `/download`, { params: queryParams, observe: 'response', responseType: 'blob' })
            .toAugurHttpRequest();
    }
    
    preview(resourceId: number): IAugurHttpRequest {
        const queryParams = new HttpParams()
            .set('resourceId', resourceId ? resourceId.toString() : '');
        
        return this.http.get(this.baseUrl + this.controllerRoute + `/preview`, { params: queryParams })
            .toAugurHttpRequest();
    }
}