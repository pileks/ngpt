import { Directive, EventEmitter, OnInit, Output } from '@angular/core';

@Directive({
    selector: '[isLoaded]'
})
export class IsLoadedDirective implements OnInit {
    constructor() {}

    @Output() isLoaded = new EventEmitter<void>();

    ngOnInit(): void {
        this.isLoaded.emit();
    }
}
