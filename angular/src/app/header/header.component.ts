import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AppState } from '../app.service';
import { NgZone } from '@angular/core';
import { ToastMsgService } from '@services/toastmsg.service';
import { CartService } from '@services/cart.service';
import { WishListService } from '@services/wishlist.service';
import { LoginLogoutService } from '@services/loginlogout.service';

@Component({
  selector: 'header',  // <header></header>

  styleUrls: ['./header.component.css'],

  templateUrl: './header.component.html'
})
export class HeaderComponent implements OnInit {

  public menuOpened: boolean = false;
  public scrolled: boolean = false;
  public mobile: boolean = false;
  public clickedInsidePopup = false;
  public loggedIn: { 'loggedIn': boolean } = { loggedIn: false};
  public wishlistCount: number = 0;
  public cartCount: number = 0;

  constructor(private router: Router, public appState: AppState,
              zone: NgZone, private toastmsg: ToastMsgService,
              private cartService: CartService, private wishListService: WishListService,
              private loginLogout: LoginLogoutService) {
    this.loggedIn = this.appState.get('loggedIn');
    window.onscroll = () => {
      zone.run(() => {
      if (window.pageYOffset > 0 && window.screen.width > 600) {
        this.scrolled = true;
        } else {
        this.scrolled = false;
      }
      if ( window.screen.width < 600) {
        this.mobile = true;
      }
      });
  }; 
}
public ngOnInit() {
    if (this.appState.get('loggedIn') === true) {
    this.cartService.getCartItems(localStorage.getItem('UserName'));
    this.wishListService.getWishListItems(localStorage.getItem('UserName'));
    }
  }
public openMenu() {
    this.menuOpened = !this.menuOpened;
  }
public LoginRegister() {
    this.menuOpened = false;
    if (this.appState.get('loggedIn') !== true) {
      this.router.navigate(['/loginregister']);
    }
  }
public ChangePassword() {
    this.router.navigate(['/changepassword']);
  }
public SignOutClicked() {
    this.loggedIn.loggedIn = false;
    this.menuOpened = false;
    this.loginLogout.Logout();
  }
public getUserName() {
  return localStorage.getItem('FirstName');
}

public onMouseHover(){ 
let el = document.querySelectorAll('.popupAfterLogin > a');
for( let i = 0 ; i < el.length; i++){
  el[i].addEventListener('click', this.classToggle);
}
document.querySelector('.userLoggedIn').addEventListener('mouseover',this.removeClass);
}
public classToggle(){
document.querySelector('.userLoggedIn').classList.toggle('deactive');
}
public removeClass(){
document.querySelector('.userLoggedIn').classList.remove('deactive');
}
}

