import { HttpClient } from '@angular/common/http';
import { Inject } from '@angular/core';
import { GridRequestModel, GridResult } from './grid-types';
import { AugurEntityController } from './augur-entity-controller';
import { AugurHttpRequest } from './augur-http-request';

export class AugurEntityWithGridController<TEntity, TGridModel> extends AugurEntityController<TEntity> {
    constructor(protected controllerRoute: string, protected http: HttpClient, @Inject('BASE_URL') protected baseUrl: string) {
        super(controllerRoute, http, baseUrl);

        this.grid = this.grid.bind(this);
    }

    grid(gridRequestModel: GridRequestModel): AugurHttpRequest<GridResult<TGridModel>> {

        return this.http.post<any[]>(this.baseUrl + this.controllerRoute + '/grid', gridRequestModel ?? {})
            .toAugurHttpRequest<GridResult<TGridModel>>();
    }
}
