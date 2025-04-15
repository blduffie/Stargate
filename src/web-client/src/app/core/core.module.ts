import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule, Optional, SkipSelf } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HeaderComponent } from './components/header/header.component';
import { LandingComponent } from './components/landing/landing.component';
import { ResponseLoggingInterceptor } from 'libs/shared/src/lib/interceptors/response-logging.interceptor';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SharedModule } from '@dd-lib/shared';

@NgModule({
  declarations: [HeaderComponent, LandingComponent],
  exports: [HeaderComponent, LandingComponent],
  imports: [BrowserAnimationsModule, BrowserModule, HttpClientModule, SharedModule],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ResponseLoggingInterceptor,
      multi: true,
    },
  ],
})
export class CoreModule {
  constructor(@Optional() @SkipSelf() core: CoreModule) {
    if (core) {
      throw new Error('The CoreModule should only be imported once in the AppModule');
    }
  }
}
