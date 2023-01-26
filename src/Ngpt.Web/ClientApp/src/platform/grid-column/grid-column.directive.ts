import { Directive, Input, TemplateRef } from '@angular/core';

@Directive({
    selector: '[gridColumn]',
})
export class GridColumnDirective {

    constructor(public readonly template: TemplateRef<any>) {
    }

    @Input() property: string;
}
