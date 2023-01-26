import { NgModule } from '@angular/core';
import { UserModule } from './entity-modules/user/user.module';
import { ReadingQuestionTextModule } from '@src/app/entity-crud/entity-modules/reading-question-text/reading-question-text.module';
import { ListeningQuestionAudioModule } from '@src/app/entity-crud/entity-modules/listening-question-audio/listening-question-audio.module';
import { ListeningQuestionModule } from '@src/app/entity-crud/entity-modules/listening-question/listening-question.module';
import { ReadingQuestionModule } from '@src/app/entity-crud/entity-modules/reading-question/reading-question.module';
import { UseOfLanguageQuestionModule } from '@src/app/entity-crud/entity-modules/use-of-language-question/use-of-language-question.module';
import { EditorsModule } from '@src/app/editors/editors.module';
import { InstructionModule } from '@src/app/entity-crud/entity-modules/instruction/instruction.module';

@NgModule({
    declarations: [
    ],
    imports: [
        UserModule,
        ReadingQuestionTextModule,
        ListeningQuestionAudioModule,
        ListeningQuestionModule,
        ReadingQuestionModule,
        UseOfLanguageQuestionModule,
        EditorsModule,
        InstructionModule
    ],
    exports: [
    ]
})
export class EntityCrudModule { }
