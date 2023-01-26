import { NgModule } from '@angular/core';
import { InstructionListComponent } from './instruction-list/instruction-list.component';
import { InstructionUpdateComponent } from './instruction-update/instruction-update.component';
import { NgxAugurModule } from '@augur';
import { AppSharedModule } from '@shared';
import { AppRoutingModule } from '@src/app/routing/app-routing.module';
import { RouterModule } from '@angular/router';
import { RequireSuperAdminLoggedInRouteGuard } from '@platform/routing/guards/require-super-admin-logged-in-route-guard';
import { PlatformModule } from '@platform/platform.module';



@NgModule({
    declarations: [
        InstructionListComponent,
        InstructionUpdateComponent
    ],
    imports: [
        NgxAugurModule,
        AppSharedModule,
        AppRoutingModule,
        PlatformModule,

        RouterModule.forRoot([
            { path: 'instruction/create', component: InstructionUpdateComponent, canActivate: [RequireSuperAdminLoggedInRouteGuard] },
            { path: 'instruction/update/:id', component: InstructionUpdateComponent, canActivate: [RequireSuperAdminLoggedInRouteGuard] },
            { path: 'instruction/list', component: InstructionListComponent, canActivate: [RequireSuperAdminLoggedInRouteGuard] }
        ])
    ]
})
export class InstructionModule { }
