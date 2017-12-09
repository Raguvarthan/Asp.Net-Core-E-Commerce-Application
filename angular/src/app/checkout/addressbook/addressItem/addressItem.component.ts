import { Component, Input } from '@angular/core';
import { AddressService } from '@services/address.service';
import { ToastMsgService } from '@services/toastmsg.service';

@Component({
  selector: 'addressitem',
  templateUrl: './addressitem.component.html'
})
export class AddressItemComponent {

  @Input() public address: any;
  @Input() public itemIndex: number;

  constructor(private addressService: AddressService, private toastmsg: ToastMsgService) {
  }

  public removeItem() {
    this.addressService.addressItems.listOfAddress.splice(this.itemIndex, 1);
    this.addressService.refreshAddressList();
  }

  public setBillingAddress() {
    this.addressService.addressItems.listOfAddress.forEach((add) => {
      add.billingAddress = false;
    });
    this.addressService.addressItems.listOfAddress[this.itemIndex].billingAddress = true;
    this.addressService.refreshAddressList();
    this.toastmsg.popToast('success', 'Success', 'Primary Address Updated');
  }
  public setDeliveryAddress() {
    this.addressService.addressItems.listOfAddress.forEach((add) => {
      add.shippingAddress = false;
    });
    this.addressService.addressItems.listOfAddress[this.itemIndex].shippingAddress = true;
    this.addressService.refreshAddressList();
    this.toastmsg.popToast('success', 'Success', 'Primary Address Updated');
  }
}
