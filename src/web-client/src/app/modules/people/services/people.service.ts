import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, finalize } from 'rxjs';
import { environments, env } from 'src/config/env.config';
import { Person } from '../models/person';

@Injectable({
  providedIn: 'root',
})
export class PeopleService {
  public apiUrl: string = environments[env.env].apiUrl;

  public isLoadingTaskItems$ = new BehaviorSubject(false);
  public isUpdatingTaskItems$ = new BehaviorSubject(false);
  public isDeletingTaskItems$ = new BehaviorSubject(false);
  public isSavingTaskItems$ = new BehaviorSubject(false);

  constructor(protected http: HttpClient) {}

  public getPersons(): Observable<Person[]> {
    this.isLoadingTaskItems$.next(true);

    return this.http
      .get<any>(`${this.apiUrl}/person`)
      .pipe(finalize(() => this.isLoadingTaskItems$.next(false)));
  }

  public savePerson(item: Person): Observable<Person> {
    this.isSavingTaskItems$.next(true);

    return this.http
      .post<Person>(`${this.apiUrl}/Persons`, item)
      .pipe(finalize(() => this.isSavingTaskItems$.next(false)));
  }

  public updatePerson(item: Person): Observable<Person> {
    this.isUpdatingTaskItems$.next(true);

    return this.http
      .put<Person>(`${this.apiUrl}/Persons`, item)
      .pipe(finalize(() => this.isUpdatingTaskItems$.next(false)));
  }
}
