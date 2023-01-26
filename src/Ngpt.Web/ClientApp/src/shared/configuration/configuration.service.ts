import { Injectable } from '@angular/core';
import { ConfigurationController } from '@src/app/web-api-controllers/configuration/configuration-controller';

@Injectable({
    providedIn: 'root'
})
export class ConfigurationService {
    constructor(private configurationController: ConfigurationController) {}

    async loadConfig() {
        return await this.configurationController.getConfig();
    }
}
