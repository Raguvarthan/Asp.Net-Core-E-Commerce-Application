import { Inject, Injectable, Optional, EventEmitter } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { ApiService } from './api.service';
import { AppState } from '@app/app.service';
import * as CartModel from '@models/cart.model';
import 'rxjs/add/operator/map';

@Injectable()
export class AddressService {

    public addressItems: { listOfAddress: any[]} = { listOfAddress: [] };

    public addressUpdated: EventEmitter<boolean> = new EventEmitter();

    constructor(private apiService: ApiService, private appState: AppState) {
    }

    public getAddresses() {
        let userName = localStorage.getItem('UserName');
        this.apiService.get('user/userinfo/' + userName).then(
            (response: any) => {
                this.addressItems.listOfAddress = response.data;
                this.addressUpdated.emit(true);
            },
            (error: any) => {
                console.log(error);
            }
        );
    }
    public refreshAddressList() {
        let userName = localStorage.getItem('UserName');
        if (userName !== undefined) {
            this.apiService.post('user/userinfo/' + userName,
                                 this.addressItems, { useAuth: true }, undefined).then(
                (response: any) => {
                    if (response.code === '200') {
                        this.addressUpdated.emit(true);
                        return true;
                    }
                })
                .catch(
                (error: any) => {
                    if (error.code === '400') {
                        return false;
                    }
                }
                );
        }
    }
}
