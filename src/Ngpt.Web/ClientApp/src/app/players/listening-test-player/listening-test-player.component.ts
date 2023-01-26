import { HttpClient, HttpParams } from '@angular/common/http';
import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { ListeningQuestion } from '@src/app/entities/listening-question';
import { ListeningQuestionAnswer } from '@src/app/entities/listening-question-answer';
import { ListeningQuestionAudio } from '../../entities/listening-question-audio';

@Component({
    selector: 'app-listening-test-player',
    templateUrl: './listening-test-player.component.html',
    styleUrls: ['./listening-test-player.component.css']
})
export class ListeningTestPlayerComponent implements OnInit {

    constructor(private httpClient: HttpClient) { }

    @Output() onComplete: EventEmitter<number> = new EventEmitter();
    @Input() questions: ListeningQuestion[];
    @Input() audio: ListeningQuestionAudio;
    @Input() maxPlays: number = 2;

    audioElement: HTMLAudioElement;
    audioBlobUrl: string;
    canPlay: boolean = false;
    isLoading: boolean = true;
    playsCount: number = 0;

    get playsRemaining(): number { return this.maxPlays - this.playsCount };

    correctAnswers: boolean[];
    isSubmitted: boolean = false;

    ngOnInit(): void {
        if (!this.audio) {
            throw new Error('Attribute "audio" is required.')
        }

        this.correctAnswers = this.questions.map(x => false);

        let params = new HttpParams();
        params = params.set('resourceId', `${this.audio.resourceId}`);

        this.httpClient
            .get(
                '/api/uploadedResources/preview',
                {
                    params: params,
                    responseType: 'blob'
                })
            .subscribe(data => {
                let url = URL.createObjectURL(data);

                this.audioBlobUrl = url;

                this.initializeAudio(url);

                this.isLoading = false;
            });

        this.isLoading = true;
    }

    initializeAudio(url: string) {
        this.canPlay = false;
        this.audioElement = new Audio(url);
        this.audioElement.oncanplaythrough = () => {
            this.canPlay = true;
            this.audioElement.oncanplaythrough = () => { };
        };
    }

    play() {
        this.canPlay = false;
        this.audioElement.play();

        this.audioElement.onended = () => {
            this.playsCount += 1;

            if (this.playsCount < this.maxPlays) {
                this.canPlay = true;
            }
        };
    }

    onAnswerChange(answer: ListeningQuestionAnswer, questionIdx: number) {
        this.correctAnswers[questionIdx] = answer.isCorrect;
        console.log(this.correctAnswers);
    }

    complete() {
        this.onComplete.emit(this.correctAnswers.filter(x => x).length);
        this.audioElement.pause();
        this.isSubmitted = true;
    }
}
