/*
    This is an autogenerated file.
    Remove this comment if you don't want your changes to this file to be overwritten.
*/
import { IAugurEntityWithId } from '@augur';
import { SingleAnswerQuestionAnswerType } from '@src/app/enums/single-answer-question-answer-type';
import { SingleAnswerQuestionAnswer } from '@src/app/entities/single-answer-question-answer';
import { User } from '@platform/entities/user';
import { Tenant } from '@platform/entities/tenant';

export class SingleAnswerQuestion implements IAugurEntityWithId {
    
    constructor(init?: Partial<SingleAnswerQuestion>) {
        this.answers = [];
        
        Object.assign(this, init);
    }
    
    questionText: string;
    answerType: SingleAnswerQuestionAnswerType;
    answers: SingleAnswerQuestionAnswer[];
    user: User;
    userId: number;
    tenant: Tenant;
    tenantId: number;
    id: number;
}