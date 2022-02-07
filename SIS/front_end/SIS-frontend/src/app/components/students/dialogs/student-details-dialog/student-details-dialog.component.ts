import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Student, StudentDetails } from 'src/app/models/student';
import { LoggerService } from 'src/app/services/logger.service';
import { PagedDataSource } from 'src/app/services/data-source/paged-data-source';
import { EditStudentDialogComponent } from '../edit-student-dialog/edit-student-dialog.component';
import { STUDENTS_DATA_TOKEN } from 'src/app/services/dependency-injection/di-config';
import { DetailsDialogBase } from 'src/app/base-components/details-dialog-base';
import { DialogService } from 'src/app/services/dialog.service';

@Component({
  selector: 'app-student-details-dialog',
  templateUrl: './student-details-dialog.component.html',
  styleUrls: ['./student-details-dialog.component.scss']
})
export class StudentDetailsDialogComponent extends DetailsDialogBase<Student, StudentDetails> {
  constructor(
    @Inject(MAT_DIALOG_DATA) inputId: string,
    @Inject(STUDENTS_DATA_TOKEN) dataSource: PagedDataSource<Student, StudentDetails>,
    dialogService: DialogService, logger: LoggerService, currentDialog: MatDialogRef<StudentDetailsDialogComponent>
  ) {
    const configuration = {
      title: 'Een student bekijken',
      endpoint: 'students',
      editComponent: EditStudentDialogComponent
    };
    super(inputId, dataSource, dialogService, logger, currentDialog, configuration);
  }
}
