import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AnswerPreviewComponent } from '@src/app/entity-crud/preview/answer-preview/answer-preview.component';
import { AppSharedModule } from '@shared';

@NgModule({
    declarations: [
        AnswerPreviewComponent
    ],
    imports: [
        CommonModule,
        AppSharedModule
    ],
    exports: [
        AnswerPreviewComponent
    ]
})
export class PreviewModule { }
