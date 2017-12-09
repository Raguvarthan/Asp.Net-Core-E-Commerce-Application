import { ChangePasswordComponent } from './changepassword/changepassword.component';
import { CreateAccountComponent } from './createaccount/createaccount.component';
import { CheckEmailComponent } from './createaccount/checkemail/checkemail.component';
import { VerifyEmailComponent } from './createaccount/verifyemail/verifyemail.component';
import { ForgotPasswordComponent } from './forgotpassword/forgotpassword.component';
import { LoginRegisterComponent } from './loginregister/loginregister.component';
import { FaceBookSigninComponent } from './loginregister/facebook/facebooksignin.component';
import { GetEmailComponent } from './loginregister/facebook/getemail/getemail.component';
import { GoogleSigninComponent } from './loginregister/google/googlesignin.component';
import { UpdatePasswordComponent } from './updatepassword/updatepassword.component';
import { VerificationComponent } from './verification/verification.component';

export const AuthComponentBarrel = [
    ChangePasswordComponent, CheckEmailComponent, CreateAccountComponent,
    CreateAccountComponent, CreateAccountComponent, FaceBookSigninComponent,
    ForgotPasswordComponent, GetEmailComponent, GoogleSigninComponent,
    LoginRegisterComponent, UpdatePasswordComponent, VerificationComponent,
    VerifyEmailComponent];

export { AuthGuard } from './authguard/authguard';
