import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { IGridFilterEntityResolver, GridFilterModel, IGridFilterEnumResolver, GridFilterMetadata } from '@augur';

class FilterModel {
    constructor(init?: Partial<FilterModel>) {
        Object.assign(this, init);
    }

    metadata: GridFilterMetadata;
    operator: number;
    value: any;
}

@Component({
    selector: 'app-grid-filter-builder',
    templateUrl: './grid-filter-builder.component.html',
    styleUrls: ['./grid-filter-builder.component.css']
})
export class GridFilterBuilderComponent implements OnInit {

    constructor() { }

    @Input() filters: GridFilterModel[];
    @Output() filtersChange = new EventEmitter<GridFilterModel[]>();

    @Input() filterMetadata: GridFilterMetadata[];

    _filterMetadata: GridFilterMetadata[] = [];

    @Input() entityResolver: IGridFilterEntityResolver;
    @Input() enumResolver: IGridFilterEnumResolver;

    filterModels: FilterModel[];

    ngOnInit(): void {
        if (this.filters === null || this.filters === undefined) {
            throw new TypeError('The input "filters" is required!');
        }
        if (this.filterMetadata === null || this.filterMetadata === undefined) {
            throw new TypeError('The input "filterMetadata" is required!');
        }

        this.filterMetadata.forEach(val => this._filterMetadata.push(Object.assign({}, val)));

        this.filterModels = this.filters.map(x => new FilterModel({
            value: x.value,
            metadata: this._filterMetadata.find(m => m.property === x.property),
            operator: 1
        }));

        if (this.filterModels.length === 0) {
            this.addFilter();
        }
    }

    addFilter() {
        this.filterModels.push(new FilterModel({
            metadata: this._filterMetadata[0],
            operator: 1,
            value: undefined
        }));
    }

    removeFilter(filter: FilterModel) {
        const i = this.filterModels.indexOf(filter);
        this.filterModels.splice(i, 1);
    }

    applyFilters() {
        this.filters = this.filterModels.map(x => new GridFilterModel({
            property: x.metadata.property,
            operator: x.operator,
            value: x.value
        }));

        this.filtersChange.emit(this.filters);
    }

    clearFilters() {
        this.filterModels = [];

        this.applyFilters();
    }

    onFilterMetadataChange(filter: FilterModel) {
        filter.value = null;
        filter.operator = 1;
    }

    booleanValues = [
        {
            title: 'Yes',
            value: true
        },
        {
            title: 'No',
            value: false
        },
    ];
}