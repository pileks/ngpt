/*
    This is an autogenerated file.
    Remove this comment if you don't want your changes to this file to be overwritten.
*/
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { AugurHttpRequest, IAugurHttpRequest } from '@augur';
import { GlobalSettings } from '@platform/entities/global-settings';

@Injectable({
    providedIn: 'root'
})
export class GlobalSettingsController {

    constructor(protected http: HttpClient, @Inject('BASE_URL')protected baseUrl: string) {
        
        this.get = this.get.bind(this);
        this.update = this.update.bind(this);
    }

    controllerRoute: string = 'api/globalSettings';
    
    get(): AugurHttpRequest<GlobalSettings> {
        
        return this.http.get(this.baseUrl + this.controllerRoute + `/get`)
            .toAugurHttpRequest<GlobalSettings>();
    }
    update(model: GlobalSettings): AugurHttpRequest<GlobalSettings> {
        
        return this.http.post(this.baseUrl + this.controllerRoute + `/update`, model ? model : {})
            .toAugurHttpRequest<GlobalSettings>();
    }
}