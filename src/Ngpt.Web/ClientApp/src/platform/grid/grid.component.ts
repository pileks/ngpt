import { Component, ContentChildren, EventEmitter, Input, OnInit, Output, QueryList } from '@angular/core';
import { GridColumnDirective } from '@platform/grid-column/grid-column.directive';
import { IGridFilterEntityResolver, GridFilterModel, GridSortModel, GridDataSource, IGridFilterEnumResolver } from '@augur';

@Component({
    selector: 'app-grid',
    templateUrl: './grid.component.html',
    styleUrls: ['./grid.component.css']
})
export class GridComponent<TGridModel> implements OnInit {

    constructor() { }

    async ngOnInit(): Promise<void> {
        await this.dataSource.loadData();

        this.columnTemplates.forEach((x) => {
            this.columns[x.property] = x.template;
        });
    }

    columns: any = {};

    @ContentChildren(GridColumnDirective) columnTemplates: QueryList<GridColumnDirective>;

    @Input() dataSource: GridDataSource<TGridModel>;
    @Input() entityResolver: IGridFilterEntityResolver;
    @Input() enumResolver: IGridFilterEnumResolver;

    @Output() onRowClicked = new EventEmitter<TGridModel>();
    @Output() onRowMiddleClicked = new EventEmitter<TGridModel>();
    @Output() onRowCtrlClicked = new EventEmitter<TGridModel>();

    searchQuery: string;
    displayFilters: boolean = false;

    onSearchQueryChange() {
        this.dataSource.search = this.searchQuery;
        this.debouncedLoadData();
    }

    toggleSort(column: string) {
        let sort = this.getSortForColumn(column);

        if (sort) {
            if (sort.direction === 'asc') {
                sort.direction = 'desc';
            } else {
                const i = this.dataSource.sortBy.indexOf(sort);
                this.dataSource.sortBy.splice(i, 1);
            }
        } else {
            let newSort = new GridSortModel({
                column: column,
                direction: 'asc'
            });
            this.dataSource.sortBy.push(newSort);
        }

        this.dataSource.loadData();
    }

    getColumnSortDirection(column: string) {
        const sort = this.getSortForColumn(column);

        return sort ? sort.direction : '';
    }

    getSortForColumn(column: string) {
        return this.dataSource.sortBy.find((x) => x.column === column);
    }

    toggleFilters() {
        this.displayFilters = !this.displayFilters;
    }

    get hasActiveFilters(): boolean {
        return this.displayFilters || this.dataSource.filters.length > 0;
    }

    get pages(): number[] {
        return Array.from({ length: this.dataSource.numPages }, (_, i) => i + 1);
    }

    setPage(i: number) {
        this.dataSource.page = i;
        this.dataSource.loadData();
    }

    debounce = (fn: Function, ms = 300) => {
        let timeoutId: ReturnType<typeof setTimeout>;
        return function (this: any, ...args: any[]) {
            clearTimeout(timeoutId);
            timeoutId = setTimeout(() => fn.apply(this, args), ms);
        };
    };

    debouncedLoadData = this.debounce(() => {
        this.dataSource.loadData();
    }, 500);

    onFiltersChange(filters: GridFilterModel[]) {
        this.dataSource.filters = filters;
        this.dataSource.loadData();
    }

    onClick(event: MouseEvent, row: TGridModel) {
        if (event.ctrlKey || event.metaKey) {
            this.onRowCtrlClicked.emit(row);
        } else {
            this.onRowClicked.emit(row);
        }
    }

    onAuxClick(event: MouseEvent, row: TGridModel) {
        if (event.which == 2) {
            this.onRowMiddleClicked.emit(row);
        }
    }
}
