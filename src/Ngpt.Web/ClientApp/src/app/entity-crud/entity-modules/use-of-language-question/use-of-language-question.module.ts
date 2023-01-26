import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AppRoutingModule } from '@src/app/routing/app-routing.module';
import { NgxAugurModule } from '@augur';
import { AppSharedModule } from '@shared';
import { UseOfLanguageQuestionListComponent } from '@src/app/entity-crud/entity-modules/use-of-language-question/use-of-language-question-list/use-of-language-question-list.component';
import { UseOfLanguageQuestionUpdateComponent } from '@src/app/entity-crud/entity-modules/use-of-language-question/use-of-language-question-update/use-of-language-question-update.component';
import { EditorsModule } from '@src/app/editors/editors.module';
import { PlayersModule } from '@src/app/players/players.module';
import { PreviewModule } from '@src/app/entity-crud/preview/preview.module';
import { RequireUserWithVerifiedEmailLoggedInRouteGuard } from '@platform';
import { PlatformModule } from '@platform/platform.module';

@NgModule({
    declarations: [
        UseOfLanguageQuestionUpdateComponent, UseOfLanguageQuestionListComponent
    ],
    imports: [
        NgxAugurModule,
        AppSharedModule,
        AppRoutingModule,
        EditorsModule,
        PlayersModule,
        PreviewModule,
        PlatformModule,

        RouterModule.forRoot([
            {
                path: 'use-of-language-question/create',
                component: UseOfLanguageQuestionUpdateComponent,
                canActivate: [RequireUserWithVerifiedEmailLoggedInRouteGuard]
            },
            {
                path: 'use-of-language-question/update/:id',
                component: UseOfLanguageQuestionUpdateComponent,
                canActivate: [RequireUserWithVerifiedEmailLoggedInRouteGuard]
            },
            {
                 path: 'use-of-language-question/list', 
                 component: UseOfLanguageQuestionListComponent, 
                 canActivate: [RequireUserWithVerifiedEmailLoggedInRouteGuard]
            }
        ])

    ]
})
export class UseOfLanguageQuestionModule { }
