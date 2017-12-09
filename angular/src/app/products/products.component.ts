import { Component, OnInit } from '@angular/core';
import { AppState } from '../app.service';
import { CartService } from '../../services/cart.service';
import { ApiService } from '../../services/api.service';
import { RouterModule } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { ParamMap } from '@angular/router';

@Component({
  selector: 'products',
  providers: [
  ],
  styleUrls: ['./products.component.css'],
  templateUrl: './products.component.html'
})
export class ProductsComponent implements OnInit {
  public localState = { value: '' };
  public products: any[] = [];
  public for: any;
  public type: any;
  constructor(public appState: AppState, private cartServ: CartService,
              private apiService: ApiService, private route: ActivatedRoute) {
      this.route.params.subscribe( (params) => {
      this.for = params['productFor'];
      this.type = params['productType'];
      this.ngOnInit();
      });
  }

public ngOnInit() {
 this.GetProducts();
}
public GetProducts() {
    this.apiService.get('SubCategory/' + this.for + '/' + this.type).then(
      (response: any) => {
      this.products = response.data;
      },
      (error: any) => {
      console.log(error);
      }
    );
}
public loadRelatedItems(removeDesign: any) { 

   let relatedItems = this.products.filter( (item) => {
   return item.productDesign !== removeDesign;
   });
   localStorage.setItem(this.for + '-' + this.type + '-' + removeDesign + '-related',
                         JSON.stringify(relatedItems)); 
}
}
