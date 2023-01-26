import { OrganizationUserListComponent } from './../pages/organization-user-list/organization-user-list.component';
import { OrganizationUserUpdateComponent } from './../pages/organization-user-update/organization-user-update.component';
import { Routes } from '@angular/router';
import {
    RequireUserWithVerifiedEmailLoggedInRouteGuard,
    RequireSuperAdminLoggedInRouteGuard,
    RequireUserLoggedInRouteGuard
} from '@platform';
import { DragDropQuestionEditorComponent } from '@src/app/editors/drag-drop-question-editor/drag-drop-question-editor.component';
import { MultipleChoiceQuestionEditorComponent } from '@src/app/editors/multiple-choice-question-editor/multiple-choice-question-editor.component';
import { ReadingQuestionEditorComponent } from '@src/app/editors/reading-question-editor/reading-question-editor.component';
import { SingleGapQuestionEditorComponent } from '@src/app/editors/single-gap-question-editor/single-gap-question-editor.component';
import { AboutComponent } from '@src/app/pages/about/about.component';
import { ChangePasswordComponent } from '@src/app/pages/change-password/change-password.component';
import { EmailConfirmationComponent } from '@src/app/pages/email-confirmation/email-confirmation.component';
import { FormTestComponent } from '@src/app/pages/form-test/form-test.component';
import { GlobalSettingsComponent } from '@src/app/pages/global-settings/global-settings.component';
import { HomeComponent } from '@src/app/pages/home/home.component';
import { LoginComponent } from '@src/app/pages/login/login.component';
import { MyProfileComponent } from '@src/app/pages/my-profile/my-profile.component';
import { PrivacyPolicyEnComponent } from '@src/app/pages/privacy-policy/en/privacy-policy-en.component';
import { RegistrationComponent } from '@src/app/pages/registration/registration.component';
import { ResetPasswordComponent } from '@src/app/pages/reset-password/reset-password.component';
import { SetNewPasswordComponent } from '@src/app/pages/set-new-password/set-new-password.component';
import { RequireOrgAdminLoggedInRouteGuard } from '@platform/routing/guards/require-org-admin-logged-in-route-guard';
import { ReadingQuestionTextListComponent } from '@src/app/entity-crud/entity-modules/reading-question-text/reading-question-text-list/reading-question-text-list.component';
import { ReadingQuestionTextUpdateComponent } from '@src/app/entity-crud/entity-modules/reading-question-text/reading-question-text-update/reading-question-text-update.component';
import { ReadingQuestionUpdateComponent } from '@src/app/entity-crud/entity-modules/reading-question/reading-question-update/reading-question-update.component';
import { ListeningQuestionUpdateComponent } from '@src/app/entity-crud/entity-modules/listening-question/listening-question-update/listening-question-update.component';
import { ListeningQuestionAudioUpdateComponent } from '@src/app/entity-crud/entity-modules/listening-question-audio/listening-question-audio-update/listening-question-audio-update.component';
import { ListeningQuestionAudioListComponent } from '@src/app/entity-crud/entity-modules/listening-question-audio/listening-question-audio-list/listening-question-audio-list.component';
import { TakePlacementTestComponent } from '@src/app/pages/take-placement-test/take-placement-test.component';
import { PlacementTestResultsComponent } from '@src/app/pages/placement-test-results/placement-test-results.component';
import { StartPlacementTestComponent } from '@src/app/pages/start-placement-test/start-placement-test.component';
import { AcceptPlacementTestInvitationComponent } from '@src/app/pages/accept-placement-test-invitation/accept-placement-test-invitation.component';

export const routes: Routes = [
    {
        path: '',
        redirectTo: 'home',
        pathMatch: 'full'
    },
    {
        path: 'home',
        component: HomeComponent,
        canActivate: [RequireUserWithVerifiedEmailLoggedInRouteGuard]
    },
    {
        path: 'login',
        component: LoginComponent
    },
    {
        path: 'registration',
        component: RegistrationComponent
    },
    {
        path: 'change-password',
        component: ChangePasswordComponent
    },
    {
        path: 'settings',
        component: GlobalSettingsComponent,
        canActivate: [RequireSuperAdminLoggedInRouteGuard]
    },
    {
        path: 'my-profile',
        component: MyProfileComponent,
        canActivate: [RequireUserWithVerifiedEmailLoggedInRouteGuard]
    },
    {
        path: 'about',
        component: AboutComponent,
        canActivate: [RequireUserWithVerifiedEmailLoggedInRouteGuard]
    },
    {
        path: 'reset-password',
        component: ResetPasswordComponent
    },
    {
        path: 'set-new-password/:uid',
        component: SetNewPasswordComponent
    },
    {
        path: 'email-confirmation',
        component: EmailConfirmationComponent,
        canActivate: [RequireUserLoggedInRouteGuard]
    },
    {
        path: 'multiple-choice-question-editor',
        component: MultipleChoiceQuestionEditorComponent,
        canActivate: [RequireUserLoggedInRouteGuard]
    },
    {
        path: 'single-gap-question-editor',
        component: SingleGapQuestionEditorComponent,
        canActivate: [RequireUserLoggedInRouteGuard]
    },
    {
        path: 'drag-drop-question-editor',
        component: DragDropQuestionEditorComponent,
        canActivate: [RequireUserLoggedInRouteGuard]
    },
    {
        path: 'reading-question-editor',
        component: ReadingQuestionEditorComponent,
        canActivate: [RequireUserLoggedInRouteGuard]
    },
    {
        path: 'placement-test/:placementTestId',
        component: TakePlacementTestComponent
    },
    {
        path: 'accept-placement-test-invitation/:token',
        component: AcceptPlacementTestInvitationComponent
    },
    {
        path: 'placement-test/:placementTestId/results',
        component: PlacementTestResultsComponent
    },
    {
        path: 'start-placement-test',
        component: StartPlacementTestComponent
    },
    {
        path: 'reading-question-text',
        children: [
            {
                path: 'list',
                component: ReadingQuestionTextListComponent,
                canActivate: [RequireUserWithVerifiedEmailLoggedInRouteGuard]
            },
            {
                path: 'create',
                component: ReadingQuestionTextUpdateComponent,
                canActivate: [RequireUserWithVerifiedEmailLoggedInRouteGuard]
            },
            {
                path: 'update/:id',
                component: ReadingQuestionTextUpdateComponent,
                canActivate: [RequireUserWithVerifiedEmailLoggedInRouteGuard]
            },
            {
                path: 'update/:id/questions/create',
                component: ReadingQuestionUpdateComponent,
                canActivate: [RequireUserWithVerifiedEmailLoggedInRouteGuard]
            },
            {
                path: 'update/:id/questions/:questionId',
                component: ReadingQuestionUpdateComponent,
                canActivate: [RequireUserWithVerifiedEmailLoggedInRouteGuard]
            },
        ]
    },
    {
        path: 'listening-question-audio',
        children: [
            {
                path: 'list',
                component: ListeningQuestionAudioListComponent,
                canActivate: [RequireUserWithVerifiedEmailLoggedInRouteGuard]
            },
            {
                path: 'create',
                component: ListeningQuestionAudioUpdateComponent,
                canActivate: [RequireUserWithVerifiedEmailLoggedInRouteGuard]
            },
            {
                path: 'update/:id',
                component: ListeningQuestionAudioUpdateComponent,
                canActivate: [RequireUserWithVerifiedEmailLoggedInRouteGuard]
            },
            {
                path: 'update/:id/questions/create',
                component: ListeningQuestionUpdateComponent,
                canActivate: [RequireUserWithVerifiedEmailLoggedInRouteGuard]
            },
            {
                path: 'update/:id/questions/:questionId',
                component: ListeningQuestionUpdateComponent,
                canActivate: [RequireUserWithVerifiedEmailLoggedInRouteGuard]
            },
        ]
    },
    {
        path: 'privacy-policy/en',
        component: PrivacyPolicyEnComponent
    },
    {
        path: 'privacy-policy',
        redirectTo: 'privacy-policy/en'
    },
    {
        path: 'form-test',
        component: FormTestComponent
    },
    {
        path: 'organization-user/list',
        component: OrganizationUserListComponent,
        canActivate: [RequireOrgAdminLoggedInRouteGuard]
    },
    {
        path: 'organization-user/update/:id',
        component: OrganizationUserUpdateComponent,
        canActivate: [RequireOrgAdminLoggedInRouteGuard]
    },
    {
        path: 'organization-user/create',
        component: OrganizationUserUpdateComponent,
        canActivate: [RequireOrgAdminLoggedInRouteGuard]
    }
];
