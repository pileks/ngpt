/*
    This is an autogenerated file.
    Remove this comment if you don't want your changes to this file to be overwritten.
*/
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { AugurEntityController, AugurHttpRequest, IAugurHttpRequest } from '@augur';
import { MultipleChoiceQuestion } from '@src/app/entities/multiple-choice-question';

@Injectable({
    providedIn: 'root'
})
export class MultipleChoiceQuestionsController extends AugurEntityController<MultipleChoiceQuestion> {

    constructor(protected http: HttpClient, @Inject('BASE_URL')protected baseUrl: string) {
        super('api/multipleChoiceQuestions', http, baseUrl);
        
        
    }
    
    
}