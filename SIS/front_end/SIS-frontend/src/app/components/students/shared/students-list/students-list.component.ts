import { Component } from '@angular/core';
import { ChildrensListBase, ListConfiguration } from 'src/app/base-components/childrens-list-base';
import { Student } from 'src/app/models/student';
import { LoggerService } from 'src/app/services/logger.service';

@Component({
  selector: 'app-students-list',
  templateUrl: './students-list.component.html',
  styleUrls: ['./students-list.component.scss']
})
export class StudentsListComponent extends ChildrensListBase<Student> {
  constructor(logger: LoggerService) {

    const configuration: ListConfiguration = {
      displayedColumns: ['fullName'],
      sorts: [
        (a, b) => a.firstName.localeCompare(b.firstName),
        (a, b) => a.lastName.localeCompare(b.lastName)
      ]
    }
    super(logger, configuration);
   }
}


