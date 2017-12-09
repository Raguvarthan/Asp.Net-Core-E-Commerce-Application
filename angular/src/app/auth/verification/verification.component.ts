import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ApiService } from '@services/api.service';
import { LoginLogoutService } from '@services/loginlogout.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AppState } from '@app/app.service';
import { ToastMsgService } from '@services/toastmsg.service';
import { apiUrl } from '@app/config/configuration';

@Component({
  selector: 'verification',
  templateUrl: './verification.component.html',
  styleUrls: ['./verification.component.scss']
})
export class VerificationComponent {
  public PhoneNumber: string;
  public JWT: any;
  public action: string;
  public postUrl: string;
  constructor(private apiService: ApiService, private toastmsg: ToastMsgService,
              private activatedRoute: ActivatedRoute, private router: Router,
              public appState: AppState, private loginLogout: LoginLogoutService) {
    this.PhoneNumber = activatedRoute.snapshot.paramMap.get('PhoneNumber');
    this.action = activatedRoute.snapshot.paramMap.get('action');
  }

public onSubmit(form: NgForm) {
    if (this.action === 'createaccount') {
     this.postUrl = '/register/verification/' +
                    this.PhoneNumber + '/' + form.value.VerificationCode;
    } else if (this.action === 'forgotpassword') {
    this.postUrl = '/forgotpassword/verification/'  +
                   this.PhoneNumber + '/' + form.value.VerificationCode;
    }
    const verificationCode = form.value;
    verificationCode.PhoneNumber = this.PhoneNumber;
    this.apiService.get(this.postUrl, undefined, apiUrl.authServer).then(
      (response: any) => {
        if (response.value === undefined) {
          throw response.error;
        }
        if (response.value.code === '999') {
        let loginModel = { accessToken: response.value.data,
                           firstName: response.value.content.FirstName,
                           userName: verificationCode.PhoneNumber};
        this.loginLogout.Login(loginModel);
        } else if (response.value.code === '201') {
          this.JWT = response.value.data;
          localStorage.setItem('JWT', this.JWT);
          localStorage.setItem('FirstName', response.value.content.FirstName);
          localStorage.setItem('UserName', verificationCode.PhoneNumber);
          this.toastmsg.popToast('success', 'Success', 'Verified!');
          this.router.navigate(['/updatepassword', verificationCode.PhoneNumber]);
        }
      })
      .catch(
      (error: any) => {
        if (error.code === '404') {
          this.toastmsg.popToast('error', 'Error', 'User not Registered');
          this.router.navigate(['/forgotpassword']);
        } else if (error.code === '401') {
          this.toastmsg.popToast('error', 'Error',
                                 'OTP expired. Please try again');
          this.router.navigate(['/createaccount']);
        } else if (error.code === '400') {
          this.toastmsg.popToast('error', 'Error', 'Invalid OTP');
        }
      }
    );
  }
}
