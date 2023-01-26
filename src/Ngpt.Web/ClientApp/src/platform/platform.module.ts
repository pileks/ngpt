import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GridComponent } from './grid/grid.component';
import { GridColumnDirective } from './grid-column/grid-column.directive';
import { AppSharedModule } from '@shared';
import { FormsModule } from '@angular/forms';
import { GridFilterBuilderComponent } from './grid-filter-builder/grid-filter-builder.component';
import { NgxAugurModule } from '@augur';
import { GridPaginationComponent } from './grid-pagination/grid-pagination.component';



@NgModule({
    declarations: [
        GridComponent,
        GridColumnDirective,
        GridFilterBuilderComponent,
        GridPaginationComponent
    ],
    imports: [
        CommonModule,
        AppSharedModule,
        FormsModule,
        NgxAugurModule
    ],
    exports: [
        GridComponent,
        GridColumnDirective
    ]
})
export class PlatformModule { }
