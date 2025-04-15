import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, finalize } from 'rxjs';
import { environments, env } from 'src/config/env.config';
import { Astronaut } from '../models/astronaut';

@Injectable({
  providedIn: 'root',
})
export class AstronautService {
  public apiUrl: string = environments[env.env].apiUrl;

  public isLoadingAstronauts$ = new BehaviorSubject(false);
  public isUpdatingAstronauts$ = new BehaviorSubject(false);
  public isDeletingAstronauts$ = new BehaviorSubject(false);
  public isSavingAstronauts$ = new BehaviorSubject(false);

  public allAstronauts$: BehaviorSubject<Astronaut[]> = new BehaviorSubject<Astronaut[]>([]);

  constructor(protected http: HttpClient) {}

  getAstronauts(): Observable<Astronaut[]> {
    this.isLoadingAstronauts$.next(true);

    return this.http
      .get<Astronaut[]>(`${this.apiUrl}/astronaut`)
      .pipe(finalize(() => this.isLoadingAstronauts$.next(false)));
  }
}
