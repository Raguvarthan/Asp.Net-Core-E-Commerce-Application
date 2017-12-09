import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { CartService } from './cart.service';
import { WishListService } from './wishlist.service';
import { ToastMsgService } from './toastmsg.service';
import { AppState } from '@app/app.service';
import {Location} from '@angular/common';

declare const FB: any;

@Injectable()
export class LoginLogoutService {
    public fbResponse: any;
    constructor(private cartService: CartService, private wishListService: WishListService,
                private toastmsg: ToastMsgService, private appState: AppState,
                private router: Router, private location: Location) {
    }
    public Login(loginModel: any) {
        this.toastmsg.popToast('success', 'Success', 'Welcome!');
        localStorage.setItem('JWT', loginModel.accessToken);
        localStorage.setItem('FirstName', loginModel.firstName);
        localStorage.setItem('UserName', loginModel.userName);
        this.appState.set('loggedIn', true);
        this.cartService.getCartItems(loginModel.userName);
        this.wishListService.getWishListItems(loginModel.userName);
        this.location.back();
        //this.router.navigate(['/']);
    }
    public Logout() {
        this.cartService.refreshCart().then((res) => {
            this.cartService.cartItems.listOfProducts = [];
            this.wishListService.refreshList().then((resp) => {
                this.wishListService.wishListItems.listOfProducts = [];
                localStorage.removeItem('JWT');
            });
        });
        this.appState.set('loggedIn', false);
        FB.getLoginStatus((response) => {
            this.fbResponse = response;
            this.statusChangeCallback(response);
        });
        this.toastmsg.popToast('success', 'Success', 'Logged Out');
        this.router.navigate(['/']);
    }
    public statusChangeCallback(resp) {
        console.log(resp);
        if (resp.status === 'connected') {
            FB.logout((response) => {
            document.location.reload();
            });
            // connect here with your server for facebook login
            // by passing access token given by facebook
        } else if (resp.status === 'not_authorized') {
          console.log('not logged in');
        } else {
          console.log('different error');
        }
    };
}
