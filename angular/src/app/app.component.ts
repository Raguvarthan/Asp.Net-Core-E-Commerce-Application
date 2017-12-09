/**
 * Angular 2 decorators and services
 */
import {
  Component,
  OnInit,
  AfterViewChecked,
  ViewEncapsulation
} from '@angular/core';
import { AppState } from './app.service';
import { Router, NavigationEnd } from '@angular/router';
import { ToastMsgService } from '../services/toastmsg.service';
import { ApiService } from '../services/api.service';
import { HeaderComponent } from './header';

import * as Config from './config/configuration';
/**
 * App Component
 * Top Level Component
 */
@Component({
  selector: 'app',
  encapsulation: ViewEncapsulation.None,
  styleUrls: [
    './app.component.scss'
  ],
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
  public angularclassLogo = 'assets/img/angularclass-avatar.png';
  public name = 'Angular 2 Webpack Starter';
  public url = 'https://twitter.com/AngularClass';
  public config: any;
  constructor(public appState: AppState, private router: Router,
              private toastmsg: ToastMsgService, private apiService: ApiService) {
              this.config = toastmsg.config;
  }

  public ngOnInit() {
    if (localStorage.getItem('UserName') != null && localStorage.getItem('JWT') != null) {
      this.appState.set('loggedIn', true);
    } else {
      this.appState.set('loggedIn', false);
    }
    console.log('Initial App State', this.appState.state);

    this.router.events.subscribe((evt) => {
      if (!(evt instanceof NavigationEnd)) {
        return;
      }
      window.scrollTo(0, 0);
    });
    this.apiService.get('', undefined, 'https://ipapi.co/json/').then(
      (response: any) => {
        localStorage.setItem('Country', response.country);
        let countryCode = Config.isdCodes.filter((country) => {
        return country.code === response.country; })[0];
        localStorage.setItem('IsdCode', countryCode['dial_code']);
      })
      .catch((error: any) => {
        console.log(error);
      });
  }
}
