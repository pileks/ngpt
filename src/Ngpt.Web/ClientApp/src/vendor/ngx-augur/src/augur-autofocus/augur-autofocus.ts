import { AfterContentInit, Directive, ElementRef, Input } from '@angular/core';

@Directive({
    selector: '[augurAutoFocus]'
})
export class AugurAutofocusDirective implements AfterContentInit {

    @Input() public augurAutoFocus: boolean;

    public constructor(private el: ElementRef) {

    }

    public ngAfterContentInit() {

        setTimeout(() => {
            this.el.nativeElement.focus();
        },);
    }
}