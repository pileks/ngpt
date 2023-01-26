import { AugurHttpRequest } from "./augur-http-request";
import { GridFilterModel, GridMetadata, GridRequestModel, GridResult, GridSortModel } from "./grid-types";

export class GridDataSource<TGridModel> {
    constructor(
        private gridFunction: (model: GridRequestModel) => AugurHttpRequest<GridResult<TGridModel>>) { }

    get isLoading(): boolean {
        return this.gridRequest && this.gridRequest.isLoading;
    }

    gridRequest: AugurHttpRequest<GridResult<TGridModel>>;

    data: TGridModel[];
    metadata: GridMetadata;

    sortBy: GridSortModel[] = [];
    filters: GridFilterModel[] = [];

    _search: string;
    get search() {
        return this._search;
    }
    set search(val) {
        this._search = val;
    }

    pageSize = 20;
    page = 1;

    get numPages(): number {
        return Math.ceil(this.totalCount / this.pageSize);
    }

    private _totalCount = 0;
    get totalCount(): number {
        return this._totalCount;
    }

    async loadData() {
        const model = new GridRequestModel({
            search: this.search,
            page: this.page,
            pageSize: this.pageSize,
            filters: this.filters,
            sortBy: this.sortBy
        });

        this.gridRequest = this.gridFunction(model);

        const resp = await this.gridRequest;

        this.data = resp.data;
        this.metadata = resp.metadata;
        this._totalCount = resp.count;
    }
};