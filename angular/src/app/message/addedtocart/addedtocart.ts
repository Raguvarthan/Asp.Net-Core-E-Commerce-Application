import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
    selector: 'addedtocart',
    templateUrl: './addedincart.html'
})
export class AddedToCartComponent {

constructor(private router: Router) {

}

public viewBag() {
this.router.navigate(['/cart']);
}
public checkOut() {
this.router.navigate(['/checkout']);
}
public continue() {
this.router.navigate(['/']);
}

}
