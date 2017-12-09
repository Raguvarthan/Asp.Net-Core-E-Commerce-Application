import { Component, Input, OnInit } from '@angular/core';
import { WishListService } from '@services/wishlist.service';
import { CartService } from '@services/cart.service';
import { Router } from '@angular/router';

@Component({
  selector: 'listitem',
  templateUrl: './wishlistitem.component.html'
})
export class WishListItemComponent implements OnInit {

  @Input() public item: any;

  @Input() public itemIndex: number;

  constructor(private wishListService: WishListService, private cartService: CartService,
              private router: Router) {
  }

  public ngOnInit() {
    this.item.productQuantity = 1;
  }
  public totalAmount() {
    return this.item.productQuantity * this.item.productPrice;
  }
  public addOne() {
    if (this.item.productQuantity < 10) {
      this.item.productQuantity++;
    }
  }
  public reduceOne() {
    if (this.item.productQuantity > 1) {
      this.item.productQuantity--;
    }
  }
  public removeItem() {
    this.wishListService.wishListItems.listOfProducts.splice(this.itemIndex, 1);
  }
  public addToCart() {
    this.cartService.cartItems.listOfProducts.push(this.item);
    this.removeItem();
    this.router.navigate(['/addedtocart']);
  }

  public viewItem(){
  let viewItemUrl = './products/' + this.item.productFor + '/' + this.item.productType + '/variants/' + this.item.productDesign;
  this.router.navigate([viewItemUrl]);
  }

}
