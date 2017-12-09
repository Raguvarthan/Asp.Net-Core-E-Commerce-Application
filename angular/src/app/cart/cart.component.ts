import { Component, OnInit, OnDestroy } from '@angular/core';
import { CartService } from '@services/cart.service';
import { Router } from '@angular/router';

@Component({
  selector: 'cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss']
})
export class CartComponent implements OnInit, OnDestroy {

public cartItems: any;

constructor(public cartService: CartService, private route: Router) {
}

public ngOnInit() {
 this.cartItems = this.cartService.cartItems.listOfProducts;
}
public ngOnDestroy() {
    this.cartService.refreshCart();
}
public checkOut() {
this.route.navigate(['/checkout']);
}
}
