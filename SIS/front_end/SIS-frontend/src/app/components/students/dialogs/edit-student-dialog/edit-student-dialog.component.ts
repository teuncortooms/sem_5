import { Component, Inject } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Student, StudentDetails } from 'src/app/models/student';
import { LoggerService } from 'src/app/services/logger.service';
import { PagedDataSource } from 'src/app/services/data-source/paged-data-source';
import { AssignGroupsService } from 'src/app/services/data-source/assign-groups.service';
import { AssignGroupDialogComponent } from '../assign-group-dialog/assign-group-dialog.component';
import { StudentDetailsDialogComponent } from '../student-details-dialog/student-details-dialog.component';
import { STUDENTS_DATA_TOKEN } from 'src/app/services/dependency-injection/di-config';
import { DialogService } from 'src/app/services/dialog.service';
import { EditDialogBase, EditDialogConfiguration } from 'src/app/base-components/edit-dialog-base';

@Component({
  selector: 'app-edit-student-dialog',
  templateUrl: './edit-student-dialog.component.html',
  styleUrls: ['./edit-student-dialog.component.scss']
})
export class EditStudentDialogComponent extends EditDialogBase<Student, StudentDetails> {
  emailControl!: FormControl;
  phoneControl!: FormControl;
  postalcodeControl!: FormControl;

  constructor(
    @Inject(MAT_DIALOG_DATA) studentDetails: Partial<StudentDetails>,
    @Inject(STUDENTS_DATA_TOKEN) dataSource: PagedDataSource<Student, StudentDetails>,
    assignGroupsService: AssignGroupsService,
    dialogService: DialogService,
    logger: LoggerService,
    currentDialog: MatDialogRef<EditStudentDialogComponent>
  ) {

    const configuration: EditDialogConfiguration = {
      titleAdd: 'Een student toevoegen',
      titleEdit: 'Een student bewerken',
      assignChildrenDialog: AssignGroupDialogComponent,
      detailsDialog: StudentDetailsDialogComponent
    }
    super(studentDetails, dataSource, assignGroupsService, dialogService, logger, currentDialog, configuration);

    this.initFormControls();
  }

  protected parseDefinitiveModel(): StudentDetails | undefined {
    if (!this.details.firstName || !this.details.lastName || !this.details.email) {
      this.logger.log("Cannot save student details. Missing properties.")
      return undefined;
    }

    return {
      id: this.details.id!, firstName: this.details.firstName,
      lastName: this.details.lastName, fullName: this.details.fullName!,
      email: this.details.email!,
      currentGroup: this.details.currentGroup, groups: this.details.groups
    }
  }

  private initFormControls() {
    this.emailControl = new FormControl('', [Validators.required, Validators.email]);
    this.phoneControl = new FormControl('', [Validators.required, Validators.maxLength(10), Validators.minLength(10)]);
    this.postalcodeControl = new FormControl('', [Validators.required, Validators.maxLength(7), Validators.minLength(6)]);
  }

  getErrorMessage(validatortype: string) {
    if (validatortype == "email") {
      return this.emailControl.hasError('required') ? 'Dit veld is verplicht' :
        this.emailControl.hasError('email') ? 'Geen geldig email-adres' :
          '';
    }
    if (validatortype == "phone") {
      return this.phoneControl.hasError('required') ? 'Dit veld is verplicht' :
        this.phoneControl.hasError('minlength') ? 'Een telefoon nummer moet uit 10 nummers bestaan' :
          this.phoneControl.hasError('maxlength') ? 'Een telefoon nummer moet uit 10 nummers bestaan' :
            '';
    }
    if (validatortype == "postalcode") {
      return this.postalcodeControl.hasError('required') ? 'Dit veld is verplicht' :
        this.postalcodeControl.hasError('minlength') ? 'Een postcode moet uit 6 tekens bestaan' :
          this.postalcodeControl.hasError('maxlength') ? 'Een postcode moet uit 6 tekens bestaan' :
            '';
    }
    return '';
  }
}

