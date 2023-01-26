import { Component, OnInit } from '@angular/core';
import { EventEmitter, Output, ViewChild } from '@angular/core';

import { Glicko2 } from 'glicko2'
import IResettable from '@src/app/players/resettable-question-player';
import { Input } from '@angular/core';
import { ListeningQuestion } from '../../entities/listening-question';
import { ListeningQuestionAudio } from '../../entities/listening-question-audio';
import { PlacementTest } from '@src/app/entities/placement-test';
import { PlacementTestProgressModel } from '@src/app/web-api-controllers/placement-tests/models/placement-test-progress-model';
import { PlacementTestProgressQuestionModel } from '@src/app/web-api-controllers/placement-tests/models/placement-test-progress-question-model';
import { PlacementTestsController } from '@src/app/web-api-controllers/placement-tests/placement-tests-controller';
import { ReadingQuestion } from '../../entities/reading-question';
import { ReadingQuestionText } from '../../entities/reading-question-text';
import { UseOfLanguageQuestion } from '@src/app/entities/use-of-language-question';
import { UseOfLanguageQuestionPlayerComponent } from '@src/app/players/use-of-language-question-player/use-of-language-question-player.component';

type Screen = "useOfLanguage" | "reading" | "listening";

@Component({
    selector: 'app-placement-test-player',
    templateUrl: './placement-test-player.component.html',
    styleUrls: ['./placement-test-player.component.css']
})
export class PlacementTestPlayerComponent implements OnInit {
    glicko2Settings = {
        tau: 0.5,
        // rating : default rating
        rating: 800,
        //rd : Default rating deviation 
        //     small number = good confidence on the rating accuracy
        rd: 50,
        //vol : Default volatility (expected fluctation on the player rating)
        vol: 0.06
    };

    @Input() questionsPerSeries: number = 5;

    @ViewChild(UseOfLanguageQuestionPlayerComponent) questionPlayer: IResettable;
    @Output() onTestCompleted: EventEmitter<void> = new EventEmitter();

    @Input() debug: boolean = false;
    @Input() tau: number = null;
    @Input() rd: number = null;
    @Input() vol: number = null;
    @Input() rdCutoff: number = null;

    testRdCutoff: number = 60;

    screen: Screen = "useOfLanguage";

    readingText: ReadingQuestionText;
    readingQuestions: ReadingQuestion[];

    listeningAudio: ListeningQuestionAudio;
    listeningQuestions: ListeningQuestion[];

    constructor(
        private placementTestsController: PlacementTestsController
    ) { }

    async ngOnInit() {
        console.log(this.placementTestId)
        this.isPlacementTestLoading = true;

        if (this.debug) {
            console.log("Debug enabled!");
            if (this.tau) {
                this.glicko2Settings.tau = this.tau;
                console.log(`Set tau to ${this.tau}`);
            }
            if (this.rd) {
                this.glicko2Settings.rd = this.rd;
                console.log(`Set question rd to ${this.rd}`);
            }
            if (this.vol) {
                this.glicko2Settings.vol = this.vol;
                console.log(`Set question vol to ${this.vol}`);
            }
            if (this.rdCutoff) {
                this.testRdCutoff = this.rdCutoff;
                console.log(`Set question vol to ${this.rdCutoff}`);
            }
        }

        if (isNaN(this.placementTestId)) {
            throw new TypeError("placementTestId must be a valid ID for a placement test!");
        }

        this.placementTest = await this.placementTestsController.get(this.placementTestId);

        if (!this.placementTest.completedOn) {
            this.glickoRanking = new Glicko2(this.glicko2Settings);
            this.glickoPlayer = this.glickoRanking.makePlayer(this.placementTest.rating, this.placementTest.rd, this.placementTest.vol)

            await this.loadQuestionSeries();
        } else {
            await this.goToReadingOrFurther();
            return;
        }


        this.isPlacementTestLoading = false;
    }

    @Input() placementTestId: number;

    isPlacementTestLoading: boolean = false;
    areQuestionsLoading: boolean = false;

    placementTest: PlacementTest;
    questions: UseOfLanguageQuestion[] = [];

    currentQuestionIdx: number = 0;
    isCurrentQuestionCorrectlyAnswered: boolean = false;
    canMoveToNextQuestion: boolean = false;

    answeredQuestions: PlacementTestProgressQuestionModel[] = [];

    glickoRanking: any;
    glickoPlayer: any;
    glickoMatches = [];

    async addQuestionResultToMatches(isAnsweredCorrectly: boolean) {
        const currentQuestion = this.questions[this.currentQuestionIdx];

        const opponent = this.glickoRanking.makePlayer(currentQuestion.level.rating);

        const result = isAnsweredCorrectly ? 1 : 0;

        this.glickoMatches.push([this.glickoPlayer, opponent, result]);
    }

    async updateRatings() {
        this.glickoRanking.updateRatings(this.glickoMatches);

        const oldRd = this.placementTest.rd;

        this.placementTest.rating = this.glickoPlayer.getRating();
        this.placementTest.rd = this.glickoPlayer.getRd();
        this.placementTest.vol = this.glickoPlayer.getVol();

        if (Math.abs(oldRd - this.placementTest.rd) < 5) {
            console.log("Too low RD change detected!")
            this.placementTest.rd = oldRd - 5;
        }

        this.glickoMatches = [];
        this.glickoRanking = new Glicko2(this.glicko2Settings);
        this.glickoPlayer = this.glickoRanking.makePlayer(this.placementTest.rating, this.placementTest.rd, this.placementTest.vol)
    }

    async goToNextQuestion(debugQuestionCorrectlyAnswered: boolean = null) {
        const isAnswerCorrect = debugQuestionCorrectlyAnswered ?? this.isCurrentQuestionCorrectlyAnswered;

        this.addQuestionResultToMatches(isAnswerCorrect);
        this.answeredQuestions.push({
            isAnsweredCorrectly: isAnswerCorrect,
            questionId: this.questions[this.currentQuestionIdx].id
        });

        this.currentQuestionIdx++;

        if (this.currentQuestionIdx >= this.questions.length) {
            await this.updateRatings();
            await this.updateProgress(this.shouldFinishTest());

            if (!this.shouldFinishTest()) {
                await this.loadQuestionSeries();
            } else {
                await this.goToReadingOrFurther();
                return;
            }

            this.currentQuestionIdx = 0;
        }

        this.canMoveToNextQuestion = false;
        this.isCurrentQuestionCorrectlyAnswered = false;

        if (this.questionPlayer) {
            this.questionPlayer.reset();
        }

        console.log(this.currentQuestionIdx);
    }

    async debugSkipQuestion(isAnswerCorrect: boolean) {
        await this.goToNextQuestion(isAnswerCorrect);
    }

    shouldFinishTest() {
        return this.placementTest.rd <= this.testRdCutoff;
    }

    async updateProgress(isCompleted: boolean) {
        const model = new PlacementTestProgressModel();

        model.placementTestId = this.placementTest.id;
        model.rating = this.placementTest.rating;
        model.rd = this.placementTest.rd;
        model.vol = this.placementTest.vol;
        model.questions = [...this.answeredQuestions];
        model.isCompleted = isCompleted;

        this.answeredQuestions = [];

        await this.placementTestsController.updateProgress(model);
    }

    async loadQuestionSeries() {
        this.areQuestionsLoading = true;

        const questionsResult = await this.placementTestsController.getQuestionsWithinRating(
            this.placementTest.rating,
            this.placementTest.rd,
            this.placementTest.languageId,
            this.questionsPerSeries,
            this.placementTestId
        );

        this.questions = questionsResult.questions;

        if (questionsResult.questions.length === 0) {
            await this.placementTestsController.updateProgress({
                isCompleted: true,
                placementTestId: this.placementTest.id,
                questions: [],
                rating: this.placementTest.rating,
                rd: this.placementTest.rd,
                vol: this.placementTest.vol
            });

            this.goToReadingOrFurther();
        }

        this.areQuestionsLoading = false;
    }

    async onQuestionAnswer(isCorrect: boolean) {
        this.canMoveToNextQuestion = true;
        this.isCurrentQuestionCorrectlyAnswered = isCorrect;
    }

    async goToReadingOrFurther() {
        console.log("Going to reading or further...")

        // If we should test reading, and no reading has been tested.
        if (this.placementTest.shouldTestReading && !this.placementTest.readingQuestionTextId) {
            const readingTest = await this.placementTestsController.getReadingTest(this.placementTestId);

            if (readingTest.shouldSkip) {
                await this.goToListeningOrFurther();
                return;
            }

            this.readingQuestions = readingTest.questions;
            this.readingText = readingTest.text;

            console.log("Setting screen to reading...")
            this.screen = "reading";

            this.isPlacementTestLoading = false;
        } else {
            await this.goToListeningOrFurther();
        }
    }

    async completeReading(numberOfCorrectAnswers: number) {
        console.log(`Correctly answered ${numberOfCorrectAnswers} questions.`);

        await this.placementTestsController.completeReadingTest({
            placementTestId: this.placementTestId,
            textId: this.readingText.id,
            correctAnswers: numberOfCorrectAnswers,
            totalAnswers: this.readingQuestions.length,
        })

        await this.goToListeningOrFurther();
    }

    async goToListeningOrFurther() {
        console.log("Going to listening or further...")
        // If we should test reading, and no reading has been tested.
        if (this.placementTest.shouldTestListening && !this.placementTest.listeningQuestionAudioId) {
            const listeningTest = await this.placementTestsController.getListeningTest(this.placementTestId);

            if (listeningTest.shouldSkip) {
                await this.completeTest();
                return;
            }

            this.listeningAudio = listeningTest.audio;
            this.listeningQuestions = listeningTest.questions;

            console.log("Setting screen to listening...")
            this.screen = "listening";

            this.isPlacementTestLoading = false;
        } else {
            await this.completeTest();
        }
    }

    async completeListening(numberOfCorrectAnswers: number) {
        console.log(`Correctly answered ${numberOfCorrectAnswers} questions.`);

        await this.placementTestsController.completeListeningTest({
            placementTestId: this.placementTestId,
            audioId: this.listeningAudio.id,
            correctAnswers: numberOfCorrectAnswers,
            totalAnswers: this.listeningQuestions.length,
        })

        await this.completeTest();
    }

    async completeTest() {
        this.onTestCompleted.emit();
    }
}
