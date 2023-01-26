import { HttpClient, HttpParams } from '@angular/common/http';
import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { ListeningQuestion } from '@src/app/entities/listening-question';
import { ListeningQuestionAnswer } from '@src/app/entities/listening-question-answer';
import { ReadingListeningQuestionAnswerPickerComponent } from '@src/app/players/reading-listening-question-answer-picker/reading-listening-question-answer-picker.component';
import IResettable from '@src/app/players/resettable-question-player';

@Component({
    selector: 'app-listening-question-player',
    templateUrl: './listening-question-player.component.html',
    styleUrls: ['./listening-question-player.component.css']
})
export class ListeningQuestionPlayerComponent implements OnInit, IResettable {

    constructor(private httpClient: HttpClient) { }

    @ViewChild(ReadingListeningQuestionAnswerPickerComponent) picker: IResettable;

    @Output() answerChange: EventEmitter<boolean> = new EventEmitter();
    @Input() question: ListeningQuestion;
    @Input() maxPlays: number = 2;

    audio: HTMLAudioElement;
    audioBlobUrl: string;
    canPlay: boolean = false;
    isLoading: boolean = true;
    playsCount: number = 0;

    get playsRemaining(): number { return this.maxPlays - this.playsCount };

    ngOnInit(): void {
        if (!this.question) {
            throw new Error('Attribute "question" is required.')
        }

        let params = new HttpParams();
        params = params.set('resourceId', `${this.question.audio.resourceId}`);

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
        this.audio = new Audio(url);
        this.audio.oncanplaythrough = () => {
            this.canPlay = true;
            this.audio.oncanplaythrough = () => { };
        };
    }

    play() {
        this.canPlay = false;
        this.audio.play();

        this.audio.onended = () => {
            this.playsCount += 1;

            if (this.playsCount < this.maxPlays) {
                this.canPlay = true;
            }
        };
    }

    reset(): void {
        if (this.audio) {
            this.audio.pause();
            this.audio.currentTime = 0;
        }

        this.picker.reset();

        this.initializeAudio(this.audioBlobUrl);
        this.playsCount = 0;
    }

    onAnswerChange(answer: ListeningQuestionAnswer) {
        this.answerChange.emit(answer?.isCorrect ?? false);
    }
}
