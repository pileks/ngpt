import { DragDropModule } from '@angular/cdk/drag-drop';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgxAugurModule } from '@augur';
import { AppSharedModule } from '@shared';
import { DragDropQuestionEditorComponent } from '@src/app/editors/drag-drop-question-editor/drag-drop-question-editor.component';
import { ListeningQuestionEditorComponent } from '@src/app/editors/listening-question-editor/listening-question-editor.component';
import { MultipleChoiceQuestionEditorComponent } from '@src/app/editors/multiple-choice-question-editor/multiple-choice-question-editor.component';
import { ReadingQuestionEditorComponent } from '@src/app/editors/reading-question-editor/reading-question-editor.component';
import { SingleGapQuestionEditorComponent } from '@src/app/editors/single-gap-question-editor/single-gap-question-editor.component';
import { PlayersModule } from '@src/app/players/players.module';
import { SingleAnswerQuestionEditorComponent } from './single-answer-question-editor/single-answer-question-editor.component';

@NgModule({
    declarations: [
        SingleGapQuestionEditorComponent,
        MultipleChoiceQuestionEditorComponent,
        DragDropQuestionEditorComponent,
        ReadingQuestionEditorComponent,
        ListeningQuestionEditorComponent,
        SingleAnswerQuestionEditorComponent
    ],
    imports: [
        FormsModule,
        AppSharedModule,
        NgxAugurModule,
        DragDropModule,
        PlayersModule
    ],
    exports: [
        SingleGapQuestionEditorComponent,
        MultipleChoiceQuestionEditorComponent,
        DragDropQuestionEditorComponent,
        SingleAnswerQuestionEditorComponent
    ]
})
export class EditorsModule { }
