/*
    This is an autogenerated file.
    Remove this comment if you don't want your changes to this file to be overwritten.
*/
import { IAugurEntityWithId } from '@augur';
import { UploadedResource } from '@platform/entities/uploaded-resource';
import { ReadingQuestion } from '@src/app/entities/reading-question';

export class ReadingQuestionAnswer implements IAugurEntityWithId {
    
    constructor(init?: Partial<ReadingQuestionAnswer>) {
        
        
        Object.assign(this, init);
    }
    
    text: string;
    isCorrect: boolean;
    ordinal: number;
    image: UploadedResource;
    imageId?: number;
    question: ReadingQuestion;
    questionId: number;
    id: number;
}