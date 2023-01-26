import { Component, OnInit } from '@angular/core';
import { ListeningQuestion } from '@src/app/entities/listening-question';
import { ListeningQuestionAnswer } from '@src/app/entities/listening-question-answer';
import { ListeningQuestionAudio } from '@src/app/entities/listening-question-audio';
import { ListeningQuestionAnswerType, ListeningQuestionAnswerTypeDefinition } from '@src/app/enums/listening-question-answer-type';
import { ListeningQuestionAudiosController } from '@src/app/web-api-controllers/listening-question-audios/listening-question-audios-controller';

@Component({
    selector: 'app-listening-question-editor',
    templateUrl: './listening-question-editor.component.html',
    styleUrls: ['./listening-question-editor.component.css']
})
export class ListeningQuestionEditorComponent implements OnInit {
    
    listeningQuestionAudiosController: ListeningQuestionAudiosController;

    constructor(listeningQuestionAudiosController: ListeningQuestionAudiosController) {
        this.listeningQuestionAudiosController = listeningQuestionAudiosController;
    }

    async ngOnInit() {
    }

    question: ListeningQuestion = new ListeningQuestion({
        audio: null,
        audioId: null,
        answers: [],
        answerType: ListeningQuestionAnswerType.Text
    });

    onAudioSelected(text: ListeningQuestionAudio) {
        this.question.audio = text;
    }
    
    listAnswerTypes = Object.values(ListeningQuestionAnswerTypeDefinition);

    addAnswer() {
        let answer = new ListeningQuestionAnswer({
            isCorrect: false
        });

        this.question.answers.push(answer);
    }

    removeAnswer(answer: ListeningQuestionAnswer) {
        const index = this.question.answers.indexOf(answer);
        this.question.answers.splice(index, 1);
    }

    get areAnswersText(): boolean {
        return this.question.answerType === ListeningQuestionAnswerType.Text;
    }

    get areAnswersImages(): boolean {
        return this.question.answerType === ListeningQuestionAnswerType.Image;
    }

    get canAddAnswer(): boolean {
        return this.question.answers.length < 4;
    }
}
