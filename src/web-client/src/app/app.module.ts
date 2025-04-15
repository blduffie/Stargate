import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { SharedModule } from './shared/shared.module';
import { AppPreloadStrategy } from './appPreLoadStrategy';
import { CoreModule } from './core/core.module';

@NgModule({
  declarations: [AppComponent],
  imports: [AppRoutingModule, SharedModule.forRoot(), CoreModule],
  providers: [provideAnimationsAsync(), AppPreloadStrategy],
  bootstrap: [AppComponent],
})
export class AppModule {}
