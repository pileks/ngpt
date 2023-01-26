import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { AppRoutingModule } from '@src/app/routing/app-routing.module';
import { NgxAugurModule } from '@augur';
import { AppSharedModule } from '@shared';
import { ReadingQuestionTextUpdateComponent } from '@src/app/entity-crud/entity-modules/reading-question-text/reading-question-text-update/reading-question-text-update.component';
import { ReadingQuestionTextListComponent } from '@src/app/entity-crud/entity-modules/reading-question-text/reading-question-text-list/reading-question-text-list.component';
import { RequireUserWithVerifiedEmailLoggedInRouteGuard } from '@platform';
import { PlatformModule } from '@platform/platform.module';

@NgModule({
    declarations: [
        ReadingQuestionTextUpdateComponent, ReadingQuestionTextListComponent
    ],
    imports: [
        NgxAugurModule,
        AppSharedModule,
        AppRoutingModule,
        PlatformModule,
        RouterModule
    ]
})
export class ReadingQuestionTextModule { }
