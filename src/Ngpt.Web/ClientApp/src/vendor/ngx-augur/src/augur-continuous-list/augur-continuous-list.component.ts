import { Component, OnInit, ContentChild, TemplateRef, Input } from '@angular/core';
import { EntityListDataSource } from '../entity-list-data-source';

@Component({
    selector: 'augur-continuous-list',
    templateUrl: './augur-continuous-list.component.html',
    styleUrls: ['./augur-continuous-list.component.css']
})
export class AugurContinuousListComponent<TEntity> implements OnInit {

    constructor() {
        this.loadMore = this.loadMore.bind(this);
    }

    @ContentChild('item', { static: true }) itemTemplate: TemplateRef<any>;
    @ContentChild('footer', { static: true }) footerTemplate: TemplateRef<any>;

    @Input() dataSource: EntityListDataSource<TEntity, TEntity>;
    @Input() itemAction: (item: TEntity) => void;

    items: TEntity[];

    get isMoreDataAvailable() { return this.dataSource.pageNumber * this.dataSource.pageSize < this.dataSource.totalCount }

    get isLoading() {
        return this.dataSource.isLoading;
    }

    async loadMore() {
        this.dataSource.pageNumber++;

        await this.dataSource.loadData(null);
        
        this.items = this.items.concat(this.dataSource.entities);
    }

    async ngOnInit() {
        this.items = new Array<TEntity>();
     
        this.dataSource.pageNumber = 0;
        this.dataSource.pageSize = 10;
        
        await this.loadMore();
    }
}
