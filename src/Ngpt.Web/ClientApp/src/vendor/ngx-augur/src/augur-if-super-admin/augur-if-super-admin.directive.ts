import {
    OnInit,
    TemplateRef,
    ViewContainerRef,
    Directive,
    Input,
    Inject
} from '@angular/core';
import { I_AUGUR_LOGGED_IN_USER_INFO_PROVIDER, IAugurLoggedInUserInfoProvider } from '../i-augur-logged-in-user-info-provider';

@Directive({
    selector: '[augurIfSuperAdmin]'
})
export class AugurIfSuperAdminDirective implements OnInit {
    constructor(
        @Inject(I_AUGUR_LOGGED_IN_USER_INFO_PROVIDER) private loggedInUserProvider: IAugurLoggedInUserInfoProvider,
        private templateRef: TemplateRef<any>,
        private viewContainer: ViewContainerRef
    ) {}

    @Input() augurIfSuperAdmin: boolean;
    
    ngOnInit() {
        if (this.loggedInUserProvider.user.isSuperAdmin) {
            this.viewContainer.createEmbeddedView(this.templateRef);
        } else {
            this.viewContainer.clear();
        }
    }
}
