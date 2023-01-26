import { OrganizationUserUpdateComponent } from './pages/organization-user-update/organization-user-update.component';
import { OrganizationUserListComponent } from './pages/organization-user-list/organization-user-list.component';
import { NgModule } from '@angular/core';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { BrowserModule } from '@angular/platform-browser';
import { QuillModule } from 'ngx-quill';
import { ServiceWorkerModule } from '@angular/service-worker';
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgxAugurModule, I_AUGUR_LOGGED_IN_USER_INFO_PROVIDER } from '@augur';
import {
    NewAppVersionAvailableModal,
    RouteNotFoundModule,
    AddAuthorizationHeaderInterceptor,
    AppHttpResponseInterceptor,
    LoggedInUserInfoProvider
} from '@platform';
import { AppSharedModule } from '@shared';
import { AppComponent } from '@src/app/app.component';
import { EntityCrudModule } from '@src/app/entity-crud/entity-crud.module';
import { AppRoutingModule } from '@src/app/routing/app-routing.module';
import { environment } from '@src/environments/environment';
import { PendingChangesGuard } from '@platform/routing/guards/pending-changes-guard';
import { AboutComponent } from '@src/app/pages/about/about.component';
import { ChangePasswordComponent } from '@src/app/pages/change-password/change-password.component';
import { EmailConfirmationComponent } from '@src/app/pages/email-confirmation/email-confirmation.component';
import { GlobalSettingsComponent } from '@src/app/pages/global-settings/global-settings.component';
import { HomeComponent } from '@src/app/pages/home/home.component';
import { LoginComponent } from '@src/app/pages/login/login.component';
import { MyProfileComponent } from '@src/app/pages/my-profile/my-profile.component';
import { PrivacyPolicyEnComponent } from '@src/app/pages/privacy-policy/en/privacy-policy-en.component';
import { RegistrationComponent } from '@src/app/pages/registration/registration.component';
import { ResetPasswordComponent } from '@src/app/pages/reset-password/reset-password.component';
import { SetNewPasswordComponent } from '@src/app/pages/set-new-password/set-new-password.component';
import { FormTestComponent } from './pages/form-test/form-test.component';
import { EditorsModule } from '@src/app/editors/editors.module';
import { PlayersModule } from '@src/app/players/players.module';
import { OverlayModule } from '@angular/cdk/overlay';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { TakePlacementTestComponent } from './pages/take-placement-test/take-placement-test.component';
import { PlacementTestResultsComponent } from './pages/placement-test-results/placement-test-results.component';
import { StartPlacementTestComponent } from './pages/start-placement-test/start-placement-test.component';
import { AcceptPlacementTestInvitationComponent } from './pages/accept-placement-test-invitation/accept-placement-test-invitation.component';
import { SortablejsModule } from 'ngx-sortablejs';

// AoT requires an exported function for factories
function HttpLoaderFactory(http: HttpClient) {
    return new TranslateHttpLoader(http);
}
@NgModule({
    declarations: [
        AppComponent,
        HomeComponent,
        LoginComponent,
        RegistrationComponent,
        ChangePasswordComponent,
        GlobalSettingsComponent,
        MyProfileComponent,
        ResetPasswordComponent,
        SetNewPasswordComponent,
        EmailConfirmationComponent,
        NewAppVersionAvailableModal,
        AboutComponent,
        PrivacyPolicyEnComponent,
        FormTestComponent,
        OrganizationUserListComponent,
        OrganizationUserUpdateComponent,
        TakePlacementTestComponent,
        PlacementTestResultsComponent,
        StartPlacementTestComponent,
        AcceptPlacementTestInvitationComponent,
    ],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        HttpClientModule,
        NgxAugurModule,
        AppRoutingModule,
        AppSharedModule,
        EditorsModule,
        EntityCrudModule,
        RouteNotFoundModule,
        OverlayModule,
        ServiceWorkerModule.register('ngsw-worker.js', {
            enabled: environment.production
        }),
        QuillModule.forRoot(),
        ToastrModule.forRoot(),
        DragDropModule,
        PlayersModule,
        TranslateModule.forRoot({
            defaultLanguage: 'en',
            loader: {
                provide: TranslateLoader,
                useFactory: HttpLoaderFactory,
                deps: [HttpClient]
            }
        }),
        SortablejsModule.forRoot({})
    ],
    entryComponents: [NewAppVersionAvailableModal],
    providers: [
        PendingChangesGuard,
        {
            provide: HTTP_INTERCEPTORS,
            useClass: AddAuthorizationHeaderInterceptor,
            multi: true
        },
        {
            provide: HTTP_INTERCEPTORS,
            useClass: AppHttpResponseInterceptor,
            multi: true
        },
        {
            provide: I_AUGUR_LOGGED_IN_USER_INFO_PROVIDER,
            useExisting: LoggedInUserInfoProvider
        }
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
