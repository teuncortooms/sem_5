import { Component } from '@angular/core';
import { AssignChildrenDialogBase } from 'src/app/base-components/assign-children-dialog-base';
import { Student, StudentDetails } from 'src/app/models/student';

@Component({
  selector: 'app-assign-student-dialog',
  templateUrl: './assign-student-dialog.component.html',
  styleUrls: ['./assign-student-dialog.component.scss']
})
export class AssignStudentDialogComponent extends AssignChildrenDialogBase<Student, StudentDetails> {

}
