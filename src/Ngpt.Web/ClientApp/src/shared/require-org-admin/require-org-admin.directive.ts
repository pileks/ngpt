import {
    OnInit,
    TemplateRef,
    ViewContainerRef,
    Directive,
    Input,
    Inject
} from '@angular/core';
import { LoggedInUserInfoProvider } from '@platform';

@Directive({
    selector: '[requireOrgAdmin]'
})
export class RequireOrgAdminDirective implements OnInit {
    constructor(
        private loggedInUserProvider: LoggedInUserInfoProvider,
        private templateRef: TemplateRef<any>,
        private viewContainer: ViewContainerRef
    ) { }

    @Input() requireOrgAdmin: boolean;

    ngOnInit() {
        if (this.loggedInUserProvider.user.isOrgAdmin || !this.requireOrgAdmin) {
            this.viewContainer.createEmbeddedView(this.templateRef);
        } else {
            this.viewContainer.clear();
        }
    }
}
