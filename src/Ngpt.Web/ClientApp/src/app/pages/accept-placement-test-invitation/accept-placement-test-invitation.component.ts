import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PlacementTestInvitation } from '@src/app/entities/placement-test-invitation';
import { LevelsController } from '@src/app/web-api-controllers/levels/levels-controller';
import { PlacementTestsController } from '@src/app/web-api-controllers/placement-tests/placement-tests-controller';

@Component({
    selector: 'app-accept-placement-test-invitation',
    templateUrl: './accept-placement-test-invitation.component.html',
    styleUrls: ['./accept-placement-test-invitation.component.css']
})
export class AcceptPlacementTestInvitationComponent implements OnInit {

    constructor(
        private placementTestsController: PlacementTestsController,
        public levelsController: LevelsController,
        private route: ActivatedRoute,
        private router: Router
    ) { }

    async ngOnInit() {
        this.isLoading = true;

        this.token = this.route.snapshot.paramMap.get('token');

        this.invitation = await this.placementTestsController.getInvitationByToken(this.token);

        if (this.invitation.placementTestId) {
            this.router.navigate(['/placement-test', this.invitation.placementTestId]);
        }

        this.isLoading = false;
    }

    token: string;
    invitation: PlacementTestInvitation;
    levelId: number = null;
    shouldTestReading: boolean = true;
    shouldTestListening: boolean = true;

    isLoading: boolean = false;

    async start() {
        this.isLoading = true;

        const result = await this.placementTestsController.start(
            this.invitation.languageId,
            this.levelId,
            this.invitation.id,
            null,
            null,
            null,
            this.shouldTestReading,
            this.shouldTestListening);

        this.router.navigate(['/placement-test', result.placementTest.id]);
    }
}
