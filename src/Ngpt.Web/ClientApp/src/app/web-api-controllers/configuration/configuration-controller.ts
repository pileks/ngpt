/*
    This is an autogenerated file.
    Remove this comment if you don't want your changes to this file to be overwritten.
*/
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { AugurHttpRequest, IAugurHttpRequest } from '@augur';


@Injectable({
    providedIn: 'root'
})
export class ConfigurationController {

    constructor(protected http: HttpClient, @Inject('BASE_URL')protected baseUrl: string) {
        
        this.getConfig = this.getConfig.bind(this);
    }

    controllerRoute: string = 'api/configuration';
    
    getConfig(): IAugurHttpRequest {
        
        return this.http.get(this.baseUrl + this.controllerRoute + `/getConfig`)
            .toAugurHttpRequest();
    }
}