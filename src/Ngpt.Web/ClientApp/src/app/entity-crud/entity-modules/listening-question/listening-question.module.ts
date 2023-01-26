import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AppRoutingModule } from '@src/app/routing/app-routing.module';
import { NgxAugurModule } from '@augur';
import { RequireUserWithVerifiedEmailLoggedInRouteGuard } from '@platform';
import { AppSharedModule } from '@shared';
import { ListeningQuestionUpdateComponent } from '@src/app/entity-crud/entity-modules/listening-question/listening-question-update/listening-question-update.component';
import { ListeningQuestionListComponent } from '@src/app/entity-crud/entity-modules/listening-question/listening-question-list/listening-question-list.component';
import { PlayersModule } from '@src/app/players/players.module';
import { PreviewModule } from '@src/app/entity-crud/preview/preview.module';
import { PlatformModule } from '@platform/platform.module';

@NgModule({
    declarations: [
        ListeningQuestionUpdateComponent, ListeningQuestionListComponent
    ],
    imports: [
        NgxAugurModule,
        AppSharedModule,
        AppRoutingModule,
        PlayersModule,
        PreviewModule,
        PlatformModule,

        RouterModule.forRoot([
            {
                path: 'listening-question/create',
                component: ListeningQuestionUpdateComponent,
                canActivate: [RequireUserWithVerifiedEmailLoggedInRouteGuard]
            },
            {
                path: 'listening-question/update/:id',
                component: ListeningQuestionUpdateComponent,
                canActivate: [RequireUserWithVerifiedEmailLoggedInRouteGuard]
            },
            {
                 path: 'listening-question/list', 
                 component: ListeningQuestionListComponent, 
                 canActivate: [RequireUserWithVerifiedEmailLoggedInRouteGuard]
            }
        ])

    ]
})
export class ListeningQuestionModule { }
