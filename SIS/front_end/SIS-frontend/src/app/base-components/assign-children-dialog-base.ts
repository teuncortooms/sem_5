import { Directive, Inject, ViewChild } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AssignGroupsService } from 'src/app/services/data-source/assign-groups.service';
import { GroupsTableComponent } from '../components/groups/shared/groups-table/groups-table.component';
import { StudentsTableComponent } from '../components/students/shared/students-table/students-table.component';
import { HasId } from '../interfaces/has-id';
import { LoggerService } from '../services/logger.service';
import { TableBase } from './table-base';

@Directive()
export abstract class AssignChildrenDialogBase<TModel extends HasId, TDetails extends HasId> {
  @ViewChild('myTable') table?: TableBase<TModel, TDetails>;

  constructor(
    @Inject(MAT_DIALOG_DATA) private parentIds: string[],
    private assignGroupsService: AssignGroupsService,
    private logger: LoggerService,
    private currentDialog: MatDialogRef<any>) {
  }

  public assign(): void {
    const selection = this.table?.selection.selected;

    if (!selection || selection.length <= 0) {
      this.logger.log("Nothing selected!");
      return;
    }

    const selectedIds = selection.map(s => s.id);
    this.assignToParents(selectedIds)
      .then(() => this.currentDialog.close());
  }

  protected async assignToParents(selectedIds: string[]) {
    if (this.table instanceof GroupsTableComponent)
      await this.assignGroupsService.assign(selectedIds, this.parentIds);
    else if (this.table instanceof StudentsTableComponent)
      await this.assignGroupsService.assign(this.parentIds, selectedIds);
    else {
      throw new Error("Type not recognized");
    }
  }
}
