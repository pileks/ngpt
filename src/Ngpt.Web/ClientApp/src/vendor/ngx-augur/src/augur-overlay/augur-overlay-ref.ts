import { Subject } from 'rxjs';

import { OverlayRef } from '@angular/cdk/overlay';

import { TemplateRef, Type } from '@angular/core';
import { HasOverlayRef } from '@src/vendor/ngx-augur/src/augur-overlay/i-has-overlay-ref';

export interface OverlayCloseEvent<R> {
    type: 'backdropClick' | 'close';
    data: R;
}

// R = Response Data Type, T = Data passed to Modal Type
export class AugurOverlayRef<TInput = any, TOutput = any> {
    afterClosed$ = new Subject<OverlayCloseEvent<TOutput>>();

    constructor(
        public overlay: OverlayRef,
        public content: string | TemplateRef<any> | HasOverlayRef<TInput, TOutput>,
        public data: TInput, // pass data to modal i.e. FormData
        public closeOnBackdropClick: boolean
    ) {
        overlay.backdropClick().subscribe(() => this._close('backdropClick', null));
    }

    close(data: TOutput = null) {
        this._close('close', data);
    }

    private _close(type: 'backdropClick' | 'close', data: TOutput) {
        if (type === 'backdropClick' && !this.closeOnBackdropClick) {
            return;
        }

        this.overlay.dispose();
        this.afterClosed$.next({
            type,
            data
        });

        this.afterClosed$.complete();
    }
}