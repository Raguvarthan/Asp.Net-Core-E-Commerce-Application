import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { ApiService } from '@services/api.service';
import 'rxjs/add/observable/of';

@Injectable()
export class OrderListResolver implements Resolve<any> {

    constructor(private apiService: ApiService) {

    }
    public resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        let orders: any;

      let userName = localStorage.getItem('UserName');
      return  this.apiService.get('Order/vieworder/' + userName).then(
      (response: any) => {
      orders = response.data;
      console.log(orders.result);
      return orders.result;
      },
      (error: any) => {
      console.log(error);
      return error;
      }
    );
    }
}
