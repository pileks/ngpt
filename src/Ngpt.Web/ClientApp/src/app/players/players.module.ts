import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DragDropQuestionPlayerComponent } from './drag-drop-question-player/drag-drop-question-player.component';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { MultipleChoiceQuestionPlayerComponent } from './multiple-choice-question-player/multiple-choice-question-player.component';
import { SingleGapQuestionPlayerComponent } from './single-gap-question-player/single-gap-question-player.component';
import { FormsModule } from '@angular/forms';
import { ListeningQuestionPlayerComponent } from './listening-question-player/listening-question-player.component';
import { ReadingListeningQuestionAnswerPickerComponent } from './reading-listening-question-answer-picker/reading-listening-question-answer-picker.component';
import { ReadingQuestionPlayerComponent } from './reading-question-player/reading-question-player.component';
import { SingleAnswerQuestionPlayerComponent } from '@src/app/players/single-answer-question-player/single-answer-question-player.component';
import { AppSharedModule } from '@shared';
import { UseOfLanguageQuestionPlayerComponent } from './use-of-language-question-player/use-of-language-question-player.component';
import { PlacementTestPlayerComponent } from './placement-test-player/placement-test-player.component';
import { SortablejsModule } from 'ngx-sortablejs';
import { ReadingTestPlayerComponent } from './reading-test-player/reading-test-player.component';
import { ListeningTestPlayerComponent } from './listening-test-player/listening-test-player.component';

@NgModule({
    declarations: [
        DragDropQuestionPlayerComponent,
        MultipleChoiceQuestionPlayerComponent,
        SingleGapQuestionPlayerComponent,
        ListeningQuestionPlayerComponent,
        ReadingListeningQuestionAnswerPickerComponent,
        ReadingQuestionPlayerComponent,
        SingleAnswerQuestionPlayerComponent,
        UseOfLanguageQuestionPlayerComponent,
        PlacementTestPlayerComponent,
        ReadingTestPlayerComponent,
        ListeningTestPlayerComponent
    ],
    imports: [
        CommonModule,
        DragDropModule,
        FormsModule,
        AppSharedModule,
        SortablejsModule
    ],
    exports: [
        DragDropQuestionPlayerComponent,
        MultipleChoiceQuestionPlayerComponent,
        SingleGapQuestionPlayerComponent,
        ListeningQuestionPlayerComponent,
        ReadingListeningQuestionAnswerPickerComponent,
        ReadingQuestionPlayerComponent,
        SingleAnswerQuestionPlayerComponent,
        UseOfLanguageQuestionPlayerComponent,
        PlacementTestPlayerComponent,
        ReadingTestPlayerComponent,
        ListeningTestPlayerComponent
    ]
})
export class PlayersModule { }
