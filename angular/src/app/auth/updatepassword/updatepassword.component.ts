import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ApiService } from '@services/api.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AppState } from '@app/app.service';
import { ToastMsgService } from '@services/toastmsg.service';
import { apiUrl } from '@app/config/configuration';

@Component({
  selector: 'updatepassword',
  templateUrl: './updatepassword.component.html',
  styleUrls: ['./updatepassword.component.css']
})
export class UpdatePasswordComponent {
public password: string;
public newPassword: { userName: string, Password: string} = { userName: '', Password: ''};
public userName: string;
constructor(private apiService: ApiService, private toastmsg: ToastMsgService,
            private activatedRoute: ActivatedRoute, private router: Router,
            public appState: AppState) {
    this.userName = activatedRoute.snapshot.paramMap.get('userName');
}

public onSubmit(form: NgForm) {
    this.newPassword.Password = form.value.Password;
    this.newPassword.userName = this.userName;
    this.apiService.post('/forgotpassword/changepassword',
                         this.newPassword, undefined, apiUrl.authServer).then(
      (response: any) => {
        if (response.code === '200') {
        this.toastmsg.popToast('success', 'Success', 'Password Changed');
        this.appState.set('loggedIn', true);
        this.router.navigate(['/']);
        }
      })
      .catch(
      (error: any) => {
        console.log(error);
      }
    );
}
}
