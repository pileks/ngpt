/*
    This is an autogenerated file.
    Remove this comment if you don't want your changes to this file to be overwritten.
*/
import { IAugurEntityWithId } from '@augur';
import { Level } from '@src/app/entities/level';
import { Language } from '@src/app/entities/language';
import { UseOfLanguageQuestionType } from '@src/app/enums/use-of-language-question-type';
import { MultipleChoiceQuestion } from '@src/app/entities/multiple-choice-question';
import { SingleGapQuestion } from '@src/app/entities/single-gap-question';
import { DragDropQuestion } from '@src/app/entities/drag-drop-question';
import { SingleAnswerQuestion } from '@src/app/entities/single-answer-question';
import { Instruction } from '@src/app/entities/instruction';
import { User } from '@platform/entities/user';
import { Tenant } from '@platform/entities/tenant';

export class UseOfLanguageQuestion implements IAugurEntityWithId {
    
    constructor(init?: Partial<UseOfLanguageQuestion>) {
        
        
        Object.assign(this, init);
    }
    
    level: Level;
    levelId: number;
    language: Language;
    languageId: number;
    title: string;
    approved: boolean;
    type: UseOfLanguageQuestionType;
    multipleChoiceQuestionId?: number;
    multipleChoiceQuestion: MultipleChoiceQuestion;
    singleGapQuestionId?: number;
    singleGapQuestion: SingleGapQuestion;
    dragDropQuestionId?: number;
    dragDropQuestion: DragDropQuestion;
    singleAnswerQuestionId?: number;
    singleAnswerQuestion: SingleAnswerQuestion;
    instruction: Instruction;
    instructionId: number;
    user: User;
    userId: number;
    tenant: Tenant;
    tenantId: number;
    approver: User;
    approverId?: number;
    id: number;
}