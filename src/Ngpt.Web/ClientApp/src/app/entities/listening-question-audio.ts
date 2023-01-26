/*
    This is an autogenerated file.
    Remove this comment if you don't want your changes to this file to be overwritten.
*/
import { IAugurEntityWithId } from '@augur';
import { UploadedResource } from '@platform/entities/uploaded-resource';
import { Language } from '@src/app/entities/language';
import { Level } from '@src/app/entities/level';
import { User } from '@platform/entities/user';
import { Tenant } from '@platform/entities/tenant';
import { ListeningQuestion } from '@src/app/entities/listening-question';

export class ListeningQuestionAudio implements IAugurEntityWithId {
    
    constructor(init?: Partial<ListeningQuestionAudio>) {
        this.questions = [];
        
        Object.assign(this, init);
    }
    
    title: string;
    approved: boolean;
    resource: UploadedResource;
    resourceId?: number;
    language: Language;
    languageId: number;
    level: Level;
    levelId: number;
    user: User;
    userId: number;
    tenant: Tenant;
    tenantId: number;
    approver: User;
    approverId?: number;
    questions: ListeningQuestion[];
    id: number;
}