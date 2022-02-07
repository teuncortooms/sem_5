import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Group, GroupDetails } from 'src/app/models/group';
import { LoggerService } from 'src/app/services/logger.service';
import { PagedDataSource } from 'src/app/services/data-source/paged-data-source';
import { EditGroupDialogComponent } from '../edit-group-dialog/edit-group-dialog.component';
import { GROUPS_DATA_TOKEN } from 'src/app/services/dependency-injection/di-config';
import { DialogService } from 'src/app/services/dialog.service';
import { DetailsDialogBase } from 'src/app/base-components/details-dialog-base';


@Component({
  selector: 'app-group-details-dialog',
  templateUrl: './group-details-dialog.component.html',
  styleUrls: ['./group-details-dialog.component.scss']
})
export class GroupDetailsDialogComponent extends DetailsDialogBase<Group, GroupDetails> {
  constructor(
    @Inject(MAT_DIALOG_DATA) inputId: string,
    @Inject(GROUPS_DATA_TOKEN) dataSource: PagedDataSource<Group, GroupDetails>,
    dialogService: DialogService, logger: LoggerService, currentDialog: MatDialogRef<GroupDetailsDialogComponent>
  ) {
    const configuration = {
      title: 'Een klas bekijken',
      endpoint: 'groups',
      editComponent: EditGroupDialogComponent
    };
    super(inputId, dataSource, dialogService, logger, currentDialog, configuration);
  }
}
