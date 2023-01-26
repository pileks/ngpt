import {Directive, ElementRef, Output, EventEmitter, HostListener} from '@angular/core';
 
@Directive({
    selector: '[augurClickOutside]'
})
export class AugurClickOutsideDirective {
    constructor(private _elementRef : ElementRef) {
    }
 
    @Output()
    public augurClickOutside = new EventEmitter();
 
    @HostListener('document:click', ['$event.target'])
    public onClick(targetElement) {
        const clickedInside = this._elementRef.nativeElement.contains(targetElement);
        if (!clickedInside) {
            this.augurClickOutside.emit(null);
        }
    }
}