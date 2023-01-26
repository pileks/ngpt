import { AugurEntityController, EntityListDataSource } from '@augur';
import { Directive, OnInit } from '@angular/core';

@Directive()
export class EntityListWithScopeFilterComponent<TEntity> implements OnInit {
    constructor(private apiController: AugurEntityController<TEntity>) {
    }

    dataSource = new EntityListDataSource<TEntity, TEntity>(
        this.apiController,
        null
    );

    shouldFilterForOrganization: boolean = false;
    shouldFilterOwn: boolean = false;

    columnFilter: any = {
        shouldFilterForOrganization: this.shouldFilterForOrganization,
        shouldFilterOwn: this.shouldFilterOwn
    };

    async ngOnInit() {
        this.dataSource.loadData(this.columnFilter);
    }

    async onFilterForOrganizationChanged() {
        this.columnFilter = {
            shouldFilterForOrganization: this.shouldFilterForOrganization,
            shouldFilterOwn: this.shouldFilterOwn
        };

        await this.dataSource.loadData(this.columnFilter);
    }

    async onFilterOwnChanged() {
        this.columnFilter = {
            shouldFilterForOrganization: this.shouldFilterForOrganization,
            shouldFilterOwn: this.shouldFilterOwn
        };

        await this.dataSource.loadData(this.columnFilter);
    }
}
