import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Injectable({
    providedIn: 'root'
})
export class VerboseExceptionsProcessor {
    constructor(private toastrService: ToastrService) {
    }

    exceptionTypeActions = {
        'SimpleVerboseException': (data) => {
            this.toastrService.error(data.VerboseMessage);
        }
    };

    processException(ex) {
        this.exceptionTypeActions[ex.Type](ex.Data);
    }
}
