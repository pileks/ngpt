import { Component, Input, OnInit } from '@angular/core';
import { GridDataSource } from '@augur';

type PageModel = {
    number?: number;
    isEllipsis: boolean;
};

@Component({
    selector: 'app-grid-pagination',
    templateUrl: './grid-pagination.component.html',
    styleUrls: ['./grid-pagination.component.css']
})
export class GridPaginationComponent<TGridModel> implements OnInit {

    constructor() { }

    @Input() dataSource: GridDataSource<TGridModel>;

    ngOnInit(): void {
        if (this.dataSource === null || this.dataSource === undefined) {
            throw new TypeError("The property 'dataSource' is required!");
        }
    }

    get pages(): PageModel[] {
        let pages = new Array<PageModel>();

        if (this.dataSource.numPages < 10) {
            for (var i = 1; i <= this.dataSource.numPages; i++) {
                pages.push({ number: i, isEllipsis: false });
            }

        }
        else if (this.dataSource.page <= 5) {
            //render one ellipsis to the right end
            for (var i = 1; i <= 7; i++) {
                pages.push({ number: i, isEllipsis: false });
            }

            pages.push({ isEllipsis: true });
            pages.push({ number: this.dataSource.numPages, isEllipsis: false });

        } else if (this.dataSource.page >= this.dataSource.numPages - 4) {
            //render one ellipsis to the left end
            pages.push({ number: 1, isEllipsis: false });
            pages.push({ isEllipsis: true });

            for (var i = this.dataSource.numPages - 6; i <= this.dataSource.numPages; i++) {
                pages.push({ number: i, isEllipsis: false });
            }

        } else {
            //render two ellipses
            pages.push({ number: 1, isEllipsis: false });
            pages.push({ isEllipsis: true });

            for (var i = this.dataSource.page - 2; i <= this.dataSource.page + 2; i++) {
                pages.push({ number: i, isEllipsis: false });
            }

            pages.push({ isEllipsis: true });
            pages.push({ number: this.dataSource.numPages, isEllipsis: false });

        }

        return pages;
    }

    get currentPageStart(): number {
        return (this.dataSource.page - 1) * this.dataSource.pageSize + 1;
    }
    get currentPageEnd(): number {
        if (this.dataSource.page == this.dataSource.numPages) {
            return this.dataSource.totalCount;
        }
        return (this.dataSource.page - 1) * this.dataSource.pageSize + this.dataSource.pageSize;
    }
    get totalCount(): number {
        return this.dataSource.totalCount;
    }

    setPage(i: number) {
        this.dataSource.page = i;
        this.dataSource.loadData();
    }
}
