import { AfterViewInit, ChangeDetectorRef, Component, EventEmitter, Inject, InjectionToken, Input, OnInit, Output } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { SortDirection } from '@angular/material/sort';
import { TableBase } from 'src/app/base-components/table-base';
import { HasId } from 'src/app/interfaces/has-id';
import { Student, StudentDetails } from 'src/app/models/student';
import { PagedDataSource } from 'src/app/services/data-source/paged-data-source';
import { GENERICSUB_DATA_TOKEN, GENERIC_DATA_TOKEN } from 'src/app/services/dependency-injection/di-config';
import { LoggerService } from 'src/app/services/logger.service';


export interface ModelListColumn<T> {
  displayName: string,
  callbackStringValue: (model: T) => string | undefined
}

export interface ModelListParameters<T extends HasId,D extends HasId> {
  editboxValueDisplayCallback: () => string | undefined,
  editboxLabel: string
  sortHeader: string,
  dataSource: PagedDataSource<T, D>,
  sortDirection: SortDirection,
  endpoint: string,
  model?: T,
  models?: T[],
  onSelect?: (model: T) => void,
  onSelects?: (model: T, values:any[]) => void,
  displayedColumns: Map<string, ModelListColumn<T>>
}

@Component({
  selector: 'app-select-model-list',
  templateUrl: './select-model-list.component.html',
  styleUrls: ['./select-model-list.component.scss']
})
export class SelectModelListComponent<M extends HasId,D extends HasId> extends TableBase<M,D>  {
  public data: ModelListParameters<M,D>;
  public student?: M;
  private currentDialog: MatDialogRef<any>;
  public dataSource: PagedDataSource<M, D>;

  constructor(
    @Inject(MAT_DIALOG_DATA) data: ModelListParameters<M,D>,
    currentDialog: MatDialogRef<any>,
    //@Inject(GENERIC_DATA_TOKEN) dataSource: PagedDataSource<M, D>,
  logger: LoggerService, dialog: MatDialog, changeDetector: ChangeDetectorRef) {

    var columns :string[] = ['Select'];
    data.displayedColumns.forEach( (v,k)=> columns.push(k) );

    logger.log("selecting columns: "+ columns.toString())
    const configuration: { endpoint: string, displayedColumns: string[], sortHeader: string, sortDirection: SortDirection } =
    {
      endpoint: data.endpoint,
      displayedColumns: columns,
      sortHeader: data.sortHeader,
      sortDirection: data.sortDirection
    }

    super(data.dataSource, logger, dialog, changeDetector, configuration);
    this.dataSource = data.dataSource;

    this.data = data;
    this.student = data.model;
    this.currentDialog = currentDialog;
  }

  public onSelection(student: M): void {
    this.student = student;
    if(this.data.onSelect != null)
    {
      this.data.onSelect(student);
      this.logger.log("selection event sent");
      this.currentDialog.close();
    } else {
      this.logger.log("unable to send selection event");
    }
  }

  public isSelected(student: M): boolean {
    return this.student?.id == student.id;
  }

}
