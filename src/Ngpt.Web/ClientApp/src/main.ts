import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

//Fix: Changed
//import { AppModule } from '@src/app/app.module';
//to
//import { AppModule } from './app/app.module'
//so ng add @angular/pwa would work
//src: https://github.com/angular/angular-cli/issues/11499

import { AppModule } from './app/app.module'
import { environment } from '@src/environments/environment';

export function getBaseUrl() {
  return document.getElementsByTagName('base')[0].href;
}

const providers = [
  { provide: 'BASE_URL', useFactory: getBaseUrl, deps: [] }
];

if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic(providers).bootstrapModule(AppModule)
  .catch(err => console.log(err));
