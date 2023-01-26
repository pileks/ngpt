/*
    This is an autogenerated file.
    Remove this comment if you don't want your changes to this file to be overwritten.
*/
import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { AugurHttpRequest, IAugurHttpRequest } from '@augur';
import { AugurEntityWithGridController, GridResult, GridRequestModel } from '@augur';
import { ListeningQuestionAudio } from '@src/app/entities/listening-question-audio';
import { ListeningQuestionAudioGridModel } from '@src/app/web-api-controllers/listening-question-audios/models/listening-question-audio-grid-model';

@Injectable({
    providedIn: 'root'
})
export class ListeningQuestionAudiosController extends AugurEntityWithGridController<ListeningQuestionAudio, ListeningQuestionAudioGridModel> {

    constructor(protected http: HttpClient, @Inject('BASE_URL')protected baseUrl: string) {
        super('api/listeningQuestionAudios', http, baseUrl);
        
        this.toggleApproval = this.toggleApproval.bind(this);
    }
    
    toggleApproval(question: ListeningQuestionAudio): AugurHttpRequest<boolean> {
        
        return this.http.post(this.baseUrl + this.controllerRoute + `/toggleApproval`, question ? question : {})
            .toAugurHttpRequest<boolean>();
    }
}