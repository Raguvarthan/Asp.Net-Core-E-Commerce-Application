import { Component, OnInit, Input, OnChanges } from '@angular/core';
import { ToastMsgService } from '@services/toastmsg.service';

@Component({
  selector: 'colorsizestock',
  providers: [
  ],
  styleUrls: ['./colorsizestock.component.css'],
  templateUrl: './colorsizestock.component.html'
})
export class ColorSizeStockComponent implements OnInit, OnChanges {
  public selectedSize: string = '';
  public remainingQty: string;
  public isDataLoaded: boolean;
  public quantity: number = 1;
  public itemToCart: any;
  public selectColor: string = 'Select a Colour';
  public selectSize: string = 'Select a Size';

  @Input() public selectedVariant: any;
  @Input() public initItem: any;
  public topItem: any = {};
  constructor(private toastMsg: ToastMsgService) {
  }
  public ngOnInit() {
    this.isDataLoaded = true;
    this.topItem = this.initItem.topItem;
    if (this.topItem.productSize === '') {
        this.selectedVariant = {};        
        this.selectedVariant.variants = [];
        this.selectedVariant.variants.push(this.topItem);
        this.checked('');        
    }
  }

  public ngOnChanges() {
    if (this.selectedSize !== '') {
      this.checked(this.selectedSize);
    }
    if (this.remainingQty === this.selectColor && this.selectedVariant) {
      this.checked(this.selectedSize);
    }
    this.quantity = 1;
  }
  public isAvailable(size: any) {
    if (this.selectedVariant) {
      let index = this.selectedVariant.variants.findIndex((myObj) =>
        myObj['productSize'] === size);
      if (this.selectedVariant.variants[index].productStock > 0) {
        return false;
      } else {
        return true;
      }
    }
  }
  public checked(size: string) {
    this.selectedSize = size;
    if (this.selectedVariant) {
      let index = this.selectedVariant.variants.findIndex((myObj) =>
        myObj['productSize'] === size);
      if (this.selectedVariant.variants[index].productStock === 0) {
        this.selectedSize = null;
        this.remainingQty = this.selectSize;
      } else {
        this.remainingQty = this.selectedVariant.variants[index].productStock;
        this.itemToCart = this.selectedVariant.variants[index];
      }
    } else {
      this.remainingQty = this.selectColor;
    }
    this.quantity = 1;
  }
  public addOne() {
    if (this.quantity < 10 && this.quantity < Number(this.remainingQty)) {
      this.quantity++;
    } else {
      this.toastMsg.popToast('info', 'Info', 'Please Select a Colour and Size.');
    }
  }
  public reduceOne() {
    if (this.quantity > 1) {
      this.quantity--;
    }
  }
}
