import { Component, Input } from '@angular/core';

@Component({
    selector: 'app-folder-creation-progress-indicator',
    templateUrl: './folder-creation-progress-indicator.component.html',
    styleUrls: ['./folder-creation-progress-indicator.component.css']
})
export class FolderCreationProgressIndicatorComponent {
    constructor() {}

    @Input() folderCreationProgress: {
        isLoading: boolean;
        message: string;
        finishedLoading: boolean;
    };
}
