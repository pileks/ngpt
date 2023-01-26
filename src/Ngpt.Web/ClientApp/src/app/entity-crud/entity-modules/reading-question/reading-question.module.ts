import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AppRoutingModule } from '@src/app/routing/app-routing.module';
import { NgxAugurModule } from '@augur';
import { RequireUserWithVerifiedEmailLoggedInRouteGuard } from '@platform';
import { AppSharedModule } from '@shared';
import { ReadingQuestionUpdateComponent } from '@src/app/entity-crud/entity-modules/reading-question/reading-question-update/reading-question-update.component';
import { ReadingQuestionListComponent } from '@src/app/entity-crud/entity-modules/reading-question/reading-question-list/reading-question-list.component';
import { PlayersModule } from '@src/app/players/players.module';
import { PreviewModule } from '@src/app/entity-crud/preview/preview.module';
import { PlatformModule } from '@platform/platform.module';

@NgModule({
    declarations: [
        ReadingQuestionUpdateComponent, ReadingQuestionListComponent
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
                path: 'reading-question/create',
                component: ReadingQuestionUpdateComponent,
                canActivate: [RequireUserWithVerifiedEmailLoggedInRouteGuard]
            },
            {
                path: 'reading-question/update/:id',
                component: ReadingQuestionUpdateComponent,
                canActivate: [RequireUserWithVerifiedEmailLoggedInRouteGuard]
            },
            {
                 path: 'reading-question/list', 
                 component: ReadingQuestionListComponent, 
                 canActivate: [RequireUserWithVerifiedEmailLoggedInRouteGuard]
            }
        ])

    ]
})
export class ReadingQuestionModule { }
