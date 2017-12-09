import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { FormGroup, FormControl, FormArray, Validators } from '@angular/forms';
import { ApiService } from '@services/api.service';
import { ActivatedRoute, Params, Router, ParamMap } from '@angular/router';
import { ToastMsgService } from '@services/toastmsg.service';
import { apiUrl } from '@app/config/configuration';

@Component({
  selector: 'createaccount',
  templateUrl: './createaccount.component.html',
  styleUrls: ['./createaccount.component.scss']
})
export class CreateAccountComponent {
  public userLocation: string;
  public dialCode: string;
  constructor(private apiService: ApiService, private route: ActivatedRoute,
              private router: Router, private toastmsg: ToastMsgService) {
    this.userLocation = localStorage.getItem('Country');
    this.dialCode = localStorage.getItem('IsdCode');
  }

  public onSubmit(form: NgForm) {
    let userDetails: any = {};
    userDetails = form.value;
    this.apiService.post('/register', userDetails, undefined, apiUrl.authServer).then(
      (response: any) => {
        if (response.message === 'User Registered') {
          this.toastmsg.popToast('success', 'Success', 'Account Created');
          localStorage.setItem('UserName', userDetails.PhoneNumber);
          if (this.userLocation === 'IN') {
            this.router.navigate(['../verify', userDetails.PhoneNumber, 'createaccount'],
              { relativeTo: this.route });
          } else {
            this.router.navigate(['../checkemail']);
          }
        } else {
          throw response.error;
        }
      })
      .catch(
      (error: any) => {
        if (error.message === 'User Already Registered') {
          this.toastmsg.popToast('info', 'Msg', 'User already Registered');
        }
      }
      );
  }
}
