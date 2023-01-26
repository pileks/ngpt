import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { HttpEventType, HttpClient } from '@angular/common/http';
import { DomSanitizer } from '@angular/platform-browser';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PrintPreviewComponent } from '@shared/print-preview/print-preview.component';
import { AugurLocalStorageManager } from '@augur';
import { UploadedResourcesController } from '@platform';

@Component({
    selector: 'file-uploader',
    templateUrl: './file-uploader.component.html',
    styleUrls: ['./file-uploader.component.css']
})
export class FileUploaderComponent implements OnInit {
    constructor(
        private http: HttpClient,
        private modalService: NgbModal,
        private uploadedResourcesController: UploadedResourcesController,
        private localStorageManager: AugurLocalStorageManager
    ) {}

    progress: number;
    isLoading: boolean = false;
    isUploading: boolean = false;
    fileName: string;
    file: any;
    mimeType: string;

    @Input() disabled: boolean;
    @Input() btnClasses: string;
    @Output() onUploadFinished = new EventEmitter();

    resourceIdValue: any;
    @Output() resourceIdChange = new EventEmitter();

    @Input()
    get resourceId() {
        return this.resourceIdValue;
    }
    set resourceId(val) {
        var oldVal = this.resourceIdValue;
        
        this.resourceIdValue = val;
        this.resourceIdChange.emit(this.resourceId);
    
        if(val !== oldVal) {
            this.loadResource(this.resourceId);    
        }
    }

    async ngOnInit() {
    }

    async loadResource(resourceId) {
        if (resourceId) {
            this.isLoading = true;

            const response = await this.uploadedResourcesController.get(resourceId);

            this.fileName = response.name;
            this.mimeType = response.mimeType;
            this.isLoading = false;
        }
    }

    uploadFile = files => {
        if (files.length === 0) {
            return;
        }

        let fileToUpload = <File>files[0];
        const formData = new FormData();
        formData.append('file', fileToUpload, fileToUpload.name);

        this.isUploading = true;
        this.progress = 0;
        this.resourceId = null;

        this.http.post('/api/files/upload', formData, { reportProgress: true, observe: 'events' }).subscribe((event: any) => {
            if (event.type === HttpEventType.UploadProgress) this.progress = Math.round((100 * event.loaded) / event.total);
            else if (event.type === HttpEventType.Response) {
                this.onUploadFinished.emit(event.body);

                this.resourceId = event.body.resourceId;
                this.fileName = fileToUpload.name;
                this.isUploading = false;
            }
        });
    };

    removeSelection($event) {
        this.resourceId = null;
        this.fileName = null;
        $event.stopPropagation();
    }

    async download() {
        function getFileNameFromContentDispositionHeader(response) {
            var contentDisposition = response.headers.get('content-disposition');
            var result = contentDisposition.split(';')[1].trim().split('=')[1];
            return result.replace(/"/g, '');
        }

        this.isLoading = true;

        const resp = await this.uploadedResourcesController.download(this.resourceId);

        const fileName = getFileNameFromContentDispositionHeader(resp);

        const url = URL.createObjectURL(resp.body);

        const downloadLink = document.createElement("a");

        downloadLink.href = url;
        downloadLink.download = fileName;
        downloadLink.click();

        this.isLoading = false;
    }

    previewFile() {
        const modalRef = this.modalService.open(PrintPreviewComponent, { size: 'lg' });
        modalRef.componentInstance.id = this.resourceId;
        modalRef.componentInstance.url = 'api/uploadedResources/downloadForPreview';
        modalRef.componentInstance.mimeType = this.mimeType;
        modalRef.componentInstance.close = () => modalRef.close();
        modalRef.result.then(r => {
            window.history.back();
            window.history.back();
        }, r => {
            window.history.back();
            if(!r) {
                window.history.back();
            }
        });

        var hideModal = function(event) {
            if (event.state == 'backPressed') {
                modalRef.dismiss(true);
            }
        };

        //https://stackoverflow.com/questions/49954394/when-modal-popup-is-open-prevent-mobile-back-button-to-quit-the-site-close-t
        window.history.pushState('backPressed', null, null);
        window.history.pushState('dummy', null, null);
        window.addEventListener('popstate', hideModal, { once: true });
    }

    private getToken(): string {
        return this.localStorageManager.load<string>('token');
    }
}
