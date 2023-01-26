import { Component, OnInit, Input, EventEmitter, Output, ContentChildren, QueryList, TemplateRef } from '@angular/core';
import { EntityListDataSource } from '../entity-list-data-source';
import { AugurEntityController } from '../augur-entity-controller';

@Component({
    selector: 'augur-grid',
    templateUrl: './augur-grid.component.html',
    styleUrls: ['./augur-grid.component.css']
})
export class AugurGridComponent<TEntity> implements OnInit {

    constructor() {
    }

    @ContentChildren('columnTemplate') columnTemplates: QueryList<TemplateRef<any>>;

    @Input() apiController: AugurEntityController<TEntity>;
    @Input() columns: any;
    @Output() onRowClicked: EventEmitter<any> = new EventEmitter();

    @Input() dataSource: EntityListDataSource<TEntity, TEntity>;
    @Input() disableLoadOnInit: boolean;

    public properties: string[] = [];

    columnFilterValue: any = {};
    @Output() columnFilterChange = new EventEmitter();

    @Input()
    get columnFilter() {
        return this.columnFilterValue;
    }
    set columnFilter(val) {
        this.columnFilterValue = val;
        this.columnFilterChange.emit(this.columnFilter);
    }

    set pageSize(val: number) { this.dataSource.pageSize = val; this.dataSource.pageNumber = 1; this.loadData() }
    get pageSize() { return this.dataSource.pageSize; }

    set pageNumber(val: number) { this.dataSource.pageNumber = val; this.loadData(); }
    get pageNumber() { return this.dataSource.pageNumber; }

    get pageCount() {
        return Math.ceil(this.dataSource.totalCount / this.dataSource.pageSize);
    }

    get pages(): number[] {
        return Array.from({length: this.pageCount}, (_, i) => i + 1);
    }

    setPageNumber(pageNumber: number) {
        this.pageNumber = pageNumber;
    }

    ngOnInit() {
        for (let prop in this.columns) {
            this.properties.push(prop);
        }

        if (!this.disableLoadOnInit) {
            this.loadData();
        }
    }

    public onRowClickedInternal(entity: any): void {
        this.onRowClicked.emit(entity);
    }

    public loadData() {
        this.dataSource.loadData(this.columnFilter);
    }

    public onPageSizeChanged() {
        this.loadData();
    }

    public onColumnFilterChanged(propertyName: string) {
        this.dataSource.pageNumber = 1;
        this.loadData();
    }

    public shouldEnableColumnFilter(): boolean {
        return this.properties.some(prop => this.columns[prop].hasColumnFilter);
    }
}
