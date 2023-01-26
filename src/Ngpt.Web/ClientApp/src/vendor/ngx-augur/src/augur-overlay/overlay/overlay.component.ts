import { Component, OnInit, TemplateRef, Type } from '@angular/core';
import { HasOverlayRef } from '@src/vendor/ngx-augur/src/augur-overlay/i-has-overlay-ref';
import { AugurOverlayRef } from '../augur-overlay-ref';

@Component({
    selector: 'app-overlay',
    templateUrl: './overlay.component.html',
    styleUrls: ['./overlay.component.css']
})
export class OverlayComponent implements OnInit {
    contentType: 'template' | 'string' | 'component';
    content: string | TemplateRef<any> | HasOverlayRef;
    context;

    constructor(private ref: AugurOverlayRef) { }

    close() {
        this.ref.close(null);
    }

    ngOnInit() {
        this.content = this.ref.content;

        if (typeof this.content === 'string') {
            this.contentType = 'string';
        } else if (this.content instanceof TemplateRef) {
            this.contentType = 'template';
            this.context = {
                close: this.ref.close.bind(this.ref)
            };
        } else {
            this.contentType = 'component';
        }
    }
}