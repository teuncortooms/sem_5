import { Component } from '@angular/core';
import { ChildrensListBase, ListConfiguration } from 'src/app/base-components/childrens-list-base';
import { Group } from 'src/app/models/group';
import { LoggerService } from 'src/app/services/logger.service';

@Component({
  selector: 'app-groups-list',
  templateUrl: './groups-list.component.html',
  styleUrls: ['./groups-list.component.scss']
})
export class GroupsListComponent extends ChildrensListBase<Group> {
  constructor(logger: LoggerService) {

    const configuration: ListConfiguration = {
      displayedColumns: ['name', 'dates'],
      sorts: [
        (a, b) => this.getTime(b.endDate) - this.getTime(a.endDate),
        (a, b) => this.getTime(b.startDate) - this.getTime(a.startDate)
      ]
    }
    super(logger, configuration);
  }

  private getTime(date?: Date): number {
    if (!date) return 0;

    const thisDateDoesWorkButWhy = new Date(date);
    return thisDateDoesWorkButWhy.getTime();
  }
}


