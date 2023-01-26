import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AugurTableComponent } from './augur-table/augur-table.component';
import { AugurEntitySelectComponent } from './augur-entity-select/augur-entity-select.component';
import { AugurGridComponent } from './augur-grid/augur-grid.component';
import { AugurSelectComponent } from './augur-select/augur-select.component';
import { FontAwesomeModule, FaIconLibrary } from '@fortawesome/angular-fontawesome';
import { fas } from '@fortawesome/free-solid-svg-icons';
import { AugurCurrencyInputComponent } from './augur-currency-input/augur-currency-input.component';
import { AugurTextInputComponent } from './augur-text-input/augur-text-input.component';
import { AugurDatePickerComponent } from './augur-date-picker/augur-date-picker.component';
import { AugurNumberInputComponent } from './augur-number-input/augur-number-input.component';
import { AugurContinuousListComponent } from './augur-continuous-list/augur-continuous-list.component';
import { AugurCollapsibleCardComponent } from './augur-collapsible-card/augur-collapsible-card.component';
import { AugurClickOutsideDirective } from './augur-click-outside/augur-click-outside.directive';
import { AugurLoadingIndicatorComponent } from './augur-loading-indicator/augur-loading-indicator.component';
import { AugurEmptyLoggedInUserInfoProvider } from './augur-empty-logged-in-user-info-provider';
import { AugurRequireSuperAdminDirective } from './augur-require-super-admin/augur-require-super-admin.directive';
import { AugurIfSuperAdminDirective } from './augur-if-super-admin/augur-if-super-admin.directive';
import { DATE_DISPLAY_FORMAT, DateDisplayFormat } from './date-display-format';
import { NgbDateCustomParserFormatter } from './ngb-date-custom-parser-formatter';
import { AugurLocalStorageManager } from './augur-local-storage';
import { AugurPageWithNavigationMenuComponent } from './augur-page-with-navigation-menu/augur-page-with-navigation-menu.component';
import { AugurNavigationMenuComponent } from './augur-navigation-menu/augur-navigation-menu.component';
import { AugurContentViewComponent } from './augur-content-view/augur-content-view.component';
import { I_AUGUR_NAVIGATION_MENU_DEFINITION } from './i-augur-navigation-menu-definition';
import { AugurEmptyNavigationMenuDefinition } from './augur-empty-navigation-menu-definition';
import { I_AUGUR_LOGGED_IN_USER_INFO_PROVIDER } from './i-augur-logged-in-user-info-provider';
import { AugurMinutesRemainingPipe } from './augur-minutes-remaining-pipe/augur-minutes-remaining-pipe';
import { AugurTableListComponent } from './augur-table-list/augur-table-list.component';
import { AugurAutofocusDirective } from '@src/vendor/ngx-augur/src/augur-autofocus/augur-autofocus';
import { OverlayComponent } from '@src/vendor/ngx-augur/src/augur-overlay/overlay/overlay.component';
import { NgbDateParserFormatter, NgbModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
    declarations: [
        AugurTableComponent,
        AugurEntitySelectComponent,
        AugurSelectComponent,
        AugurGridComponent,
        AugurCurrencyInputComponent,
        AugurDatePickerComponent,
        AugurTextInputComponent,
        AugurNumberInputComponent,
        AugurContinuousListComponent,
        AugurCollapsibleCardComponent,
        AugurClickOutsideDirective,
        AugurLoadingIndicatorComponent,
        AugurIfSuperAdminDirective,
        AugurRequireSuperAdminDirective,
        AugurPageWithNavigationMenuComponent,
        AugurContentViewComponent,
        AugurNavigationMenuComponent,
        AugurMinutesRemainingPipe,
        AugurTableListComponent,
        AugurAutofocusDirective,
        OverlayComponent
    ],
    imports: [CommonModule, FormsModule, RouterModule, NgbModule, FontAwesomeModule],
    exports: [
        CommonModule,
        FormsModule,

        NgbModule,
        FontAwesomeModule,

        AugurNavigationMenuComponent,

        AugurTableComponent,
        AugurGridComponent,
        AugurEntitySelectComponent,
        AugurSelectComponent,

        AugurCurrencyInputComponent,
        AugurDatePickerComponent,
        AugurTextInputComponent,
        AugurNumberInputComponent,

        AugurContinuousListComponent,
        AugurCollapsibleCardComponent,
        AugurClickOutsideDirective,
        AugurLoadingIndicatorComponent,

        AugurIfSuperAdminDirective,
        AugurRequireSuperAdminDirective,

        AugurPageWithNavigationMenuComponent,
        AugurContentViewComponent,

        AugurMinutesRemainingPipe,

        AugurTableListComponent,
        
        AugurAutofocusDirective,

        OverlayComponent
    ],
    providers: [
        {
            provide: I_AUGUR_LOGGED_IN_USER_INFO_PROVIDER,
            useClass: AugurEmptyLoggedInUserInfoProvider
        },
        {
            provide: DATE_DISPLAY_FORMAT,
            useValue: DateDisplayFormat
        },
        {
            provide: NgbDateParserFormatter,
            useClass: NgbDateCustomParserFormatter
        },
        {
            provide: AugurLocalStorageManager,
            useClass: AugurLocalStorageManager
        },
        {
            provide: I_AUGUR_NAVIGATION_MENU_DEFINITION,
            useClass: AugurEmptyNavigationMenuDefinition
        }
    ]
})
export class NgxAugurModule {
    constructor(library: FaIconLibrary) {
        library.addIconPacks(fas);
    }
}
