import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppPreloadStrategy } from './appPreLoadStrategy';

const routes: Routes = [
  {
    path: '',
    children: [
      {
        path: '',
        redirectTo: 'people',
        pathMatch: 'full',
      },
      {
        path: 'people',
        loadChildren: () => import('./modules/people/people.module').then((m) => m.PeopleModule),
      },
      {
        path: 'astronauts',
        loadChildren: () =>
          import('./modules/astronaunts/astronaut.module').then((m) => m.AstronautModule),
      },
    ],
  },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      preloadingStrategy: AppPreloadStrategy,
      onSameUrlNavigation: 'reload',
    }),
  ],
  exports: [RouterModule],
})
export class AppRoutingModule {}
