import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
    selector: 'addedtowishlist',
    templateUrl: './addedtowishlist.html'
})
export class AddedToWishListComponent {
constructor(private router: Router) {

}
public viewWishList() {
this.router.navigate(['/wishlist']);
}
public continue() {
this.router.navigate(['/']);
}
}
