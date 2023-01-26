import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
    selector: 'app-upload-interface',
    templateUrl: './upload-interface.component.html',
    styleUrls: ['./upload-interface.component.css']
})
export class UploadInterfaceComponent implements OnInit {
    constructor() {}

    @Input() disabled: boolean;
    @Input() label: string;
    @Output() onFileSubmit = new EventEmitter<File>();

    file: File;
    editing: boolean;

    ngOnInit(): void {}

    addFile() {
        this.editing = true;
    }

    chooseFile(file: File) {
        this.file = file;
    }

    submitFile() {
        this.onFileSubmit.emit(this.file);
        this.closeUploadInterface();
    }

    closeUploadInterface() {
        this.editing = false;
        this.file = undefined;
    }
}
