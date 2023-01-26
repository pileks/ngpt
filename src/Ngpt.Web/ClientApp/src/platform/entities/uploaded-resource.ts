/*
    This is an autogenerated file.
    Remove this comment if you don't want your changes to this file to be overwritten.
*/
import { IAugurEntityWithId } from '@augur';
export class UploadedResource implements IAugurEntityWithId {
    
    constructor(init?: Partial<UploadedResource>) {
        
        
        Object.assign(this, init);
    }
    
    name: string;
    guid: string;
    mimeType: string;
    createdOn: Date;
    isInUse: boolean;
    id: number;
}