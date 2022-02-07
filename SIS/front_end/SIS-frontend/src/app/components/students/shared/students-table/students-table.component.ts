import { ChangeDetectorRef, Component, Inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Student, StudentDetails } from 'src/app/models/student';
import { LoggerService } from 'src/app/services/logger.service';
import { PagedDataSource } from 'src/app/services/data-source/paged-data-source';
import { TableBase } from 'src/app/base-components/table-base';
import { STUDENTS_DATA_TOKEN } from 'src/app/services/dependency-injection/di-config';
import { SortDirection } from '@angular/material/sort';

@Component({
  selector: 'app-students-table',
  templateUrl: './students-table.component.html',
  styleUrls: ['./students-table.component.scss']
})
export class StudentsTableComponent extends TableBase<Student, StudentDetails> {

  constructor(@Inject(STUDENTS_DATA_TOKEN) dataSource: PagedDataSource<Student, StudentDetails>,
    logger: LoggerService, dialog: MatDialog, changeDetector: ChangeDetectorRef) {

    const configuration: { endpoint: string, displayedColumns: string[], sortHeader: string, sortDirection: SortDirection } =
    {
      endpoint: 'students',
      displayedColumns: ['Select', 'firstName', 'lastName', 'currentGroup'],
      sortHeader: 'startDate',
      sortDirection: 'desc'
    }
    super(dataSource, logger, dialog, changeDetector, configuration);
  }
}
