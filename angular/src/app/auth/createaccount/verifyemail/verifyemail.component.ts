import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { apiUrl } from '@app/config/configuration';
import { ApiService } from '@services/api.service';
import { AppState } from '@app/app.service';
import { ToastMsgService } from '@services/toastmsg.service';

@Component({
  selector: 'verifyemail',
  templateUrl: './verifyemail.component.html'
})
export class VerifyEmailComponent implements OnInit {

  public postUrl: string;
  public userName: string;
  public otp: string;
  public update: string;
  public JWT: any;
  constructor(private router: Router, private activatedRoute: ActivatedRoute,
              private apiService: ApiService, private appState: AppState,
              private toastmsg: ToastMsgService) {
    this.userName = activatedRoute.snapshot.paramMap.get('userName');
    this.otp = activatedRoute.snapshot.paramMap.get('otp');
    this.update = activatedRoute.snapshot.paramMap.get('update');
  }
  public ngOnInit() {
    this.postUrl = '/register/verification/' + this.userName + '/' + this.otp;
    this.apiService.get(this.postUrl, undefined, apiUrl.authServer).then(
      (response: any) => {
        if (response.value === undefined) {
          throw response.error;
        }
        if (response.value.code === '999') {
          this.JWT = response.value.data;
          localStorage.setItem('JWT', this.JWT);
          localStorage.setItem('FirstName', response.value.content.FirstName);
          localStorage.setItem('UserName', this.userName);
          this.appState.set('loggedIn', true);
          this.toastmsg.popToast('success', 'Success', 'Verified!');
        }
      })
      .catch(
      (error: any) => {
        if (error.code === '404') {
          this.toastmsg.popToast('error', 'Error', 'User not Registered');
          this.router.navigate(['/forgotpassword']);
        } else if (error.code === '401') {
          this.toastmsg.popToast('error', 'Error',
            'Link expired. Please try again');
          this.router.navigate(['/createaccount']);
        } else if (error.code === '402') {
          this.toastmsg.popToast('error', 'Error', 'Invalid Link');
        }
      }
      );
  }
  public updatePassword() {
    this.router.navigate(['../updatepassword', this.userName]);
  }
}
