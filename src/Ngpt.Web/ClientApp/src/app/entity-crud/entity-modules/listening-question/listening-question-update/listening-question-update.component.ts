import { Component, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EntityUpdateComponent } from '@augur';
import { ListeningQuestion } from '@src/app/entities/listening-question';
import { ListeningQuestionsController } from '@src/app/web-api-controllers/listening-questions/listening-questions-controller';
import { ListeningQuestionAudiosController } from
    '@src/app/web-api-controllers/listening-question-audios/listening-question-audios-controller';
import { ListeningQuestionAnswerType, ListeningQuestionAnswerTypeDefinition } from '@src/app/enums/listening-question-answer-type';
import { ListeningQuestionAudio } from '@src/app/entities/listening-question-audio';
import { ListeningQuestionAnswer } from '@src/app/entities/listening-question-answer';
import { ListeningQuestionPlayerComponent } from '@src/app/players/listening-question-player/listening-question-player.component';
import IResettable from '@src/app/players/resettable-question-player';
import { LanguagesController } from '@src/app/web-api-controllers/languages/languages-controller';

@Component({
    selector: 'app-listening-question-update',
    templateUrl: './listening-question-update.component.html',
    styleUrls: ['./listening-question-update.component.css']
})
export class ListeningQuestionUpdateComponent extends EntityUpdateComponent<ListeningQuestion> {

    constructor(
        router: Router,
        route: ActivatedRoute,
        public listeningQuestionAudiosController: ListeningQuestionAudiosController,
        public languagesController: LanguagesController,
        private listeningQuestionsController: ListeningQuestionsController,
    ) {
        super('listening-question', route, router, listeningQuestionsController);
    }

    @ViewChild(ListeningQuestionPlayerComponent) previewWindow: IResettable;

    previewQuestion: ListeningQuestion;
    isPreviewAnsweredCorrectly: boolean = false;

    get audioId(): number { return Number(this.route.snapshot.paramMap.get('id')); }

    async ngOnInit(): Promise<void> {
        super.ngOnInit();

        if (!this.id) {
            this.entity.audio = await this.listeningQuestionAudiosController.get(this.audioId);
        }
    }

    public getEntityIdFromRoute(): string {
        return this.route.snapshot.paramMap.get('questionId');
    }

    onPreviewAnswerChange(value: boolean) {
        this.isPreviewAnsweredCorrectly = value;
    }

    onAnswerTypeChange() {
        this.entity.answers = [];
    }

    preview() {
        this.previewQuestion = JSON.parse(JSON.stringify(this.entity));
        if (this.previewWindow) {
            this.previewWindow.reset();
        }
    }

    initNewEntity() {
        this.entity = new ListeningQuestion({
            answerType: ListeningQuestionAnswerType.Text,
            audioId: this.audioId
        });
    }

    onAudioSelected(audio: ListeningQuestionAudio) {
        this.entity.audio = audio;
    }

    listAnswerTypes = Object.values(ListeningQuestionAnswerTypeDefinition);

    addAnswer() {
        let answer = new ListeningQuestionAnswer({
            isCorrect: false
        });

        this.entity.answers.push(answer);
        this.recalculateAnswerOrdinals();
    }

    removeAnswer(answer: ListeningQuestionAnswer) {
        const index = this.entity.answers.indexOf(answer);
        this.entity.answers.splice(index, 1);
        this.recalculateAnswerOrdinals();
    }

    recalculateAnswerOrdinals() {
        for (var i = 0; i < this.entity.answers.length; i++) {
            this.entity.answers[i].ordinal = i;
        }
    }

    get areAnswersText(): boolean {
        return this.entity.answerType === ListeningQuestionAnswerType.Text;
    }

    get areAnswersImages(): boolean {
        return this.entity.answerType === ListeningQuestionAnswerType.Image;
    }

    get canAddAnswer(): boolean {
        return this.entity.answers.length < 4;
    }

    get isAtLeastOneAnswerCorrect(): boolean {
        return this.entity.answers.some(x => x.isCorrect);
    }

    submit() {
        if (this.entity.id) {
            this.update();
        } else {
            this.create();
        }
    }

    onCreate() {
        this.router.navigate(['listening-question-audio/update', this.audioId]);
    }

    onUpdate() {
        this.router.navigate(['listening-question-audio/update', this.audioId]);
    }

    onDelete() {
        this.router.navigate(['listening-question-audio/update', this.audioId]);
    }

    cancel() {
        this.router.navigate(['listening-question-audio/update', this.audioId]);
    }
}
