import { Component } from '@angular/core';
import { CartService } from '@services/cart.service';

@Component({
  selector: 'order',
  templateUrl: './order.component.html'
})
export class OrderComponent {

constructor(private cartService: CartService) {
}
public getCartTotal() {
      return this.cartService.cartItems.listOfProducts.reduce((acc, item) => {
            return acc + (Number(item.productPrice) * item.productQuantity);
        }, 0);
}
}
