import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AppRoutingModule } from '@src/app/routing/app-routing.module';
import { NgxAugurModule } from '@augur';
import { RequireUserWithVerifiedEmailLoggedInRouteGuard } from '@platform';
import { AppSharedModule } from '@shared';
import { ListeningQuestionAudioUpdateComponent } from '@src/app/entity-crud/entity-modules/listening-question-audio/listening-question-audio-update/listening-question-audio-update.component';
import { ListeningQuestionAudioListComponent } from '@src/app/entity-crud/entity-modules/listening-question-audio/listening-question-audio-list/listening-question-audio-list.component';
import { PlatformModule } from '@platform/platform.module';

@NgModule({
    declarations: [
        ListeningQuestionAudioUpdateComponent, ListeningQuestionAudioListComponent
    ],
    imports: [
        NgxAugurModule,
        AppSharedModule,
        AppRoutingModule,
        PlatformModule,

        RouterModule.forRoot([
            {
                path: 'listening-question-audio/create',
                component: ListeningQuestionAudioUpdateComponent,
                canActivate: [RequireUserWithVerifiedEmailLoggedInRouteGuard]
            },
            {
                path: 'listening-question-audio/update/:id',
                component: ListeningQuestionAudioUpdateComponent,
                canActivate: [RequireUserWithVerifiedEmailLoggedInRouteGuard]
            },
            {
                 path: 'listening-question-audio/list', 
                 component: ListeningQuestionAudioListComponent, 
                 canActivate: [RequireUserWithVerifiedEmailLoggedInRouteGuard]
            }
        ])

    ]
})
export class ListeningQuestionAudioModule { }
