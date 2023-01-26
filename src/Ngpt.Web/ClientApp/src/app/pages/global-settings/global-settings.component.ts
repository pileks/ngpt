import { Component, OnInit, Inject, Input } from '@angular/core';
import { IAugurHttpRequest } from '@augur';
import { GlobalSettingsController } from '@src/app/web-api-controllers/global-settings/global-settings-controller';
import { GlobalSettings } from '@platform';

@Component({
  selector: 'app-global-settings',
  templateUrl: './global-settings.component.html',
  styleUrls: ['./global-settings.component.css']
})
export class GlobalSettingsComponent implements OnInit {

  constructor(private globalSettingsController: GlobalSettingsController) {
  }

  settings: GlobalSettings = null;
  request: IAugurHttpRequest = null;

  get areSettingsLoading(): boolean {
    return this.request && this.request.isLoading;
  }

  async ngOnInit() {
    this.request = this.globalSettingsController.get();

    this.settings = await this.request;
  }

  isValid() {
    return this.settings;
  }

  async update() {
    this.request = this.globalSettingsController.update(this.settings);

    this.settings = await this.request;

    alert('Updated settings');
  }
}
