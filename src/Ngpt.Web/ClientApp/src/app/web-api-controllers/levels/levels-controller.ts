/*
    This is an autogenerated file.
    Remove this comment if you don't want your changes to this file to be overwritten.
*/
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { AugurEntityController, AugurHttpRequest, IAugurHttpRequest } from '@augur';
import { Level } from '@src/app/entities/level';

@Injectable({
    providedIn: 'root'
})
export class LevelsController extends AugurEntityController<Level> {

    constructor(protected http: HttpClient, @Inject('BASE_URL')protected baseUrl: string) {
        super('api/levels', http, baseUrl);
        
        
    }
    
    
}