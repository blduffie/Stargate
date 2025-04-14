import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { PeopleListComponent } from './pages/people/people.component';
import { PeopleRoutingModule } from './people.routing.module';

@NgModule({
  declarations: [PeopleListComponent],
  exports: [PeopleRoutingModule],
  imports: [SharedModule, PeopleRoutingModule],
})
export class PeopleModule {}
