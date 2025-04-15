import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AstronautListComponent } from './pages/astronaut-list/astronaut-list.component';

const routes: Routes = [
  {
    path: '',
    children: [
      {
        path: '',
        pathMatch: 'full',
        component: AstronautListComponent,
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AstronautRoutingModule {}
