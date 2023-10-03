import {
  NgModule,
  Optional,
  SkipSelf,
  CUSTOM_ELEMENTS_SCHEMA
} from '@angular/core';
import { CommonModule, JsonPipe } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { PreloadModulesStrategy } from './strategies/preload-modules.strategy';
import { AuthService } from './services/auth.service';
import { EnsureModuleLoadedOnceGuard } from './ensureModuleLoadedOnceGuard';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { StorageService } from './services/storage.service';
import { EventBusService } from './services/event-bus.service';
import { OverlayModule } from './overlay/overlay.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';


@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    HttpClientModule,
    OverlayModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({
      timeOut: 10000,
      positionClass: 'toast-bottom-right',
      preventDuplicates: true,
    }),
  ],
  exports: [
    RouterModule,
    HttpClientModule,
    OverlayModule
  ],
  providers: [
    JsonPipe,
    AuthService,
    StorageService,
    EventBusService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
    PreloadModulesStrategy
  ], // these should be singleton
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class CoreModule extends EnsureModuleLoadedOnceGuard {
  // Ensure that CoreModule is only loaded into AppModule

  // Looks for the module in the parent injector to see if it's already been loaded (only want it loaded once)
  constructor(
    @Optional()
    @SkipSelf()
    parentModule: CoreModule
  ) {
    super(parentModule);
  }
}
