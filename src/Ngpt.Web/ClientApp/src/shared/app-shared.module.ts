import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AugurWebHelpers } from '@augur';
import { PageWithNavigationMenuComponent } from '@shared/page-with-navigation-menu/page-with-navigation-menu.component';
import { NavigationMenuComponent } from '@shared/navigation-menu/navigation-menu.component';
import { ContentViewComponent } from '@shared/content-view/content-view.component';
import { I_AUGUR_NAVIGATION_MENU_DEFINITION } from '@augur';
import { NavigationMenuDefinition } from '@shared/navigation-menu-definition';
import { I_AUGUR_LOGGED_IN_USER_INFO_PROVIDER } from '@augur';
import { NgxAugurModule } from '@augur';
//import Quill from 'quill';
import { IfCanAccessDirective } from '@shared/if-can-access/if-can-access.directive';
import { IfCanPerformDirective } from '@shared/if-can-perform/if-can-perform.directive';
import { FileUploaderComponent } from '@shared/file-uploader/file-uploader.component';
import { PrintPreviewComponent } from '@shared/print-preview/print-preview.component';
import { LoggedInUserInfoProvider } from '@platform';
import { IsLoadedDirective } from '@shared/is-loaded/is-loaded.directive';
import { FolderCreationProgressIndicatorComponent } from '@shared/folder-creation-progress-indicator/folder-creation-progress-indicator.component';
import { UploadInterfaceComponent } from './upload-interface/upload-interface.component';
import { RouterModule } from '@angular/router';
import { ChooseQuantityComponent } from '@shared/choose-quantity/choose-quantity.component';
import { TextInputComponent } from './text-input/text-input.component';
import { FormsModule } from '@angular/forms';
import { ToggleComponent } from './toggle/toggle.component';
import { PanelComponent } from './panel/panel.component';
import { FileDragDropDirective } from '@shared/file-drag-drop/file-drag-drop.component';
import { ImageUploaderComponent } from './image-uploader/image-uploader.component';
import { AudioUploaderComponent } from './audio-uploader/audio-uploader.component';
import { TextAreaInputComponent } from '@shared/textarea-input/textarea-input.component';
import { SimpleHeadingComponent } from './simple-heading/simple-heading.component';
import { SelectComponent } from './select/select.component';
import { RequireOrgAdminDirective } from '@shared/require-org-admin/require-org-admin.directive';
import { TranslateModule } from '@ngx-translate/core';
import { DatePickerComponent } from '@shared/date-picker/date-picker.component';
import { NumberInputComponent } from '@shared/number-input/number-input.component';
import { AngularMyDatePickerModule } from 'angular-mydatepicker';
import { EntitySelectComponent } from '@shared/entity-select/entity-select.component';

declare var registerQuillCustomVideo: any;

@NgModule({
    declarations: [
        NavigationMenuComponent,
        ContentViewComponent,
        PageWithNavigationMenuComponent,
        IfCanAccessDirective,
        IfCanPerformDirective,
        FileUploaderComponent,
        PrintPreviewComponent,
        IsLoadedDirective,
        FolderCreationProgressIndicatorComponent,
        UploadInterfaceComponent,
        ChooseQuantityComponent,
        TextInputComponent,
        ToggleComponent,
        PanelComponent,
        FileDragDropDirective,
        ImageUploaderComponent,
        AudioUploaderComponent,
        TextAreaInputComponent,
        SimpleHeadingComponent,
        SelectComponent,
        RequireOrgAdminDirective,
        DatePickerComponent,
        NumberInputComponent,
        EntitySelectComponent
    ],
    imports: [
        BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
        NgxAugurModule,
        NgbModule,
        RouterModule,
        FormsModule,
        TranslateModule,
        AngularMyDatePickerModule
    ],
    exports: [
        NavigationMenuComponent,
        ContentViewComponent,
        PageWithNavigationMenuComponent,
        IfCanAccessDirective,
        IfCanPerformDirective,
        FileUploaderComponent,
        PrintPreviewComponent,
        IsLoadedDirective,
        FolderCreationProgressIndicatorComponent,
        UploadInterfaceComponent,
        ChooseQuantityComponent,
        TextInputComponent,
        ToggleComponent,
        PanelComponent,
        FileDragDropDirective,
        ImageUploaderComponent,
        AudioUploaderComponent,
        TextAreaInputComponent,
        SimpleHeadingComponent,
        SelectComponent,
        RequireOrgAdminDirective,
        TranslateModule,
        DatePickerComponent,
        NumberInputComponent,
        EntitySelectComponent
    ],
    entryComponents: [PrintPreviewComponent],
    providers: [
        {
            provide: I_AUGUR_NAVIGATION_MENU_DEFINITION,
            useExisting: NavigationMenuDefinition
        },
        {
            provide: I_AUGUR_LOGGED_IN_USER_INFO_PROVIDER,
            useExisting: LoggedInUserInfoProvider
        }
    ]
})
export class AppSharedModule {
    constructor() {
        AugurWebHelpers.override();
        //registerQuillCustomVideo(Quill);
    }
}
