import { NO_ERRORS_SCHEMA } from '@angular/core';
import {
  inject,
  async,
  TestBed,
  ComponentFixture
} from '@angular/core/testing';
import { Component } from '@angular/core';
import {
  BaseRequestOptions,
  ConnectionBackend,
  Http
} from '@angular/http';
import { MockBackend } from '@angular/http/testing';
/**
 * services
 */
import { RouterTestingModule } from '@angular/router/testing';
import { ToastMsgService } from '../../services/toastmsg.service';
import { ApiService } from '../../services/api.service';
import { TokenService } from '../../services/token.service';
import { LoginLogoutService } from '../../services/loginlogout.service';
import { CartService } from '../../services/cart.service';
import { AddressService } from '../../services/address.service';
import { WishListService } from '../../services/wishlist.service';
import { AppState } from '../app.service';
import { ToasterModule, ToasterService, ToasterConfig } from 'angular2-toaster';
import { HttpClientModule } from '@angular/common/http';
import { SpinnerService } from 'angular-spinners';

/**
 * Load the implementations that should be tested.
 */
import { HeaderComponent } from './header.component';

describe(`Header`, () => {
  let comp: HeaderComponent;
  let fixture: ComponentFixture<HeaderComponent>;

  /**
   * async beforeEach.
   */
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [HeaderComponent],
      schemas: [NO_ERRORS_SCHEMA],
      imports: [RouterTestingModule, ToasterModule, HttpClientModule],
      providers: [
        BaseRequestOptions,
        AppState, RouterTestingModule,
        ToastMsgService, ApiService, TokenService,
        LoginLogoutService, SpinnerService,
        CartService, WishListService,
        MockBackend,
        {
          provide: Http,
          useFactory: (backend: ConnectionBackend, defaultOptions: BaseRequestOptions) => {
            return new Http(backend, defaultOptions);
          },
          deps: [MockBackend, BaseRequestOptions]
        }
      ]
    })
    /**
     * Compile template and css.
     */
    .compileComponents();
  }));

  /**
   * Synchronous beforeEach.
   */
  beforeEach(() => {
    fixture = TestBed.createComponent(HeaderComponent);
    comp = fixture.componentInstance;

    /**
     * Trigger initial data binding.
     */
    fixture.detectChanges();
  });
  it('HeaderComponent should be defined', () => {
      expect(fixture.componentInstance).toBeDefined();
  });

});
