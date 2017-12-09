import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ApiService } from '@services/api.service';
import { LoginLogoutService } from '@services/loginlogout.service';
import { ToastMsgService } from '@services/toastmsg.service';
import { apiUrl } from '@app/config/configuration';

@Component({
  selector: 'app-loginregister',
  templateUrl: './loginregister.component.html',
  styleUrls: ['./loginregister.component.css']
})
export class LoginRegisterComponent implements OnInit {

public userLocation: string;
  constructor(private apiService: ApiService, private toastmsg: ToastMsgService,
              private loginLogout: LoginLogoutService) {
  }

public ngOnInit() {
this.userLocation = localStorage.getItem('Country');
}
public  onSignin(form: NgForm) {
    const loginDetails = form.value;
    this.apiService.post('/login', loginDetails, undefined, apiUrl.authServer).then(
      (response: any) => {
        if (response.value === undefined) {
          throw response.error;
        }
        if (response.value.code === '999') {
          let loginModel = { accessToken: response.value.data,
                             firstName: response.value.content.FirstName,
                             userName: loginDetails.UserName};
          this.loginLogout.Login(loginModel);
        }
      })
      .catch(
      (error: any) => {
        if (error.code === '401') {
          this.toastmsg.popToast('error', 'Error', 'Wrong Credentials. Please try again');
        }
        if (error.code === '404') {
          this.toastmsg.popToast('error', 'Error', 'User not Found. Please register to continue.');
        }
      }
    );
  }
}
