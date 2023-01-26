import { Component, OnInit, ViewChild } from '@angular/core';
import { HttpEventType, HttpClient } from '@angular/common/http';
import { ReadingQuestionTextsController } from '@src/app/web-api-controllers/reading-question-texts/reading-question-texts-controller';
import { SingleGapQuestion } from '@src/app/entities/single-gap-question';
import { MultipleChoiceQuestion } from '@src/app/entities/multiple-choice-question';
import { DragDropQuestion } from '@src/app/entities/drag-drop-question';
import { ReadingQuestionsController } from '@src/app/web-api-controllers/reading-questions/reading-questions-controller';
import { UseOfLanguageQuestionsController } from '@src/app/web-api-controllers/use-of-language-questions/use-of-language-questions-controller';
import { DragDropQuestionPlayerComponent } from '@src/app/players/drag-drop-question-player/drag-drop-question-player.component';
import IResettable from '@src/app/players/resettable-question-player';
import { MultipleChoiceQuestionPlayerComponent } from '@src/app/players/multiple-choice-question-player/multiple-choice-question-player.component';
import { SingleGapQuestionPlayerComponent } from '@src/app/players/single-gap-question-player/single-gap-question-player.component';
import { UploadedResourcesController } from '@platform';
import { ListeningQuestionsController } from '@src/app/web-api-controllers/listening-questions/listening-questions-controller';
import { ListeningQuestion } from '@src/app/entities/listening-question';
import { ListeningQuestionPlayerComponent } from '@src/app/players/listening-question-player/listening-question-player.component';
import { ReadingQuestionPlayerComponent } from '@src/app/players/reading-question-player/reading-question-player.component';
import { ReadingQuestion } from '@src/app/entities/reading-question';

@Component({
    selector: 'app-form-test',
    templateUrl: './form-test.component.html',
    styleUrls: ['./form-test.component.css']
})
export class FormTestComponent implements OnInit {
    constructor(
    ) { }
    async ngOnInit() {

    }
}
