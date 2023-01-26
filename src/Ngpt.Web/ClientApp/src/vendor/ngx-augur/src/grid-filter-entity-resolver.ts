import { HttpResponse } from "@angular/common/http";
import { AugurHttpRequest } from "@augur";

export interface IGridFilterEntityResolver {
    resolve: (entityType: string) => GridFilterEntityEndpointDefinition;
}

export class GridFilterEntityEndpointDefinition {
    constructor(init?: Partial<GridFilterEntityEndpointDefinition>) {
        Object.assign(this, init);
    }

    endpoint: (pageNumber, pageSize, searchQuery, columnFilter) => AugurHttpRequest<HttpResponse<any[]>>;
    map: (entity) => any;
}