import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ApiService } from '@services/api.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AppState } from '@app/app.service';
import { ToastMsgService } from '@services/toastmsg.service';
import { apiUrl } from '@app/config/configuration';

@Component({
  selector: 'forgotpassword',
  templateUrl: './forgotpassword.component.html',
  styleUrls: ['./forgotpassword.component.css']
})
export class ForgotPasswordComponent {
public PhoneNumber: string;
public userLocation: string;
constructor(private apiService: ApiService, private activatedRoute: ActivatedRoute,
            private router: Router,  public appState: AppState, private toastmsg: ToastMsgService) {
this.userLocation = localStorage.getItem('Country');
}

public onSubmit(form: NgForm) {
    const phoneNumber = form.value;
    console.log(phoneNumber);
    this.apiService.post('/forgotpassword', phoneNumber, undefined, apiUrl.authServer).then(
      (response: any) => {
        console.log(response);
        if (response.code === undefined) {
          throw response.error;
        }
        if (response.message === 'Success') {
         localStorage.setItem('UserName', phoneNumber.userName);
         if (this.userLocation === 'IN') {
         this.router.navigate(['../verify', phoneNumber.userName, 'forgotpassword'],
                              { relativeTo: this.activatedRoute});
         } else {
           this.router.navigate(['../checkemail'], { relativeTo: this.activatedRoute});
         }
        }
      })
      .catch(
      (error: any) => {
        if (error.code === '404') {
         this.toastmsg.popToast('error', 'Error', 'User not Registered');
        }
      }
    );
}
}
