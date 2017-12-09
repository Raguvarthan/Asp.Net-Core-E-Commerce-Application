import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService } from '@services/api.service';
import { ToastMsgService } from '@services/toastmsg.service';
import { LoginLogoutService } from '@services/loginlogout.service';
import { AppState } from '@app/app.service';
import { apiUrl } from '@app/config/configuration';

declare const FB: any;

@Component({
    selector: 'facebook-login',
    templateUrl: 'facebooksignin.component.html'
})

export class FaceBookSigninComponent {
    public fbResponse: any;
    constructor(private router: Router, public apiService: ApiService,
                private appState: AppState, private loginLogout: LoginLogoutService) {
        FB.init({
            appId: '281046555724146',
            cookie: false,
            xfbml: true,
            version: 'v2.10'
        });
    }
    public onFacebookLoginClick() {
        let that = this;
        FB.login(handlelogin);
        function handlelogin(loginresp) {
            console.log(loginresp);
            let postLogin = { ID: loginresp.authResponse.userID,
                              Token: loginresp.authResponse.accessToken };
            that.apiService.post('/externallogin/facebook/check', postLogin,
                                 undefined, apiUrl.authServer).then(
                (response: any) => {
                    if (response.value === undefined) {
                        throw response;
                    }
                    if (response.value.code === '999') {
                        let loginModel = { accessToken: response.value.data,
                                           firstName: response.value.content.FirstName,
                                           userName: response.value.content.UserName };
                        that.loginLogout.Login(loginModel);
                    }
                })
                .catch(
                (error: any) => {
                    if (error.code === '201') {
                        that.appState.set('registerFB', postLogin);
                        that.router.navigate(['./getemail']);
                    }
                }
                );
        }
    }
}
