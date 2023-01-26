import { HttpInterceptor, HttpHandler, HttpRequest, HttpEvent, HttpResponse, HttpErrorResponse }   from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { AuthenticationService } from '../authentication/authentication.service';
import { Router } from '@angular/router';
import { ApiSystemEvent } from '@platform/enums/api-system-event';
import { VerboseExceptionsProcessor } from '@platform/services/verbose-exceptions-processor';
import { ToastrService } from 'ngx-toastr';

@Injectable({
    providedIn: 'root'
})
export class AppHttpResponseInterceptor implements HttpInterceptor {
    constructor(
        private authService: AuthenticationService,
        private router: Router,
        private verboseExceptionsProcessor: VerboseExceptionsProcessor,
        private toastrService: ToastrService
    ) {}
    intercept(
        req: HttpRequest<any>,
        next: HttpHandler
    ): Observable<HttpEvent<any>> {
        return next.handle(req).pipe(
            tap(resp => this.processApiSystemEvents(resp)),
            catchError((err: any) => {
                if (err instanceof HttpErrorResponse && err.status === 401) {
                    this.authService.clearStoredUserAndToken();

                    //this.router.navigate(['/login']);
                } else if (
                    err instanceof HttpErrorResponse &&
                    err.status === 403
                ) {
                } else if (
                    err instanceof HttpErrorResponse &&
                    err.status === 404
                  ) {
                      
                } else if (
                  err instanceof HttpErrorResponse &&
                  err.status === 500 &&
                  err.error.IsVerboseException
                ) {
                    this.verboseExceptionsProcessor.processException(err.error);
                } else {
                    this.toastrService.error('An error occurred on the server!');
                }

                throw err;
            })
        );
    }

    processApiSystemEvents(resp: any) {
        if (resp instanceof HttpResponse) {
            const systemEventHeader = resp.headers.get('ApiSystemEventNofication');

            if(systemEventHeader) {
                this.handleSystemEvent(Number(systemEventHeader));
            }
        }
    }    

    handleSystemEvent(systemEvent: ApiSystemEvent) {
        if(systemEvent === ApiSystemEvent.LoggedInUserChanged) {
            this.authService.fetchAndLoadLoggedInUser();
        }
    }
}
