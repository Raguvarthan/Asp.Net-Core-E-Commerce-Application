import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { ApiService } from '../../services/api.service';
import 'rxjs/add/observable/of';

@Injectable()
export class VariantsResolver implements Resolve<any> {

    constructor(private apiService: ApiService) {

    }
    public resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        let variants: any;
        let variantKey = route.paramMap.get('productFor') + '-' +
            route.paramMap.get('productType') + '-' +
            route.paramMap.get('productDesign');

        variants = JSON.parse(localStorage.getItem(variantKey));
        if (variants === null) {
        return this.apiService.get('Product/' + route.paramMap.get('productFor') +
                '/' + route.paramMap.get('productType') +
                '/' + route.paramMap.get('productDesign')).then(
                (response: any) => {
                    let products = response.data;
                    variants = {};
                    variants.productDesign = route.paramMap.get('productDesign');
                    variants.topItem = products[0];
                    variants.variants = products;
                    return this.getProducts(route)
                    .then((resultData)=> {
                    localStorage.setItem(variantKey, JSON.stringify(variants));
                    let relatedItems = resultData.filter((productItem) => {
                    return productItem.productDesign !== route.paramMap.get('productDesign')
                    });
                    localStorage.setItem(variantKey + '-related', JSON.stringify(relatedItems));
                    return variants;
                    })
                },
                (error: any) => {
                    console.log(error);
                }
                );            
        } else {
            return variants;
        }
    }

public getProducts(route: ActivatedRouteSnapshot) {
    let products = [];
    return this.apiService.get('SubCategory/' + route.paramMap.get('productFor') + '/' + route.paramMap.get('productType')).then(
      (response: any) => {
      let products = response.data;
      return products             
    })
    .catch((err)=> {
      console.log(err);
      return products;
    })
}
}
