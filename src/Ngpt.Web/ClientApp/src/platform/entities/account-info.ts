/*
    This is an autogenerated file.
    Remove this comment if you don't want your changes to this file to be overwritten.
*/
import { IAugurEntityWithId } from '@augur';
import { User } from '@platform/entities/user';
import { Tenant } from '@platform/entities/tenant';

export class AccountInfo implements IAugurEntityWithId {
    
    constructor(init?: Partial<AccountInfo>) {
        
        
        Object.assign(this, init);
    }
    
    name: string;
    user: User;
    userId: number;
    hasAcceptedTermsAndPrivacyPolicy: boolean;
    tenantId: number;
    tenant: Tenant;
    id: number;
}