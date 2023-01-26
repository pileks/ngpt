import { Injectable } from '@angular/core';
import { GridFilterEnumDefinition, IGridFilterEnumResolver } from '@augur';
import { ListeningQuestionAnswerTypeDefinition } from '@src/app/enums/listening-question-answer-type';
import { ReadingQuestionAnswerTypeDefinition } from '@src/app/enums/reading-question-answer-type';
import { SingleAnswerQuestionAnswerTypeDefinition } from '@src/app/enums/single-answer-question-answer-type';
import { UseOfLanguageQuestionTypeDefinition } from '@src/app/enums/use-of-language-question-type';

@Injectable({
    providedIn: 'root'
})
export class GridFilterEnumResolverService implements IGridFilterEnumResolver {

    constructor() { }

    resolve(enumType: string): GridFilterEnumDefinition[] {
        switch (enumType) {
            case 'UseOfLanguageQuestionType':
                return Object.values(UseOfLanguageQuestionTypeDefinition);
            case 'ListeningQuestionAnswerType':
                return Object.values(ListeningQuestionAnswerTypeDefinition);
            case 'ReadingQuestionAnswerType':
                return Object.values(ReadingQuestionAnswerTypeDefinition);
            case 'SingleAnswerQuestionAnswerType':
                return Object.values(SingleAnswerQuestionAnswerTypeDefinition);
        }

        return null;
    };
}
