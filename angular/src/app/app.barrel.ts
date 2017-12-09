import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToasterModule, ToasterService, ToasterConfig } from 'angular2-toaster';
import { LazyLoadImageModule } from 'ng-lazyload-image';
import { CarouselModule } from 'angular4-carousel';
import { SpinnerModule } from 'angular-spinners';
import { SwiperModule } from 'ngx-swiper-wrapper';
import { SwiperConfigInterface } from 'ngx-swiper-wrapper';
import { ScrollToModule } from 'ng2-scroll-to';
import { RouterModule, PreloadAllModules } from '@angular/router';
import { ROUTES } from './app.routes';

const SWIPER_CONFIG: SwiperConfigInterface = {
  direction: 'horizontal'
};

export const AngularModules = [
    BrowserModule, FormsModule, HttpModule, HttpClientModule,
    BrowserAnimationsModule, ToasterModule, LazyLoadImageModule, CarouselModule,
    SpinnerModule, ScrollToModule.forRoot(), SwiperModule.forRoot(SWIPER_CONFIG),
    RouterModule.forRoot(ROUTES, { useHash: true, preloadingStrategy: PreloadAllModules })];

import { HeaderComponent } from '@app/header';
import { FooterComponent } from '@app/footer';
import { HomeComponentBarrel } from '@app/home';
import { PolicyComponentBarrel } from '@app/policies';
import { AuthComponentBarrel, AuthGuard } from '@app/auth';
import { CartComponentBarrel } from '@app/cart';
import { CheckOutComponentBarrel } from '@app/checkout';
import { OrderListComponentBarrel, OrderListResolver } from '@app/orderlist';
import { ProductsComponentBarrel } from '@app/products';
import { VariantsComponentBarrel, VariantsResolver } from '@app/variants';
import { AccountComponentBarrel } from '@app/account';
import { WishListComponentBarrel } from '@app/wishlist';
import { MessageComponentBarrel } from '@app/message';
import { ServicesBarrel } from '../services';

export const ComponentsBarrel = [
    HeaderComponent, FooterComponent,HomeComponentBarrel,
    PolicyComponentBarrel, AuthComponentBarrel, CartComponentBarrel, CheckOutComponentBarrel,
    OrderListComponentBarrel, ProductsComponentBarrel, VariantsComponentBarrel,
    AccountComponentBarrel, WishListComponentBarrel, MessageComponentBarrel];

export const ProvidersBarrel = [
    AuthGuard, OrderListResolver, VariantsResolver, ServicesBarrel];