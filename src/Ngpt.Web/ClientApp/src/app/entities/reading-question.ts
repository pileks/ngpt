/*
    This is an autogenerated file.
    Remove this comment if you don't want your changes to this file to be overwritten.
*/
import { IAugurEntityWithId } from '@augur';
import { ReadingQuestionText } from '@src/app/entities/reading-question-text';
import { ReadingQuestionAnswerType } from '@src/app/enums/reading-question-answer-type';
import { ReadingQuestionAnswer } from '@src/app/entities/reading-question-answer';
import { User } from '@platform/entities/user';
import { Tenant } from '@platform/entities/tenant';

export class ReadingQuestion implements IAugurEntityWithId {
    
    constructor(init?: Partial<ReadingQuestion>) {
        this.answers = [];
        
        Object.assign(this, init);
    }
    
    text: ReadingQuestionText;
    textId: number;
    questionText: string;
    answerType: ReadingQuestionAnswerType;
    answers: ReadingQuestionAnswer[];
    user: User;
    userId: number;
    tenant: Tenant;
    tenantId: number;
    id: number;
}