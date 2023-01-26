import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { AppRoutingModule } from '@src/app/routing/app-routing.module';
import { UserUpdateComponent } from '@src/app/entity-crud/entity-modules/user/user-update/user-update.component';
import { UserListComponent } from '@src/app/entity-crud/entity-modules/user/user-list/user-list.component';
import { NgxAugurModule } from '@augur';
import { RequireSuperAdminLoggedInRouteGuard } from '@platform';
import { AppSharedModule } from '@shared';

@NgModule({
  declarations: [
    UserUpdateComponent, UserListComponent
  ],
  imports: [
    NgxAugurModule,
    AppSharedModule,
    AppRoutingModule,

    RouterModule.forRoot([
      { path: 'user/create', component: UserUpdateComponent, canActivate: [RequireSuperAdminLoggedInRouteGuard] },
      { path: 'user/update/:id', component: UserUpdateComponent, canActivate: [RequireSuperAdminLoggedInRouteGuard] },
      { path: 'user/list', component: UserListComponent, canActivate: [RequireSuperAdminLoggedInRouteGuard] }
    ])

  ]
})
export class UserModule { }
