import { Overlay, OverlayConfig } from '@angular/cdk/overlay';
import { ComponentPortal } from '@angular/cdk/portal';
import { Injectable, Injector, TemplateRef } from '@angular/core';
import { HasOverlayRef } from '@src/vendor/ngx-augur/src/augur-overlay/i-has-overlay-ref';
import { AugurOverlayRef } from './augur-overlay-ref';
import { OverlayComponent } from './overlay/overlay.component';

@Injectable({
    providedIn: 'root'
})
export class OverlayService {
    constructor(private overlay: Overlay, private injector: Injector) {
    }

    open<TInput = any, TOutput = any>(
        content: string | TemplateRef<any> | HasOverlayRef<TInput, TOutput>,
        data: TInput = null,
        sizeClass: string = 'max-w-7xl',
        closeOnBackdropClick: boolean = false
    ): AugurOverlayRef<TInput, TOutput> {
        const configs = new OverlayConfig({
            hasBackdrop: true,
            positionStrategy: this.overlay.position().global().centerHorizontally().centerVertically(),
            panelClass: [sizeClass, 'flex-grow', 'mx-8']
        });

        const overlayRef = this.overlay.create(configs);

        const myOverlayRef = new AugurOverlayRef<TInput, TOutput>(overlayRef, content, data, closeOnBackdropClick);

        const injector = this.createInjector(myOverlayRef, this.injector);
        overlayRef.attach(new ComponentPortal(OverlayComponent, null, injector));

        return myOverlayRef;
    }

    createInjector(ref: AugurOverlayRef, inj: Injector): Injector {
        return Injector.create({
            parent: inj,
            providers: [
                {
                    provide: AugurOverlayRef,
                    useValue: ref
                }
            ],
        });
    }
}