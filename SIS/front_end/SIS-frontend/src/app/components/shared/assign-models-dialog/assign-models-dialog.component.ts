import { SelectionModel } from '@angular/cdk/collections';
import { AfterViewInit, ChangeDetectorRef, Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { SortDirection } from '@angular/material/sort';
import { MatTable } from '@angular/material/table';
import { base64UrlEncode } from 'angular-auth-oidc-client/lib/validation/jwt-window-crypto.service';
import { AssignChildrenDialogBase } from 'src/app/base-components/assign-children-dialog-base';
import { TableBase } from 'src/app/base-components/table-base';
import { HasId } from 'src/app/interfaces/has-id';
import { PagedDataSource } from 'src/app/services/data-source/paged-data-source';
import { LoggerService } from 'src/app/services/logger.service';
import { ModelListParameters } from '../select-model-list/select-model-list.component';

@Component({
  selector: 'app-assign-models-dialog',
  templateUrl: './assign-models-dialog.component.html',
  styleUrls: ['./assign-models-dialog.component.scss']
})
export class AssignModelsDialogComponent<M extends HasId> extends TableBase<M,M> implements AfterViewInit {
  @ViewChild('myTable') table?: MatTable<M>;
  public config:ModelListParameters<M,M>;
  private currentDialog: MatDialogRef<any>;
  public data:ModelListParameters<M,M>;
  private selected: M[];
  private onSave: (model: M[]) => void;

  constructor(
    @Inject(MAT_DIALOG_DATA) data: { selected: M[], onSave: (model: M[]) => void , config:ModelListParameters<M,M>},
    logger: LoggerService,
    currentDialog: MatDialogRef<any>,
    dialog: MatDialog,
    changeDetector: ChangeDetectorRef
  )
  {

    var columns :string[] = ['Select'];
    data.config.displayedColumns.forEach( (v,k)=> columns.push(k) );

    const configuration: { endpoint: string, displayedColumns: string[], sortHeader: string, sortDirection: SortDirection } =
    {
      endpoint: data.config.endpoint,
      displayedColumns: columns,
      sortHeader: data.config.sortHeader,
      sortDirection: data.config.sortDirection
    }

    super(data.config.dataSource,logger,dialog,changeDetector,configuration);
    this.currentDialog = currentDialog;
    this.config = data.config;
    this.data = data.config;
    this.selected = data.selected;
    this.onSave = data.onSave;
    //this.selection = new SelectionModel<M>(true, data.selected);


  }

  ngAfterViewInit():void {

    super.ngAfterViewInit()
    this.selection.clear()
    this.selection.select(...this.selected);
    //this.table?.selection.select(...this.selected);
    this.changeDetector.detectChanges();
  }

  public Toggle(row:M)
  {
    if(this.IsSelected(row))
      {
        for(let item of this.selection.selected)
        {
          if(item.id == row.id)
          {
            this.selection.deselect(item);
            return;
          }

        }
      }
      else this.selection.select(row);
  }

  public IsSelected(row:M):boolean
  {
    var selected:M[] = this.selection.selected;
    for(let element of selected)
    {
      if(element.id == row.id)
        return true;
    }

    return false;
  }

  public assign(): void {
    const selection = this.selection.selected;

    if (!selection || selection.length <= 0) {
      this.logger.log("Nothing selected!");
      //return;
    }

    this.onSave(selection);
    this.currentDialog.close();
  }

  protected async assignToParents(selectedIds: string[]) {
    /*
    if (this.table instanceof GroupsTableComponent)
      await this.assignGroupsService.assign(selectedIds, this.parentIds);
    else if (this.table instanceof StudentsTableComponent)
      await this.assignGroupsService.assign(this.parentIds, selectedIds);
    else {
      throw new Error("Type not recognized");
    }
    */
  }


}
