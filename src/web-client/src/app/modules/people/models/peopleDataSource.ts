import { DataSource } from '@angular/cdk/collections';
import { BehaviorSubject, Observable } from 'rxjs';
import { Person } from './person';

export class PersonDataSource extends DataSource<Person> {
  private _dataStream = new BehaviorSubject<Person[]>([]);

  constructor(initialData: Person[]) {
    super();
    this.setData(initialData);
  }

  connect(): Observable<Person[]> {
    return this._dataStream;
  }

  disconnect() {}

  setData(data: Person[]) {
    this._dataStream.next(data);
  }

  getData(): Person[] {
    return this._dataStream.value;
  }
}
