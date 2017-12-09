import { Component, Input, OnInit, EventEmitter, Output } from '@angular/core';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'relateditem',

  styleUrls: ['./related.component.css'],

  templateUrl: './related.component.html'
})
export class RelatedComponent implements OnInit {

@Input() public productitem: any;
@Input() public indexNumber: any;

public design: string;
public Price: string;
public ImgUrl: string;
public for: string;
public type: string;
@Output() public variantItemClicked = new EventEmitter<{}>();

constructor(private router: Router, private activatedRoute: ActivatedRoute) {
      this.for = activatedRoute.snapshot.paramMap.get('productFor');
      this.type = activatedRoute.snapshot.paramMap.get('productType');
}

public ngOnInit() {
this.design = this.productitem.productDesign;
this.Price = this.productitem.topItem.productPrice;
this.ImgUrl = this.productitem.topItem.minioObject_URL;     
}
public DesignClicked() {
    this.variantItemClicked.emit(this.productitem);
}
}
