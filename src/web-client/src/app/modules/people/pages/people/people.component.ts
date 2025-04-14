import { trigger, state, style, transition, animate } from '@angular/animations';
import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { SnackBarService } from 'src/app/shared/services/snack-bar.service';
import { PersonDataSource } from '../../models/peopleDataSource';
import { Person } from '../../models/person';
import { PeopleService } from '../../services/people.service';

@Component({
  selector: 'app-people',
  templateUrl: './people.component.html',
  styleUrls: ['./people.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed,void', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
  encapsulation: ViewEncapsulation.None,
})
export class PeopleListComponent implements OnInit {
  today: Date = new Date();
  dataSource: PersonDataSource;
  displayedColumns: string[] = ['id', 'name'];
  expandedElement: Person | null;

  constructor(
    protected tasksService: PeopleService,
    private dialog: MatDialog,
    protected snackBarService: SnackBarService,
  ) {}

  ngOnInit(): void {
    this.loadPeople();
  }

  // public addItem(event: MouseEvent) {
  //   event.stopPropagation();
  //   const ref = this.dialog.open(AddTodoComponent);

  //   ref.afterClosed().subscribe((item: Person) => {
  //     if (item) {
  //       this.tasksService.saveTodoItem(item).subscribe({
  //         next: (resp) => {
  //           const items = this.dataSource.getData();
  //           items.push(resp);
  //           this.dataSource.setData(items);
  //           this.snackBarService.success('People created!');
  //         },
  //         error: () => {
  //           this.snackBarService.error('Error creating People.');
  //         },
  //       });
  //     }
  //   });
  // }

  // addSubPeople(task: Person) {
  //   const ref = this.dialog.open(AddTodoComponent);

  //   ref.afterClosed().subscribe((item: Person) => {
  //     if (item) {
  //       if (!task.subPeople) {
  //         task.subPeople = [];
  //       }
  //       task.subPeople.push(item);
  //       this.tasksService.updateTodoItem(task).subscribe({
  //         next: (resp) => {
  //           const items = this.dataSource.getData();
  //           const filteredPeople = items.filter((x) => x.id !== task.id);
  //           filteredPeople.push(resp);
  //           this.dataSource.setData(filteredPeople);
  //           this.snackBarService.success('Successfully added sub task.');
  //         },
  //         error: () => {
  //           this.snackBarService.error('Error updating task.');
  //         },
  //       });
  //     }
  //   });
  // }

  // deletePeople(id: string) {
  //   const ref = this.dialog.open(ConfirmDeleteToDoComponent, { data: id });

  //   ref.afterClosed().subscribe((resp: Person[]) => {
  //     if (resp) {
  //       this.dataSource.setData(resp);
  //       this.snackBarService.success('To Do successfully deleted.');
  //     }
  //   });
  // }

  // public markComplete(id: string) {
  //   this.tasksService.markComplete(id).subscribe({
  //     next: (tasks) => {
  //       this.dataSource.setData(tasks);
  //       this.snackBarService.success('People marked complete');
  //     },
  //     error: () => {
  //       this.snackBarService.error('Error marking task complete.');
  //     },
  //   });
  // }

  // public testfunction(): void {
  //   console.log('Hello');
  //   this.loadDataThen();
  //   return;
  // }

  // loadDataThen(): void {
  //   this.tasksService
  //     .fetchData()
  //     .then((data) => console.log('Data:', data))
  //     .catch((error) => console.error('Error fetching data:', error));
  // }

  private loadPeople() {
    this.tasksService.getPersons().subscribe({
      next: (items) => {
        console.log('Data:', items);
        this.dataSource = new PersonDataSource(items);
      },
      error: () => {
        this.snackBarService.error('Error loading Todo People');
      },
    });
  }
}
