import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ApiService } from '@services/api.service';
import { Router } from '@angular/router';
import { AppState } from '@app/app.service';
import { ToastMsgService } from '@services/toastmsg.service';
import { apiUrl } from '@app/config/configuration';

@Component({
  selector: 'changepassword',
  templateUrl: './changepassword.component.html',
  styleUrls: ['./changepassword.component.css']
})
export class ChangePasswordComponent {

  constructor(private apiService: ApiService, private router: Router,
              private toastmsg: ToastMsgService, public appState: AppState) {
  }

public  onSubmit(form: NgForm) {
    const changePassword = form.value;
    this.apiService.post('/changepassword', changePassword, undefined, apiUrl.authServer).then(
      (response: any) => {
        console.log(response);
        if (response.code === undefined) {
          throw response.error;
        }
        if (response.code === '200') {
          this.toastmsg.popToast('success', 'Success',
                                 'Password Changed.');
          this.router.navigate(['/']);
        }
      })
      .catch(
      (error: any) => {
        if (error.code === '400') {
          this.toastmsg.popToast('error', 'Error',
                                 'Server Error. Please try again');
        } else if (error.code === '404') {
          this.toastmsg.popToast('error', 'Error',
                                 'User not Found. Please Check Phone Number');
        } else if (error.code === '401') {
          this.toastmsg.popToast('error', 'Error',
                                 'Wrong Password.');
        }
      }
      );
  }
}
