import { Observable } from 'rxjs';
import { AugurHttpRequest } from './augur-http-request';

declare global {
    interface Promise<T> {
        toAugurHttpRequest<TData>(): AugurHttpRequest<TData>;
        toAugurHttpRequestWithValidation<TData>(): AugurHttpRequest<TData>;
    }
}

declare module 'rxjs/internal/Observable' {
    interface Observable<T> {
        toAugurHttpRequest<TData>(): AugurHttpRequest<TData>;
        toAugurHttpRequestWithValidation<TData>(): AugurHttpRequest<TData>;
    }
}

export class AugurWebHelpers {
    static override() {
        Promise.prototype.toAugurHttpRequest = function<TData>() {
            var promise = this;
            
            var request = new AugurHttpRequest<TData>({
                isLoading: true,
                
                promise: promise.then(resp => {
                    request.hasErrors = false;
                    request.data = resp;
            
                    request.isSuccess = true;

                    return request.data;
                }, () => {
                    request.isSuccess = false;
                    request.onException && request.onException();
                }).finally(() => {
                    request.isLoading = false;
                    request.hasFinished = true;
                })
            });
            
            return request;
        };

        Promise.prototype.toAugurHttpRequestWithValidation = function<TData>() {
            var promise = this;
            
            var request = new AugurHttpRequest<TData>({
                isLoading: true,
                
                promise: promise.then(resp => {
                    request.hasErrors = resp.hasErrors;
                    request.data = resp.data;
            
                    request.isSuccess = true;

                    return request.data;
                }, () => {
                    request.isSuccess = false;
                    request.onException && request.onException();
                }).finally(() => {
                    request.isLoading = false;
                    request.hasFinished = true;
                })
            });
            
            return request;
        };
            
        Observable.prototype.toAugurHttpRequest = function<TData>() {
            var observable = this;

            return observable
                .toPromise()
                .toAugurHttpRequest();
        };

        Observable.prototype.toAugurHttpRequestWithValidation = function<TData>() {
            var observable = this;

            return observable
                .toPromise()
                .toAugurHttpRequestWithValidation();
        };
    } 
}

