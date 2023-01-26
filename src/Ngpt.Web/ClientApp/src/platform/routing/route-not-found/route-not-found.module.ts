import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [
  ],
  imports: [
    RouterModule.forRoot([
        {
            path: '**',
            redirectTo: 'home',
            pathMatch: 'full'
        }
    ])
  ]
})
export class RouteNotFoundModule { }
