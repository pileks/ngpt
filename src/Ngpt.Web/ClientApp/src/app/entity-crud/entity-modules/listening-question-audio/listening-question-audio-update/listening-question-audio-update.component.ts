import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EntityUpdateComponent } from '@augur';
import { ListeningQuestionAudio } from '@src/app/entities/listening-question-audio';
import { ListeningQuestionAudiosController } from '@src/app/web-api-controllers/listening-question-audios/listening-question-audios-controller';
import { LanguagesController } from '@src/app/web-api-controllers/languages/languages-controller';
import { LevelsController } from '@src/app/web-api-controllers/levels/levels-controller';
import { ListeningQuestion } from '@src/app/entities/listening-question';

@Component({
    selector: 'app-listening-question-audio-update',
    templateUrl: './listening-question-audio-update.component.html',
    styleUrls: ['./listening-question-audio-update.component.css']
})
export class ListeningQuestionAudioUpdateComponent extends EntityUpdateComponent<ListeningQuestionAudio> {

    constructor(
        route: ActivatedRoute,
        router: Router,
        public languagesController: LanguagesController,
        public levelsController: LevelsController,
        private listeningQuestionAudiosController: ListeningQuestionAudiosController
    ) {
        super('listening-question-audio', route, router, listeningQuestionAudiosController);
    }

    get isApproved(): boolean {
        return this.entity?.id && this.entity.approved;
    }

    submit() {
        if (this.entity.id) {
            this.update();
        } else {
            this.create();
        }
    }

    protected initNewEntity(): void {
        this.entity = new ListeningQuestionAudio();
    }

    onCreate() {
        this.router.navigate(['listening-question-audio/update', this.entity.id]);
    }

    isValid(): boolean {
        return !!this.entity.languageId;
    }

    cancel() {
        this.navigateToList();
    }

    addQuestion() {
        this.router.navigate(['listening-question-audio/update', this.id, 'questions', 'create']);
    }

    updateQuestion(question: ListeningQuestion) {
        if (this.isApproved) {
            return;
        }
        this.router.navigate(['listening-question-audio/update', this.id, 'questions', question.id]);
    }

    async toggleApproval() {
        if (this.entity.id) {
            this.entity.approved = await this.listeningQuestionAudiosController.toggleApproval(this.entity);

            if (this.entity.approved) {
                this.navigateToList();
            }
        }
    }

    private navigateToList() {
        this.router.navigate(['listening-question-audio/list']);
    }
}
