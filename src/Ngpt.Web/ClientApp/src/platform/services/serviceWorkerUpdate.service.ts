import { Injectable } from '@angular/core';

import { SwUpdate } from '@angular/service-worker';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NewAppVersionAvailableModal } from '@platform/modals/new-app-version-available/new-app-version-available.modal';
import { OverlayService } from '@src/vendor/ngx-augur/src/augur-overlay/overlay.service';

@Injectable({
    providedIn: 'root'
})
export class ServiceWorkerUpdateService {
    
    constructor(private swUpdate: SwUpdate, private modalService: NgbModal, private overlayService: OverlayService) {
    }

    private isUpdateScheduled: boolean;

    monitorForUpdates() {
        this.swUpdate.available.subscribe(async evt => {
            await this.update()
        });
    }

    private async update() {
        console.log("Found new app version.");

        console.log("Showing reload dialog.");

        this.showShouldReloadModal();
    }

    private updateIfScheduled() {

        if(this.isUpdateScheduled) {
            this.isUpdateScheduled = false,

            console.log("Updating app from schedule.");
            
            this.showShouldReloadModal();
        }
        else {
            console.log("Update not scheduled, canceling...");
        }
    }

    private showShouldReloadModal() {
        this.overlayService.open(NewAppVersionAvailableModal, null, 'max-w-lg', false);
    }
}
