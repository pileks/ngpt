import { Injectable } from '@angular/core';
import { HttpRequest, HttpInterceptor, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AugurLocalStorageManager } from '@augur';

@Injectable({
    providedIn: 'root'
})
export class AddAuthorizationHeaderInterceptor implements HttpInterceptor {
    constructor(private localStorageManager: AugurLocalStorageManager) {}

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const token = this.localStorageManager.load<string>('token');

        if (request.headers.has('authorization')) {
            return next.handle(request);
        }

        if (token) {
            request = request.clone({ setHeaders: { Authorization: token } });
        }

        return next.handle(request);
    }
}
