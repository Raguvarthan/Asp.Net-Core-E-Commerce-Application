import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { CartService } from '../../../services/cart.service';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'productitem',
  styleUrls: ['./productitem.component.css'],
  templateUrl: './productitem.component.html'
})
export class ProductItemComponent implements OnInit {

@Input() public productitem: any;
@Output() public clickedItem = new EventEmitter<{}>();

public design: string;
public Price: string;
public ImgUrl: string;
public for: string;
public type: string;

constructor(private cartServ: CartService, private router: Router,
            private activatedRoute: ActivatedRoute) {
      this.for = activatedRoute.snapshot.paramMap.get('productFor');
      this.type = activatedRoute.snapshot.paramMap.get('productType');
}
public ngOnInit() {
this.design = this.productitem.productDesign;
this.Price = this.productitem.topItem.productPrice;
this.ImgUrl = this.productitem.topItem.minioObject_URL;
}

public DesignClicked() {
    localStorage.setItem(this.for + '-' + this.type + '-' + this.design,
                         JSON.stringify(this.productitem));
    this.clickedItem.emit(this.design);
}
}
