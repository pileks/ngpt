/*
    This is an autogenerated file.
    Remove this comment if you don't want your changes to this file to be overwritten.
*/
import { IAugurEntityWithId } from '@augur';
import { DragDropQuestionPart } from '@src/app/entities/drag-drop-question-part';
import { User } from '@platform/entities/user';
import { Tenant } from '@platform/entities/tenant';

export class DragDropQuestion implements IAugurEntityWithId {
    
    constructor(init?: Partial<DragDropQuestion>) {
        this.parts = [];
        
        Object.assign(this, init);
    }
    
    parts: DragDropQuestionPart[];
    user: User;
    userId: number;
    tenant: Tenant;
    tenantId: number;
    id: number;
}