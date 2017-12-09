import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'orderlist',
  styleUrls: ['./orderlist.component.scss'],
  templateUrl: './orderlist.component.html'
})
export class OrderListComponent implements OnInit {

public orders: any = {};
constructor(private route: ActivatedRoute) {

}
public ngOnInit(){
this.orders = this.route.snapshot.data['orders'];
console.log(this.orders);
}

}
