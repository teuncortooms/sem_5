import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Group, GroupDetails } from 'src/app/models/group';
import { LoggerService } from 'src/app/services/logger.service';
import { PagedDataSource } from 'src/app/services/data-source/paged-data-source';
import { AssignGroupsService } from 'src/app/services/data-source/assign-groups.service';
import { AssignStudentDialogComponent } from '../assign-student-dialog/assign-student-dialog.component';
import { GroupDetailsDialogComponent } from '../group-details-dialog/group-details-dialog.component';
import { GROUPS_DATA_TOKEN } from 'src/app/services/dependency-injection/di-config';
import { DialogService } from 'src/app/services/dialog.service';
import { EditDialogBase, EditDialogConfiguration } from 'src/app/base-components/edit-dialog-base';

@Component({
  selector: 'app-edit-group-dialog.component',
  templateUrl: './edit-group-dialog.component.html',
  styleUrls: ['./edit-group-dialog.component.scss']
})
export class EditGroupDialogComponent extends EditDialogBase<Group, GroupDetails> {
  constructor(
    @Inject(MAT_DIALOG_DATA) groupDetails: Partial<GroupDetails>,
    @Inject(GROUPS_DATA_TOKEN) dataSource: PagedDataSource<Group, GroupDetails>,
    assignGroupsService: AssignGroupsService,
    dialogService: DialogService,
    logger: LoggerService,
    currentDialog: MatDialogRef<EditGroupDialogComponent>,
  ) {

    const configuration: EditDialogConfiguration = {
      titleAdd: 'Een klas toevoegen',
      titleEdit: 'Een klas bewerken',
      assignChildrenDialog: AssignStudentDialogComponent,
      detailsDialog: GroupDetailsDialogComponent
    }

    super(groupDetails, dataSource, assignGroupsService, dialogService, logger, currentDialog, configuration);
  }

  protected parseDefinitiveModel(): GroupDetails | undefined {
    if (!this.details.name || !this.details.period || !this.details.startDate ||
      !this.details.endDate) {
      this.logger.log("Cannot save group details. Missing properties.")
      return undefined;
    }

    return {
      id: this.details.id!, name: this.details.name,
      period: this.details.period, startDate: this.details.startDate,
      endDate: this.details.endDate, students: this.details.students
    }
  }
}
