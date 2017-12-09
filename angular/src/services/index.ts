import { AddressService } from './address.service';
import { ApiService } from './api.service';
import { CartService } from './cart.service';
import { LoginLogoutService } from './loginlogout.service';
import { ToastMsgService } from './toastmsg.service';
import { TokenService } from './token.service';
import { WishListService } from './wishlist.service';
import { SpinnerService } from 'angular-spinners';


export const ServicesBarrel = [
    AddressService, ApiService, CartService, LoginLogoutService, ToastMsgService,
    TokenService, WishListService, SpinnerService];
