import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import * as Util from '@shared/utils/utils';
import { TokenService } from '@services/token.service';

@Injectable()
export class AuthGuard implements CanActivate {
  constructor(private router: Router, private tokenService: TokenService) { }
public canActivate() {
    if (JSON.parse(localStorage.getItem('JWT')) != null) {
      let jwt = JSON.parse(localStorage.getItem('JWT')).access_token;
      if (!Util.expiredJwt(jwt)) {
        console.log('authguard-true');
        return true;
      } else {
      console.log('auth guard- token expired');
      return this.tokenService.getAuthToken().then((res)=> {
       return true;
      })
      .catch((err)=> {
       console.log('auth-guard -refresh failed');
       this.router.navigate(['/loginregister']);
       return false;        
      })
      }
    }
    console.log('auth-guard -false');
    this.router.navigate(['/loginregister']);
    return false;
  }
}
