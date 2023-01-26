import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
    selector: 'app-take-placement-test',
    templateUrl: './take-placement-test.component.html',
    styleUrls: ['./take-placement-test.component.css']
})
export class TakePlacementTestComponent implements OnInit {

    constructor(
        private route: ActivatedRoute,
        private router: Router
    ) { }

    ngOnInit(): void {
        this.placementTestId = Number.parseInt(this.route.snapshot.paramMap.get("placementTestId"));

        const debugParam = this.route.snapshot.queryParamMap.get("debug");
        const tauParam = this.route.snapshot.queryParamMap.get("tau");
        const rdParam = this.route.snapshot.queryParamMap.get("rd");
        const volParam = this.route.snapshot.queryParamMap.get("vol");
        const rdCutoffParam = this.route.snapshot.queryParamMap.get("rdCutoff");

        console.log("debugParam:", debugParam);
        console.log("tauParam:", tauParam);
        console.log("rdParam:", rdParam);
        console.log("volParam:", volParam);
        console.log("rdCutoffParam:", rdCutoffParam);

        if (tauParam) {
            this.tau = Number.parseFloat(tauParam);
        }
        if (rdParam) {
            this.rd = Number.parseFloat(rdParam);
        }
        if (volParam) {
            this.vol = Number.parseFloat(volParam);
        }
        if (rdCutoffParam) {
            this.rdCutoff = Number.parseFloat(rdCutoffParam);
        }
        this.isDebugEnabled = debugParam === "true";
    }

    placementTestId: number;
    isDebugEnabled: boolean = false;
    tau: number = null;
    rd: number = null;
    vol: number = null;
    rdCutoff: number = null;

    async onTestCompleted() {
        this.router.navigate(['results'], { relativeTo: this.route });
    }
}
