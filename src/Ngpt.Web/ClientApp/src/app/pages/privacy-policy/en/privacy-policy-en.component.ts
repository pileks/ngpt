import { Component } from '@angular/core';
import { Location } from '@angular/common';

@Component({
    selector: 'app-privacy-policy-en',
    templateUrl: './privacy-policy-en.component.html',
    styleUrls: ['./privacy-policy-en.component.css']
})
export class PrivacyPolicyEnComponent {
    constructor(private location: Location) {}

    back() {
        this.location.back();
    }
}