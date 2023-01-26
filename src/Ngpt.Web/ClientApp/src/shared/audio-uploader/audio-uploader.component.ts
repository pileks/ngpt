import { Component, Input } from '@angular/core';
import { HttpClient, HttpEventType } from '@angular/common/http';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { UploadedResourcesController } from '@platform';

@Component({
    selector: 'app-audio-uploader',
    templateUrl: './audio-uploader.component.html',
    styleUrls: ['./audio-uploader.component.css'],
    providers: [{
        provide: NG_VALUE_ACCESSOR,
        useExisting: AudioUploaderComponent,
        multi: true
    }]
})
export class AudioUploaderComponent implements ControlValueAccessor {

    constructor(
        private http: HttpClient,
        private uploadedResourcesController: UploadedResourcesController
    ) { }

    resourceId: number;

    progress: number = 0;
    isUploading: boolean = false;
    isClearing: boolean = false;
    @Input() uploaderDisabled = false;

    uploadFile(files: File[]) {
        if (this.isUploading || this.isClearing || this.uploaderDisabled) {
            return;
        }

        if (!files.length) {
            return;
        }

        let file = files[0];

        if (file.name.toLowerCase().endsWith('.mp3') ||
            file.name.toLowerCase().endsWith('.ogg') ||
            file.name.toLowerCase().endsWith('.wav')) {

            if (this.resourceId) {
                this.clear().then(() => {
                    this.performUpload(file);
                });
            } else {
                this.performUpload(file);
            }
        }
    }

    private performUpload(file: File) {
        this.isUploading = true;

        const formData = new FormData();
        formData.append('file', file, file.name);

        this.http.post(
            '/api/files/uploadAudio',
            formData,
            {
                reportProgress: true,
                observe: 'events'
            }).subscribe((event: any) => {
                if (event.type === HttpEventType.UploadProgress) {
                    this.progress = event.loaded / event.total * 100;
                }
                else if (event.type === HttpEventType.Response) {
                    let resourceId = event.body.resourceId;

                    this.resourceId = resourceId;

                    this.isUploading = false;

                    this.onChange(resourceId);
                }
            }
            );
    }

    clear() {
        this.isClearing = true;

        return this.uploadedResourcesController.delete(this.resourceId).then(() => {
            this.resourceId = null;
            this.isClearing = false;
            this.onChange(this.resourceId);
        });
    }

    /* NgModel stuff */

    onChange = (value) => { };

    onTouched = () => { };

    writeValue(value): void {
        this.resourceId = value;
    }

    registerOnChange(fn): void {
        this.onChange = fn;
    }

    registerOnTouched(fn): void {
        this.onTouched = fn;
    }
}
