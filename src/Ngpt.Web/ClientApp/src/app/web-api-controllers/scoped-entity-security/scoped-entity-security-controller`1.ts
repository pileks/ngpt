/*
    This is an autogenerated file.
    Remove this comment if you don't want your changes to this file to be overwritten.
*/
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { AugurEntityController, AugurHttpRequest, IAugurHttpRequest } from '@augur';
import { TEntity } from '@src/app/entities/tentity';

@Injectable({
    providedIn: 'root'
})
export class ScopedEntitySecurityController`1 extends AugurEntityController<TEntity> {

    constructor(protected http: HttpClient, @Inject('BASE_URL')protected baseUrl: string) {
        super('api/scopedEntitySecurity', http, baseUrl);
        
        
    }
    
    
}