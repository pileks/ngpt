import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Language } from '@src/app/entities/language';
import { LanguagesController } from '@src/app/web-api-controllers/languages/languages-controller';
import { PlacementTestInviteModel } from '@src/app/web-api-controllers/placement-tests/models/placement-test-invite-model';
import { PlacementTestsController } from '@src/app/web-api-controllers/placement-tests/placement-tests-controller';

@Component({
    selector: 'app-start-placement-test',
    templateUrl: './start-placement-test.component.html',
    styleUrls: ['./start-placement-test.component.css']
})
export class StartPlacementTestComponent implements OnInit {

    constructor(
        private placementTestsController: PlacementTestsController,
        public languagesController: LanguagesController,
        private route: ActivatedRoute
    ) { }

    async ngOnInit() {
        const languageId = this.route.snapshot.queryParamMap.get("languageId");

        if (languageId != null) {
            this.model.languageId = Number.parseInt(languageId);
            this.hideLanguageField = true;
            this.selectedLanguage = await this.languagesController.get(this.model.languageId);
        }

    }

    selectedLanguage: Language;
    model: PlacementTestInviteModel = new PlacementTestInviteModel();

    isLoading: boolean = false;
    isInvitationSent: boolean = false;
    hideLanguageField: boolean = false;

    async start() {
        this.isLoading = true;

        await this.placementTestsController.invite(this.model);

        this.isInvitationSent = true;
        this.isLoading = false;
    }
}
