import { Inject, Injectable } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ToasterContainerComponent, ToasterModule,
         Toast, ToasterService, ToasterConfig } from 'angular2-toaster';

@Injectable()
export class ToastMsgService {

public config: ToasterConfig = new ToasterConfig({
    positionClass: 'toast-top-right'
  });

constructor(private toasterService: ToasterService) {
}

public popToast(toasttype: string, toasttitle: string, toastmsg: string ) {
  let toast: Toast = {
      type: toasttype,
      title: toasttitle,
      body: toastmsg
    };
  this.toasterService.pop(toast);
  }
}
