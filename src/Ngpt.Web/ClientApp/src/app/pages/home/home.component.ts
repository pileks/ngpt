import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnDestroy, OnInit } from '@angular/core';

import { DashboardController } from '@src/app/web-api-controllers/dashboard/dashboard-controller';
import { DashboardInfoModel } from '@src/app/web-api-controllers/dashboard/models/dashboard-info-model';
import { IAugurHttpRequest } from '@augur';
import { Language } from '@src/app/entities/language';
import { LanguagesController } from '@src/app/web-api-controllers/languages/languages-controller';
import { LoggedInUserInfoModel } from '@platform';
import { LoggedInUserInfoProvider } from '@platform';
import { PermissionsService } from '@platform';
import { PlacementTestsController } from '@src/app/web-api-controllers/placement-tests/placement-tests-controller';
import { Subscription } from 'rxjs';
import { TranslateService } from '@ngx-translate/core';
import { UseOfLanguageQuestion } from '@src/app/entities/use-of-language-question';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit, OnDestroy {
    constructor(
        private dashboardController: DashboardController,
        private loggedInUserProvider: LoggedInUserInfoProvider,
        private permissionsService: PermissionsService,
        private translate: TranslateService,
        private placementTestsController: PlacementTestsController,
        private router: Router,
        private languagesController: LanguagesController,
        private route: ActivatedRoute
    ) { }

    title = 'Ngpt';

    get isLoading() {
        return this.request && this.request.isLoading;
    }

    request: IAugurHttpRequest;

    user: LoggedInUserInfoModel;
    dashboardInfo: DashboardInfoModel;

    isPlacementTestLoading: boolean;

    languages: Language[];
    isDebugModeEnabled: boolean = false;

    playerRating: number = 800;
    playerRd: number = 200;
    playerVol: number = 0.06;

    private userSubscription: Subscription;

    async ngOnInit() {
        this.user = this.loggedInUserProvider.user;

        this.dashboardInfo = await this.getDashboardInfo();

        this.languages = await this.languagesController.getAll();

        this.isDebugModeEnabled = this.route.snapshot.queryParamMap.get("debug") === "true";
    }

    ngOnDestroy() {
        if (this.userSubscription) {
            this.userSubscription.unsubscribe();
        }
    }

    useEnglishLang() {
        this.translate.use('en');
    }

    useSpanishLang() {
        this.translate.use('es');
    }

    useCroatianLang() {
        this.translate.use('hr');
    }

    useItalianLang() {
        this.translate.use('it');
    }

    useSerbianLatinLang() {
        this.translate.use('sr');
    }

    isAllowed() {
        return this.permissionsService.canPerform(c => c.email.bulkSend);
    }

    getDashboardInfo(): IAugurHttpRequest {
        this.request = this.dashboardController.getDashboardInfo();
        return this.request;
    }

    //nextQuestion() {
    //    this.questionIdx++;
    //    if (this.questionIdx >= this.questions.length) {
    //        this.questionIdx = this.questions.length - 1;
    //    }
    //}

    //prevQuestion() {
    //    this.questionIdx--;
    //    if (this.questionIdx <= -1) {
    //        this.questionIdx = 0;
    //    }
    //}

    async startPlacementTest(languageId: number) {
        this.isPlacementTestLoading = true;

        const result = await this.placementTestsController.start(
            languageId,
            null,
            null,
            this.playerRating,
            this.playerRd,
            this.playerVol,
            true,
            true);

        const queryParams = {};

        if (this.isDebugModeEnabled) {
            queryParams["debug"] = true;
        }

        this.router.navigate(
            ['/placement-test', result.placementTest.id],
            {
                queryParams: queryParams
            });

        this.isPlacementTestLoading = false;
    }
}
