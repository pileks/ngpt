import { Component, OnInit, Inject, Input } from '@angular/core';
import { Router } from '@angular/router';
import { IAugurHttpRequest } from '@augur';
import { PasswordResetController } from '@platform';

@Component({
    selector: 'app-reset-password',
    templateUrl: './reset-password.component.html',
    styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit {
    constructor(private passwordResetController: PasswordResetController, private router: Router) {
    }

    email: string;
    request: IAugurHttpRequest = null;
    hasSentPasswordResetEmail: boolean;

    get isLoading(): boolean {
      return this.request && this.request.isLoading;
    }

    async ngOnInit() {}

    async requestPasswordReset() {
        this.request = this.passwordResetController.requestPasswordReset({ email: this.email});

        await this.request;

        this.hasSentPasswordResetEmail = true;
    }

    isValid() {
        return this.email && 
            this.email.includes('@');
    }
}
