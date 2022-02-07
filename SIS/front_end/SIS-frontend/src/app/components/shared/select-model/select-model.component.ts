import { SelectionModel } from '@angular/cdk/collections';
import { Component, EventEmitter, Inject, Input, OnInit, Output } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ModelTableColumn } from 'src/app/components/shared/generic-table/generic-table-service.component';
import { HasId } from 'src/app/interfaces/has-id';
import { Student } from 'src/app/models/student';
import { DialogService } from 'src/app/services/dialog.service';
import { LoggerService } from 'src/app/services/logger.service';
import { AssignModelsDialogComponent } from '../assign-models-dialog/assign-models-dialog.component';
import { GenericEditDialogComponent } from '../generic-edit-dialog/generic-edit-dialog.component';
import { ModelListParameters, SelectModelListComponent } from '../select-model-list/select-model-list.component';
import { SelectModelsListComponent } from '../select-models-list/select-models-list.component';

@Component({
  selector: 'app-select-model',
  templateUrl: './select-model.component.html',
  styleUrls: ['./select-model.component.scss']
})

export class SelectModelComponent<M extends HasId, D extends HasId> {
  @Input() model?: M;
  @Input() parameters!: ModelListParameters<M,D>;
  @Input() columnConfig?: ModelTableColumn<M, D>;
  private callback : EventEmitter<M> = new EventEmitter<M>();

  public selectedItemsEvent : EventEmitter<SelectionModel<M>> = new EventEmitter<SelectionModel<M>>();;
  public selectedItems? : SelectionModel<M>;
  //private

  constructor(
    public dialogService: DialogService,
    public logger: LoggerService,
    private currentDialog: MatDialogRef<SelectModelComponent<M,D>>
    )
    {
      this.callback.subscribe((obj)=>
      {
        this.model = obj;
        logger.log("received selection event");
        if(this.columnConfig != undefined)
          this.columnConfig!.callbackSaveValue!(this.parameters.model!,obj as unknown as D)

      });
      this.selectedItemsEvent.subscribe(s=>this.selectedItems = s);
    }


  onOpenSelect(): void {
    this.dialogService.openDialog(SelectModelListComponent,  { data: this.parameters });
  }

  onAssignClick(): void {

    const assignDialog = this.dialogService.openDialog(AssignModelsDialogComponent, {
      data: {selected: this.parameters.models, onSave:(m: D)=>this.columnConfig?.callbackSaveValue!(this.model!,m), config: this.parameters},
      //closeDialogRef: this.currentDialog,
      doNext: () => this.openDetailsAndCloseCurrent()
    });

  }

  onUnassignClick(): void {
    const selectedIds = this.selectedItems?.selected.map(s => this.parameters.models?.indexOf(s));

    selectedIds?.forEach(i=>this.parameters.models?.splice(i!, 1));
  }

  private openDetailsAndCloseCurrent() {
    //this.dialogService.openDialog(this.currentDialog);
    /*
    this.dialogService.openDialog(GenericEditDialogComponent, {
      data: {
        input:this.model,
        dataSource:this.columnConfig,
        titleEdit: string,
        titleAdd: string,
        detailsDialog: ComponentType<unknown>
      },
      closeDialogRef: this.currentDialog
    });
    */
  }

}
