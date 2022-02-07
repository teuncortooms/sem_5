import { SelectionModel } from '@angular/cdk/collections';
import { AfterContentInit, AfterViewInit, ChangeDetectorRef, Directive, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, SortDirection } from '@angular/material/sort';
import { merge } from 'rxjs';
import { LoggerService } from 'src/app/services/logger.service';
import { PagedDataSource } from 'src/app/services/data-source/paged-data-source';
import { HasId } from '../interfaces/has-id';

@Directive()
export abstract class TableBase<TModel extends HasId, TDetails extends HasId> implements OnInit, AfterViewInit {
  @Output() rowClickEvent = new EventEmitter<string>();

  @ViewChild(MatSort) sort: MatSort = new MatSort;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  filter: string = '';
  selection = new SelectionModel<TModel>(true, []);

  displayedColumns: string[] = [];

  constructor(
    public dataSource: PagedDataSource<TModel, TDetails>,
    public logger: LoggerService,
    public dialog: MatDialog,
    public changeDetector: ChangeDetectorRef,
    private configuration: { endpoint: string, displayedColumns: string[], sortHeader: string, sortDirection: SortDirection }
  ) {
    this.displayedColumns = configuration.displayedColumns;
    this.sort.active = configuration.sortHeader;
    this.sort.direction = configuration.sortDirection;
  }


  ngOnInit(): void {
    this.dataSource.configure(this.configuration.endpoint);
  }

  ngAfterViewInit() {
    this.paginator.page.subscribe(() => this.loadData());
    this.sort.sortChange.subscribe(() => {
      this.loadData();
      this.paginator.firstPage();
    });
    this.dataSource.countStream.subscribe(count => {
      this.paginator.length = count;
    });

    // loadData needs info from the view but also updates the view. Needs explicit 
    // change detection to ensure all data updates are reflected in the view.
    this.loadData();
    this.changeDetector.detectChanges();
  }

  public loadData() {
    this.dataSource.load(
      this.paginator.pageIndex,
      this.paginator.pageSize,
      this.filter,
      this.sort.active,
      this.sort.direction
    );
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.filter = filterValue.trim().toLowerCase();
    this.loadData();
  }

  public masterToggle() {
    if (this.isAllSelected()) {
      this.selection.clear();
      return;
    }

    this.selection.select(...this.dataSource.currentData);
  }

  public isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.paginator.pageSize < this.paginator.length ?
      this.paginator.pageSize : this.paginator.length;
    return numSelected === numRows;
  }

  public onRowClick(id: string): void {
    this.rowClickEvent.emit(id);
  }
}
