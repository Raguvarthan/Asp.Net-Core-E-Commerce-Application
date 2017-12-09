import { AngularModules,
         ComponentsBarrel,
         ProvidersBarrel } from './app.barrel';
import { ENV_PROVIDERS } from './environment';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule, ApplicationRef } from '@angular/core';
import { removeNgStyles, createNewHosts,
         createInputTransfer } from '@angularclass/hmr';

import { AppComponent } from '@app/app.component';
import { NoContentComponent } from '@app/no-content';
import { AuthInterceptor } from '@shared/auth.interceptor';
import { FilterPipe } from '../pipes/filterpipe.component';

import { APP_RESOLVER_PROVIDERS } from '@app/app.resolver';
import { AppState, InternalStateType } from './app.service';

import '../styles/styles.scss';
import '../styles/headings.css';

// Application wide providers
const APP_PROVIDERS = [
  ...APP_RESOLVER_PROVIDERS,
  AppState
];


type StoreType = {
  state: InternalStateType,
  restoreInputValues: () => void,
  disposeOldHosts: () => void
};

/**
 * `AppModule` is the main entry point into Angular2's bootstraping process
 */
@NgModule({
  bootstrap: [AppComponent],
  declarations: [
    AppComponent,
    ComponentsBarrel,
    NoContentComponent,
    FilterPipe
  ],
  imports: [
    AngularModules,
  ],
  providers: [
    ENV_PROVIDERS,
    APP_PROVIDERS,
    ProvidersBarrel,
    {provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true}
  ]
})
export class AppModule {
  constructor(
    public appRef: ApplicationRef,
    public appState: AppState
  ) { }

  public hmrOnInit(store: StoreType) {
    if (!store || !store.state) {
      return;
    }
    console.log('HMR store', JSON.stringify(store, null, 2));
    /**
     * Set state
     */
    this.appState._state = store.state;
    /**
     * Set input values
     */
    if ('restoreInputValues' in store) {
      let restoreInputValues = store.restoreInputValues;
      setTimeout(restoreInputValues);
    }

    this.appRef.tick();
    delete store.state;
    delete store.restoreInputValues;
  }

  public hmrOnDestroy(store: StoreType) {
    const cmpLocation = this.appRef.components.map((cmp) => cmp.location.nativeElement);
    /**
     * Save state
     */
    const state = this.appState._state;
    store.state = state;
    /**
     * Recreate root elements
     */
    store.disposeOldHosts = createNewHosts(cmpLocation);
    /**
     * Save input values
     */
    store.restoreInputValues = createInputTransfer();
    /**
     * Remove styles
     */
    removeNgStyles();
  }
  public hmrAfterDestroy(store: StoreType) {
    /**
     * Display new elements
     */
    store.disposeOldHosts();
    delete store.disposeOldHosts;
  }
}
