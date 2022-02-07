import { Component, Inject } from '@angular/core';
import { Student, StudentDetails } from 'src/app/models/student';
import { StudentDetailsDialogComponent } from '../dialogs/student-details-dialog/student-details-dialog.component';
import { EditStudentDialogComponent } from '../dialogs/edit-student-dialog/edit-student-dialog.component';
import { AssignGroupDialogComponent } from '../dialogs/assign-group-dialog/assign-group-dialog.component';
import { PagedDataSource } from 'src/app/services/data-source/paged-data-source';
import { DialogService } from 'src/app/services/dialog.service';
import { STUDENTS_DATA_TOKEN } from 'src/app/services/dependency-injection/di-config';
import { OverviewBase } from 'src/app/base-components/overview-base';


@Component({
  selector: 'app-students-overview',
  templateUrl: './students-overview.component.html',
  styleUrls: ['./students-overview.component.scss'],
})

export class StudentsOverviewComponent extends OverviewBase<Student, StudentDetails> {

  constructor(
    @Inject(STUDENTS_DATA_TOKEN) dataSource: PagedDataSource<Student, StudentDetails>,
    dialogService: DialogService,
  ) {
    const configuration = {
      detailsComponent: StudentDetailsDialogComponent,
      editComponent: EditStudentDialogComponent
    };
    super(dataSource, dialogService, configuration);
  }

  public openAssignGroupDialog() {
    const students = this.table?.selection.selected;
    this.table?.selection.clear();

    if (students && students.length > 0) {
      const studentIds = students.map(s => s.id);

      this.dialogService.openDialog(AssignGroupDialogComponent, {
        data: studentIds
      });
    }
  }
}