/*
    This is an autogenerated file.
    Remove this comment if you don't want your changes to this file to be overwritten.
*/
import { IAugurEntityWithId } from '@augur';
import { MultipleChoiceQuestionTextPart } from '@src/app/entities/multiple-choice-question-text-part';
import { MultipleChoiceQuestionAnswerPart } from '@src/app/entities/multiple-choice-question-answer-part';
import { MultipleChoiceQuestion } from '@src/app/entities/multiple-choice-question';

export class MultipleChoiceQuestionPart implements IAugurEntityWithId {
    
    constructor(init?: Partial<MultipleChoiceQuestionPart>) {
        
        
        Object.assign(this, init);
    }
    
    ordinal: number;
    textPart: MultipleChoiceQuestionTextPart;
    textPartId?: number;
    answerPart: MultipleChoiceQuestionAnswerPart;
    answerPartId?: number;
    question: MultipleChoiceQuestion;
    questionId: number;
    id: number;
}