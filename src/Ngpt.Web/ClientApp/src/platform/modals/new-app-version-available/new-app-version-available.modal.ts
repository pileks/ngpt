import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'app-new-app-version-available',
    templateUrl: './new-app-version-available.modal.html',
    styleUrls: ['./new-app-version-available.modal.css']
})
export class NewAppVersionAvailableModal {
    reload() {
        window.location.reload();
    }
}
