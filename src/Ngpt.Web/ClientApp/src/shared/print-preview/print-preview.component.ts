import { Component, OnInit, Input } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { HttpClient } from '@angular/common/http';
import { AugurLocalStorageManager } from '@augur';
import { UploadedResourcesController } from '@platform';

@Component({
    selector: 'print-preview',
    templateUrl: './print-preview.component.html',
    styleUrls: ['./print-preview.component.css']
})
export class PrintPreviewComponent implements OnInit {
    constructor(private sanitizer: DomSanitizer, private http: HttpClient, private localStorageManager: AugurLocalStorageManager, 
        private uploadedResourcesController: UploadedResourcesController) {}

    @Input() id: number;
    @Input() url: string;
    @Input() postUrl: string;
    @Input() postData: string;
    @Input() mimeType: string;
    @Input() close: () => void;
    isLoading: boolean = true;

    async ngOnInit() {
        this.isLoading = true;
        if (this.postUrl) {
            var file = await this.http.post(this.postUrl, this.postData).toPromise();

            this.url = URL.createObjectURL(file);

            this.isLoading = false;
        } else {
            var resp = await this.uploadedResourcesController.download(this.id);

            this.url = URL.createObjectURL(resp.body);

            this.isLoading = false;
        }
    }

    getUrlForPreview() {
        return this.sanitizer.bypassSecurityTrustResourceUrl(this.url);
    }

    isImage() {
        return this.mimeType.startsWith('image');
    }

    onCloseClicked() {
        this.close();
    }

    private getToken(): string {
        return this.localStorageManager.load<string>('token');
    }
}
