import { Component, Input } from '@angular/core';

@Component({
  selector: 'orderitem',
  styleUrls: ['./orderitem.component.scss'],
  templateUrl: './orderitem.component.html'
})
export class OrderItemComponent {
@Input() public orderItem: any;
public opened: boolean = false;
constructor() {
    console.log(this.orderItem);
}
}
