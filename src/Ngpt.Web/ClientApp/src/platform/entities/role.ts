/*
    This is an autogenerated file.
    Remove this comment if you don't want your changes to this file to be overwritten.
*/
import { IAugurEntityWithId } from '@augur';
import { RoleType } from '@platform/enums/role-type';
import { Tenant } from '@platform/entities/tenant';

export class Role implements IAugurEntityWithId {
    
    constructor(init?: Partial<Role>) {
        
        
        Object.assign(this, init);
    }
    
    name: string;
    type: RoleType;
    priority: number;
    tenantId: number;
    tenant: Tenant;
    id: number;
}