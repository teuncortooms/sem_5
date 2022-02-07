import { ChangeDetectorRef, Component, Inject } from '@angular/core';
import { Group, GroupDetails } from 'src/app/models/group';
import { LoggerService } from 'src/app/services/logger.service';
import { TableBase } from 'src/app/base-components/table-base';
import { PagedDataSource } from 'src/app/services/data-source/paged-data-source';
import { MatDialog } from '@angular/material/dialog';
import { GROUPS_DATA_TOKEN } from 'src/app/services/dependency-injection/di-config';
import { SortDirection } from '@angular/material/sort';


@Component({
  selector: 'app-groups-table',
  templateUrl: './groups-table.component.html',
  styleUrls: ['./groups-table.component.scss']
})
export class GroupsTableComponent extends TableBase<Group, GroupDetails> {

  constructor(@Inject(GROUPS_DATA_TOKEN) dataSource: PagedDataSource<Group, GroupDetails>,
    logger: LoggerService, dialog: MatDialog, changeDetector: ChangeDetectorRef) {

    const configuration: { endpoint: string, displayedColumns: string[], sortHeader: string, sortDirection: SortDirection } =
    {
      endpoint: 'groups',
      displayedColumns: ['Select', 'name', 'period', 'startDate', 'endDate'],
      sortHeader: 'startDate',
      sortDirection: 'desc'
    }
    super(dataSource, logger, dialog, changeDetector, configuration);
  }
}
