import { trigger, state, style, transition, animate } from '@angular/animations';
import { Component, ViewEncapsulation, OnInit, OnDestroy } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { SnackBarService } from 'src/app/shared/services/snack-bar.service';
import { AstronautService } from '../../services/astronaut.service';
import { Subject, takeUntil } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-astronaut-list',
  templateUrl: './astronaut-list.component.html',
  styleUrls: ['./astronaut-list.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed,void', style({ height: '0px', minHeight: '0' })),
      state('expanded', style({ height: '*' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
  encapsulation: ViewEncapsulation.None,
})
export class AstronautListComponent implements OnInit, OnDestroy {
  today: Date = new Date();
  displayedColumns: string[] = [
    'id',
    'name',
    'rank',
    'currentDutyTitle',
    'careerStartDate',
    'careerEndDate',
    'duties',
  ];

  private _unsubscribe$ = new Subject<void>();

  constructor(
    protected astronautsService: AstronautService,
    private dialog: MatDialog,
    protected snackBarService: SnackBarService,
  ) {}

  ngOnInit(): void {
    this.loadAstronauts();
  }

  ngOnDestroy(): void {
    this._unsubscribe$.next();
    this._unsubscribe$.complete();
  }

  // public addItem(event: MouseEvent) {
  //   event.stopPropagation();
  //   const ref = this.dialog.open(AddTodoComponent);

  //   ref.afterClosed().subscribe((item: Person) => {
  //     if (item) {
  //       this.astronautsService.saveTodoItem(item).subscribe({
  //         next: (resp) => {
  //           const items = this.dataSource.getData();
  //           items.push(resp);
  //           this.dataSource.setData(items);
  //           this.snackBarService.success('Astronaut created!');
  //         },
  //         error: () => {
  //           this.snackBarService.error('Error creating Astronaut.');
  //         },
  //       });
  //     }
  //   });
  // }

  // addSubAstronaut(task: Person) {
  //   const ref = this.dialog.open(AddTodoComponent);

  //   ref.afterClosed().subscribe((item: Person) => {
  //     if (item) {
  //       if (!task.subAstronaut) {
  //         task.subAstronaut = [];
  //       }
  //       task.subAstronaut.push(item);
  //       this.astronautsService.updateTodoItem(task).subscribe({
  //         next: (resp) => {
  //           const items = this.dataSource.getData();
  //           const filteredAstronaut = items.filter((x) => x.id !== task.id);
  //           filteredAstronaut.push(resp);
  //           this.dataSource.setData(filteredAstronaut);
  //           this.snackBarService.success('Successfully added sub task.');
  //         },
  //         error: () => {
  //           this.snackBarService.error('Error updating task.');
  //         },
  //       });
  //     }
  //   });
  // }

  // deleteAstronaut(id: string) {
  //   const ref = this.dialog.open(ConfirmDeleteToDoComponent, { data: id });

  //   ref.afterClosed().subscribe((resp: Person[]) => {
  //     if (resp) {
  //       this.dataSource.setData(resp);
  //       this.snackBarService.success('To Do successfully deleted.');
  //     }
  //   });
  // }

  // public markComplete(id: string) {
  //   this.astronautsService.markComplete(id).subscribe({
  //     next: (astronauts) => {
  //       this.dataSource.setData(astronauts);
  //       this.snackBarService.success('Astronaut marked complete');
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
  //   this.astronautsService
  //     .fetchData()
  //     .then((data) => console.log('Data:', data))
  //     .catch((error) => console.error('Error fetching data:', error));
  // }

  private loadAstronauts() {
    this.astronautsService
      .getAstronauts()
      .pipe(takeUntil(this._unsubscribe$))
      .subscribe({
        next: (resp) => {
          this.astronautsService.allAstronauts$.next(resp);
        },
        error: (err: HttpErrorResponse) => {
          this.astronautsService.allAstronauts$.next([]);
        },
      });
  }
}
