import { Directive, ViewChild } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { LoggerService } from 'src/app/services/logger.service';
import { PagedDataSource } from 'src/app/services/data-source/paged-data-source';
import { AssignGroupsService } from 'src/app/services/data-source/assign-groups.service';
import { DialogService } from 'src/app/services/dialog.service';
import { ChildrensListBase } from './childrens-list-base';
import { HasId } from '../interfaces/has-id';
import { ComponentType } from '@angular/cdk/portal';
import { GroupsListComponent } from '../components/groups/shared/groups-list/groups-list.component';
import { StudentsListComponent } from '../components/students/shared/students-list/students-list.component';

export interface EditDialogConfiguration {
  titleAdd: string,
  titleEdit: string,
  assignChildrenDialog: ComponentType<unknown>,
  detailsDialog: ComponentType<unknown>
}

@Directive()
export abstract class EditDialogBase<TModel extends HasId, TDetails extends HasId> {
  @ViewChild('myList') childList?: ChildrensListBase<TModel>;
  title: string;
  isNew: boolean = false;

  constructor(
    public details: Partial<TDetails>,
    private dataSource: PagedDataSource<TModel, TDetails>,
    private assignGroupsService: AssignGroupsService,
    private dialogService: DialogService,
    protected logger: LoggerService,
    private currentDialog: MatDialogRef<any>,
    private configuration: EditDialogConfiguration
  ) {

    if (!details || !details.id) {
      this.isNew = true;
      this.details = {};
    }
    this.title = this.isNew ? this.configuration.titleAdd : this.configuration.titleEdit;
    this.currentDialog.disableClose = true;
  }

  onSaveButtonClick(): void {
    const definitive = this.parseDefinitiveModel();
    if (!definitive) {
      this.logger.log("Cannot define model.");
      return;
    }

    if (this.isNew) this.dataSource.add(definitive);
    else this.dataSource.update(this.details.id!, definitive);
    this.currentDialog.close();
  }

  protected abstract parseDefinitiveModel(): TDetails | undefined;

  onDeleteButtonClick(): void {
    if (!this.details.id) throw new Error("Delete button should not be visible");
    this.dataSource.remove([this.details.id]);
    this.currentDialog.close();
  }

  onAssignClick(): void {
    const assignDialog = this.dialogService.openDialog(this.configuration.assignChildrenDialog, {
      data: [this.details.id],
      closeDialogRef: this.currentDialog,
      doNext: () => this.openDetailsAndCloseCurrent()
    });
  }

  private openDetailsAndCloseCurrent() {
    this.dialogService.openDialog(this.configuration.detailsDialog, {
      data: this.details.id,
      closeDialogRef: this.currentDialog
    });
  }

  onUnassignClick(): void {
    const selectedIds = this.childList?.selection.selected.map(s => s.id);

    if (!selectedIds || selectedIds.length <= 0) {
      this.logger.log("Nothing selected!");
      return;
    }

    this.unassignFromParent(selectedIds)
      .then(() => this.openDetailsAndCloseCurrent());
  }
  
  protected async unassignFromParent(selectedIds: string[]) {
    if (!this.details.id) throw new Error("No parent id available");
    
    if (this.childList instanceof GroupsListComponent)
      await this.assignGroupsService.unassign(selectedIds, [this.details.id]);
    else if (this.childList instanceof StudentsListComponent)
      await this.assignGroupsService.unassign([this.details.id], selectedIds);
    else {
      throw new Error("Type not recognized");
    }
  }

}
