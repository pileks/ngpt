/*
    This is an autogenerated file.
    Remove this comment if you don't want your changes to this file to be overwritten.
*/
import { IAugurEntityWithId } from '@augur';
import { DragDropQuestion } from '@src/app/entities/drag-drop-question';

export class DragDropQuestionPart implements IAugurEntityWithId {
    
    constructor(init?: Partial<DragDropQuestionPart>) {
        
        
        Object.assign(this, init);
    }
    
    ordinal: number;
    text: string;
    isDraggable: boolean;
    question: DragDropQuestion;
    questionId: number;
    id: number;
}