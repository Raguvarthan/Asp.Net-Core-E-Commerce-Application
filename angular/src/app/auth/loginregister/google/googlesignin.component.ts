import { Component, ElementRef, AfterViewInit } from '@angular/core';
import { ApiService } from '@services/api.service';
import { ToastMsgService } from '@services/toastmsg.service';
import { LoginLogoutService } from '@services/loginlogout.service';
import { apiUrl } from '@app/config/configuration';

declare const gapi: any;

@Component({
  selector: 'google-signin',
  templateUrl: './googlesignin.component.html'
})
export class GoogleSigninComponent implements AfterViewInit {
  public auth2: any;
  private clientId: string =
  '761077619522-0t83hdhpc9h8ms3uf78cs3gu02bg6vjt.apps.googleusercontent.com';

  private scope = [
    'profile',
    'email',
    'https://www.googleapis.com/auth/plus.me',
    'https://www.googleapis.com/auth/contacts.readonly',
    'https://www.googleapis.com/auth/admin.directory.user.readonly'
  ].join(' ');

  constructor(private element: ElementRef, private apiService: ApiService,
              private loginLogout: LoginLogoutService, private toastmsg: ToastMsgService) {
  console.log('ElementRef: ', this.element);
}
  public googleInit() {
    let that = this;
    gapi.load('auth2', () => {
      that.auth2 = gapi.auth2.init({
        client_id: that.clientId,
        cookiepolicy: 'single_host_origin',
        scope: that.scope
      });
      that.attachSignin(that.element.nativeElement.firstChild);
    });
  }
  public attachSignin(element) {
    let that = this;
    this.auth2.attachClickHandler(element, {},
      (googleUser) => {
        let profile = googleUser.getBasicProfile();
        let postLogin = { ID: profile.getId(), Email: profile.getEmail(),
                          Token: googleUser.getAuthResponse().id_token };
        that.apiService.post('/externallogin/google', postLogin, undefined,
                             apiUrl.authServer).then(
          (response: any) => {
            if (response.value === undefined) {
              throw response.error;
            }
            if (response.value.code === '999') {
            let loginModel = { accessToken: response.value.data,
                               firstName: response.value.content.FirstName,
                               userName: profile.getEmail()};
            that.loginLogout.Login(loginModel);
            }
          })
          .catch(
          (error: any) => {
            if (error.code === '401' || error.code === '402') {
              that.toastmsg.popToast('error', 'Error', 'Please try again.');
            }
            if (error.code === '400') {
              that.toastmsg.popToast('error', 'Error', 'Please try again.');
            }
          }
          );
      }, (error) => {
    console.log(error);
  });
}

public ngAfterViewInit() {
  this.googleInit();
}
}
