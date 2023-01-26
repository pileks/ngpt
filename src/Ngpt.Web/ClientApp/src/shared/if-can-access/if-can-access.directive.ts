import {
    OnInit,
    TemplateRef,
    ViewContainerRef,
    Directive,
    Input,
    Inject
} from '@angular/core';
import { PermissionsService } from '@platform';

@Directive({
    selector: '[ifCanAccess]'
})
export class IfCanAccessDirective implements OnInit {
    constructor(
        private permissionsService: PermissionsService,
        private templateRef: TemplateRef<any>,
        private viewContainer: ViewContainerRef
    ) {}

    @Input() ifCanAccess: string;
    
    ngOnInit() {
        var componentName = this.ifCanAccess;

        if (this.permissionsService.canAccessByName(componentName)) {
            this.viewContainer.createEmbeddedView(this.templateRef);
        }
        else {
            this.viewContainer.clear();
        }
    }
}
