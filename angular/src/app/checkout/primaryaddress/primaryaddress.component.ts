import { Component, Input } from '@angular/core';
import { AddressService } from '../../../services/address.service';

@Component({
  selector: 'primaryaddress',
  templateUrl: './primaryaddress.component.html'
})
export class PrimaryAddressComponent {

  public billingAddress: any = {};
  public shippingAddress: any = {};
  constructor(private addressService: AddressService) {

    this.addressService.addressUpdated.subscribe((added) => {
      if (added === true) {
        this.billingAddress = {};
        this.shippingAddress = {};
        this.addressService.addressItems.listOfAddress.forEach((add) => {
          if (add.billingAddress === true) {
            this.billingAddress = add;
          }
          if (add.shippingAddress === true) {
            this.shippingAddress = add;
          }
        });
      }
    });
  }
}
