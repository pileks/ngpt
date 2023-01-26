import { HttpResponse } from '@angular/common/http';
import { AugurEntityController } from './augur-entity-controller';
import { AugurHttpRequest } from './augur-http-request';

export class EntityListDataSource<TEntity, TModel> {
    constructor(private apiController: AugurEntityController<TEntity>, 
        private httpList: (pageNumber: number, pageSize: number, searchQuery: string, columnFilter: any) => AugurHttpRequest<HttpResponse<TModel[]>>) { }

    get isLoading(): boolean {
      return this.listRequest && this.listRequest.isLoading;
    }

    listRequest: AugurHttpRequest<HttpResponse<TModel[]>>;

    entities: TModel[];

    _searchQuery: string;
    get searchQuery() {
     return this._searchQuery;
    }
    set searchQuery(val) {
      this._searchQuery = val;
      this.loadData(this.lastColumnFilter)
    }

    pageSize: number = 25;
    pageNumber: number = 1;

    private _totalCount: number = 0;
    private lastColumnFilter: any;

    get totalCount(): number {
        return this._totalCount;
    }

    async loadData(columnFilter) {

        this.lastColumnFilter = columnFilter;

        if(this.httpList) {
          this.listRequest = this.httpList(this.pageNumber, this.pageSize, this.searchQuery, columnFilter);
        }
        else {
          this.listRequest = this.apiController.list(this.pageNumber, this.pageSize, this.searchQuery, columnFilter);
        }

        const httpEvent = await this.listRequest;

        this._totalCount = JSON.parse(httpEvent.headers.get('x-pagination')).totalCount;
        this.entities = httpEvent.body;
    }
}
