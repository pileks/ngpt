import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PlacementTestResultModel } from '@src/app/web-api-controllers/placement-tests/models/placement-test-result-model';
import { PlacementTestsController } from '@src/app/web-api-controllers/placement-tests/placement-tests-controller';

@Component({
    selector: 'app-placement-test-results',
    templateUrl: './placement-test-results.component.html',
    styleUrls: ['./placement-test-results.component.css']
})
export class PlacementTestResultsComponent implements OnInit {

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private placementTestsController: PlacementTestsController
    ) { }

    placementTestId: number;
    results: PlacementTestResultModel;

    async ngOnInit() {
        this.placementTestId = Number.parseInt(this.route.snapshot.paramMap.get("placementTestId"));

        this.results = await this.placementTestsController.getResult(this.placementTestId);
    }

}
