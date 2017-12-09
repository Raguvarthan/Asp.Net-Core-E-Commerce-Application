import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiService } from '@services/api.service';
import { ToastMsgService } from '@services/toastmsg.service';
import { LoginLogoutService } from '@services/loginlogout.service';
import { AppState } from '@app/app.service';
import { apiUrl } from '@app/config/configuration';

@Component({
  selector: 'getemail',
  templateUrl: './getemail.component.html'
})
export class GetEmailComponent {

constructor(private apiService: ApiService, private appState: AppState,
            private loginLogout: LoginLogoutService) {

}

public onSubmit(form: NgForm) {
            let fbLogin = this.appState.get('registerFB');
            let postLogin = { ID: fbLogin.ID, Token: fbLogin.Token, Email: form.value.email };
            this.apiService.post('/externallogin/facebook', postLogin, undefined,
                                 apiUrl.authServer).then(
                (response: any) => {
                    if (response.value === undefined) {
                        throw response;
                    }
                    if (response.value.code === '999') {
                        let loginModel = { accessToken: response.value.data,
                                           firstName: response.value.content.FirstName,
                                           userName: response.value.content.UserName };
                        this.loginLogout.Login(loginModel);
                    }
                })
                .catch(
                (error: any) => {
                    console.log(error);
                }
                );
}
}
