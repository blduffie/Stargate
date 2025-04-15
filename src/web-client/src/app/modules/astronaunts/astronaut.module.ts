import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { AstronautListComponent } from './pages/astronaut-list/astronaut-list.component';
import { AstronautRoutingModule } from './astronaut.routing.module';

@NgModule({
  declarations: [AstronautListComponent],
  exports: [AstronautRoutingModule],
  imports: [SharedModule, AstronautRoutingModule],
})
export class AstronautModule {}
