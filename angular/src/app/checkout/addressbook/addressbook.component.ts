import { Component, AfterViewInit, Output, EventEmitter } from '@angular/core';
import { apiUrl } from '@app/config/configuration';
import { ApiService } from '@services/api.service';
import { AddressService } from '@services/address.service';

@Component({
    selector: 'addressbook',
    templateUrl: './addressbook.component.html'
})
export class AddressBookComponent implements AfterViewInit {
    @Output() public addAddress = new EventEmitter<boolean>();
    public addresses: any;
    public offsetNewAddress: number;
    constructor(private apiService: ApiService, public addressService: AddressService) {
    }
    public addNewAddress() {
        this.addAddress.emit(true);
    }
    public ngAfterViewInit() {
        this.offsetNewAddress = Number(document.getElementById('newAddress').offsetTop) + 60;
    }
}
