import { SelectionModel } from '@angular/cdk/collections';
import { AfterViewInit, ChangeDetectorRef, Component, Directive, EventEmitter, Inject, Injectable, Input, OnInit, Optional, Output, SkipSelf, ViewChild, ViewContainerRef } from '@angular/core';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, SortDirection } from '@angular/material/sort';
import { HasId } from 'src/app/interfaces/has-id';
import { PagedDataSource } from 'src/app/services/data-source/paged-data-source';
import { LoggerService } from 'src/app/services/logger.service';
import { ModelListParameters } from '../select-model-list/select-model-list.component';

export interface ModelTableColumn<T,VT> {
  displayName: string,
  callbackStringValue: (model: T) => string | undefined,
  callbackEditorValue?: (model: T) => VT,
  callbackSaveValue?: (model: T, newValue: VT) => void,
  lookupTableConfig?: ModelListParameters<HasId,HasId>,
  onlyNewItems?: boolean,
  secureField?: boolean,
  editType?: string
}


export interface ModelTableParameters<T> {
  entityDisplayNameSingular: string,
  entityDisplayNamePlural: string,
  sortHeader: string,
  sortDirection: SortDirection,
  endpoint: string,
  deleteClaim?: string,
  editClaim?: string,
  createClaim?: string,
  displayedColumns: Map<string, ModelTableColumn<T,any>>,
  detailDialogColumns: Map<string, ModelTableColumn<T,any>>
}

@Injectable({
  providedIn: 'root'
})
export class GenericTableConfig
{
  private settings!: ModelTableParameters<any>;

  register<M extends HasId>(config: ModelTableParameters<M>) : void {
    this.settings = config;
    console.log("Registered generic table config for: "+ config.endpoint);
  }

  public retrieve<M extends HasId>() : ModelTableParameters<M> {
    return this.settings;
  }

}

@Component({
  selector: 'app-generic-table',
  templateUrl: './generic-table-service.component.html',
  styleUrls: ['./generic-table-service.component.scss']
})
export class GenericTableServiceComponent<M extends HasId,D extends HasId> implements OnInit, AfterViewInit {
  public parameters: ModelTableParameters<M>;
  private configuration: { endpoint: string, displayedColumns: string[], sortHeader: string, sortDirection: SortDirection }

  @Output() rowClickEvent = new EventEmitter<string>();

  @ViewChild(MatSort) sort: MatSort = new MatSort;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  filter: string = '';
  selection = new SelectionModel<M>(true, []);

  displayedColumns: string[] = [];

  @Input() dataSource!: PagedDataSource<M, D>;

  constructor(parameters: GenericTableConfig,

  public logger: LoggerService,
  public dialog: MatDialog,
  public changeDetector: ChangeDetectorRef)
  {
    var columns :string[] = ['Select'];
    parameters.retrieve<M>().displayedColumns.forEach( (v,k)=> columns.push(k) );

    logger.log("selecting columns: "+ columns.toString())
    const configuration: { endpoint: string, displayedColumns: string[], sortHeader: string, sortDirection: SortDirection } =
    {
      endpoint: parameters.retrieve<M>().endpoint,
      displayedColumns: columns,
      sortHeader: parameters.retrieve<M>().sortHeader,
      sortDirection: parameters.retrieve<M>().sortDirection
    }

    this.configuration = configuration;
    this.parameters = parameters.retrieve<M>();

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
