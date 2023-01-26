import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Inject } from '@angular/core';
import { AugurHttpRequest } from './augur-http-request';

export class AugurEntityController<TEntity> {
    constructor(protected controllerRoute: string, protected http: HttpClient, @Inject('BASE_URL') protected baseUrl: string) {
        this.get = this.get.bind(this);
        this.create = this.create.bind(this);
        this.update = this.update.bind(this);
        this.delete = this.delete.bind(this);
        this.getAll = this.getAll.bind(this);
        this.list = this.list.bind(this);
    }

    get(id: number): AugurHttpRequest<TEntity> {
        return this.http.get<TEntity>(this.baseUrl + this.controllerRoute + '/' + id)
            .toAugurHttpRequest();
    }

    create(entity: TEntity): AugurHttpRequest<TEntity> {
        return this.http.post<TEntity>(this.baseUrl + this.controllerRoute, entity)
            .toAugurHttpRequest();
    }

    update(id: number, entity: TEntity): AugurHttpRequest<TEntity> {
        return this.http.put<TEntity>(this.baseUrl + this.controllerRoute + '/' + id, entity)
            .toAugurHttpRequest();
    }

    delete(id: number): AugurHttpRequest<TEntity> {
        return this.http.delete<TEntity>(this.baseUrl + this.controllerRoute + '/' + id)
            .toAugurHttpRequest();
    }

    getAll(): AugurHttpRequest<TEntity[]> {
        return this.http.get<TEntity[]>(this.baseUrl + this.controllerRoute + '/getAll')
            .toAugurHttpRequest();
    }

    list(pageNumber: number, pageSize: number, searchQuery: string, columnFilter: any): AugurHttpRequest<HttpResponse<any[]>> {
        const queryParams = new HttpParams()
            .set('pageNumber', pageNumber ? pageNumber.toString() : '')
            .set('pageSize', pageSize ? pageSize.toString() : '')
            .set('searchQuery', searchQuery ? searchQuery.toString() : '');
    
        return this.http.post<any[]>(this.baseUrl + this.controllerRoute + '/list', columnFilter ? columnFilter : {}, { params: queryParams, observe: 'response' })
            .toAugurHttpRequest<HttpResponse<any[]>>();
    }
}
