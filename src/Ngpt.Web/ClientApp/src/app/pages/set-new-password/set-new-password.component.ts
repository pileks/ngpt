import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { IAugurHttpRequest } from '@augur';
import { SetNewPasswordModel } from '@platform';
import { PasswordResetController } from '@platform';

@Component({
  selector: 'app-set-new-password',
  templateUrl: './set-new-password.component.html',
  styleUrls: ['./set-new-password.component.css']
})
export class SetNewPasswordComponent {

  constructor(public passwordResetController: PasswordResetController, private router: Router, private route: ActivatedRoute) { }

  setNewPasswordVm: SetNewPasswordViewModel = new SetNewPasswordViewModel();
  signInRequest: IAugurHttpRequest;
  
  get hasSetNewPasswordFailed(): boolean {
    return this.signInRequest && this.signInRequest.hasErrors;
  }

  get isLoading(): boolean {
    return this.signInRequest && this.signInRequest.isLoading;
  }

  async setNewPassword() {
    this.setNewPasswordVm.uid = this.route.snapshot.paramMap.get('uid');

    this.signInRequest = this.passwordResetController.setNewPassword(this.setNewPasswordVm);

    await this.signInRequest;

    if(!this.hasSetNewPasswordFailed) {
      this.router.navigate(['/login']);
    }
  }
}

class SetNewPasswordViewModel extends SetNewPasswordModel {
  confirmedPassword: string;

  isValid() {
    return this.password && 
      this.confirmedPassword &&
      this.password === this.confirmedPassword;
  }
}
