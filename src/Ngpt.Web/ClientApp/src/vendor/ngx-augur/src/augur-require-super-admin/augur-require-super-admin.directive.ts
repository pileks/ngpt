import {
    OnInit,
    TemplateRef,
    ViewContainerRef,
    Directive,
    Input,
    Inject
} from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

import { IAugurLoggedInUserInfoProvider, I_AUGUR_LOGGED_IN_USER_INFO_PROVIDER } from '../i-augur-logged-in-user-info-provider';

@Directive({
    selector: '[augurRequireSuperAdmin]'
})
export class AugurRequireSuperAdminDirective implements OnInit {
    constructor(
        @Inject(I_AUGUR_LOGGED_IN_USER_INFO_PROVIDER) private loggedInUserProvider: IAugurLoggedInUserInfoProvider,
        private templateRef: TemplateRef<any>,
        private viewContainer: ViewContainerRef
    ) { }

    @Input() augurRequireSuperAdmin: boolean;
    private unsubscribe$ = new Subject<void>();

    ngOnInit() {
      this.loggedInUserProvider.$user
          .pipe(takeUntil(this.unsubscribe$))
          .subscribe(() => {
              this.displayIfSuperAdmin();
          })

      if (this.loggedInUserProvider.user) {
          this.displayIfSuperAdmin();
      }
    }

    ngOnDestroy() {
        this.unsubscribe$.next();
        this.unsubscribe$.complete();
    }

    displayIfSuperAdmin() {
        this.viewContainer.clear();

        if (this.loggedInUserProvider.user.isSuperAdmin || !this.augurRequireSuperAdmin) {
            this.viewContainer.createEmbeddedView(this.templateRef);
        }
    }
}
