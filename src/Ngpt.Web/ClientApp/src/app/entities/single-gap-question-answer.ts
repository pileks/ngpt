/*
    This is an autogenerated file.
    Remove this comment if you don't want your changes to this file to be overwritten.
*/
import { IAugurEntityWithId } from '@augur';
import { SingleGapQuestion } from '@src/app/entities/single-gap-question';

export class SingleGapQuestionAnswer implements IAugurEntityWithId {
    
    constructor(init?: Partial<SingleGapQuestionAnswer>) {
        
        
        Object.assign(this, init);
    }
    
    text: string;
    isCaseSensitive: boolean;
    singleGapQuestion: SingleGapQuestion;
    singleGapQuestionId: number;
    id: number;
}