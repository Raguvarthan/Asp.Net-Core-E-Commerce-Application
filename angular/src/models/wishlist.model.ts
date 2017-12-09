export class WishListItem {
public productSKU: string;
public minioObject_URL: string;
public productFor: string;
public productDesign: string;
public productBrand: string;
public productPrice: string;
public productDiscount: string;
public productDiscountPrice: string;
public productQuantity: number;
public productSize: string;
public productColour: string;
public productDescription: string;
public userName: string;

constructor(productSKU: string, minioObject_URL: string, productFor: string,
            productDesign: string, productBrand: string, productPrice: string,
            productDiscount: string, productDiscountPrice: string,
            productQuantity: number, productSize: string, productColour: string,
            productDescription: string, userName: string) {
this.productSKU = productSKU;
this.minioObject_URL = minioObject_URL;
this.productFor = productFor;
this.productDesign = productDesign;
this.productBrand = productBrand;
this.productPrice = productPrice;
this.productDiscount = productDiscount;
this.productDiscountPrice = productDiscountPrice;
this.productQuantity = productQuantity;
this.productSize = productSize;
this.productColour = productColour;
this.productDescription = productDescription;
this.userName = userName;
}
}

export class WishList {
 public listOfProducts: WishListItem[];
 constructor() {
     this.listOfProducts = [];
 }
}
