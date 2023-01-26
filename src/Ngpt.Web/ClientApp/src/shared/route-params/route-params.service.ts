import { ContentChild, TemplateRef, Injectable } from '@angular/core';
import { Route, ActivatedRoute } from '@angular/router';

@Injectable({
    providedIn: 'root'
})
export class RouteParams {
    constructor(private route: ActivatedRoute) {
    }

    get(paramName: string): string {
        return this.route.snapshot.paramMap.get(paramName);
    }

    getNumber(paramName: string): number {
        return Number(this.route.snapshot.paramMap.get(paramName));
    }

    getQueryParam(paramName: string): string {
        return this.route.snapshot.queryParamMap.get(paramName);
    }

    getQueryParamNumber(paramName: string): number {
        return Number(this.route.snapshot.queryParamMap.get(paramName));
    }
}
