import { Component, Inject, ViewChild } from '@angular/core';
import { EditGroupDialogComponent } from '../dialogs/edit-group-dialog/edit-group-dialog.component';
import { GroupDetailsDialogComponent } from '../dialogs/group-details-dialog/group-details-dialog.component';
import { Group, GroupDetails } from 'src/app/models/group';
import { PagedDataSource } from 'src/app/services/data-source/paged-data-source';
import { GROUPS_DATA_TOKEN } from 'src/app/services/dependency-injection/di-config';
import { DialogService } from 'src/app/services/dialog.service';
import { OverviewBase } from 'src/app/base-components/overview-base';

@Component({
  selector: 'app-groups-overview',
  templateUrl: './groups-overview.component.html',
  styleUrls: ['./groups-overview.component.scss']
})
export class GroupsOverviewComponent extends OverviewBase<Group, GroupDetails> {

  constructor(
    @Inject(GROUPS_DATA_TOKEN) dataSource: PagedDataSource<Group, GroupDetails>,
    dialogService: DialogService
  ) {
    const configuration = {
      detailsComponent: GroupDetailsDialogComponent,
      editComponent: EditGroupDialogComponent
    };

    super(dataSource, dialogService, configuration);
  }
}
