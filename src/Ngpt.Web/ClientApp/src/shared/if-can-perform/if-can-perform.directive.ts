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
    selector: '[ifCanPerform]'
})
export class IfCanPerformDirective implements OnInit {
    constructor(
        private permissionsService: PermissionsService,
        private templateRef: TemplateRef<any>,
        private viewContainer: ViewContainerRef
    ) {}

    @Input() ifCanPerform: string;
    
    ngOnInit() {
        var componentAndActivityNames = this.ifCanPerform.split('.');

        var componentName = componentAndActivityNames[0];
        var activityName = componentAndActivityNames[1];

        if (this.permissionsService.canPerformByName(componentName, activityName)) {
            this.viewContainer.createEmbeddedView(this.templateRef);
        }
        else {
            this.viewContainer.clear();
        }
    }
}
