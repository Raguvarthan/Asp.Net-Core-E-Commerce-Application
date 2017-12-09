import { Component, OnInit, OnDestroy } from '@angular/core';
import { WishListItemComponent } from 'wishlistitem/wishlistitem.component';
import { WishListService } from '../../services/wishlist.service';

@Component({
  selector: 'wishlist',
  templateUrl: './wishlist.component.html',
  styleUrls: ['./wishlist.component.scss']
})
export class WishListComponent implements OnInit, OnDestroy {

public wishListItems: any;

constructor(public wishListService: WishListService) {

}

public ngOnInit() {
 this.wishListItems = this.wishListService.wishListItems.listOfProducts;
 console.log(this.wishListService.wishListItems.listOfProducts);
}

public ngOnDestroy() {
    this.wishListService.refreshList();
}
}
