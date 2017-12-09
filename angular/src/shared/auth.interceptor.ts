import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { Injectable, Injector } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { TokenService } from '@services/token.service';
import * as Util from '@shared/utils/utils';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    constructor(private inj: Injector) {

    }
public intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
     //   console.log("intercepted");
        return next.handle(req);
    }
}
