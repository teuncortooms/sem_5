import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Teacher } from 'src/app/models/teacher';
import { PagedDataSource } from 'src/app/services/data-source/paged-data-source';
import { TEACHERS_DATA_TOKEN } from 'src/app/services/dependency-injection/di-config';

@Component({
  selector: 'app-edit-teacher-dialog',
  templateUrl: './edit-teacher-dialog.component.html',
  styleUrls: ['./edit-teacher-dialog.component.scss']
})
export class EditTeacherDialogComponent implements OnInit {
  isNew: boolean = false;
  title: string;
  selectedTeacher: Teacher ;

  constructor(
    public editDialogRef: MatDialogRef<EditTeacherDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public id: string,
    @Inject(TEACHERS_DATA_TOKEN) public dataSource: PagedDataSource<Teacher,Teacher>,
    ) {
      if (!id) {
        this.isNew = true;
      }
      this.title = this.isNew ? "Een medewerker toevoegen" : "Een medewerker bewerken";
      this.selectedTeacher = {id: "", firstName: "", lastName: ""}
      this.editDialogRef.disableClose = true;
    }

  ngOnInit() {
    this.dataSource.getDetails(this.id).then(result => {
      if (result) this.selectedTeacher = result;
    });

  }

  onSaveButtonClick(): void {
    if (!this.selectedTeacher.firstName || !this.selectedTeacher.lastName) {

      return;
    }

    const definitive: Teacher = {
      id: this.selectedTeacher.id!, firstName: this.selectedTeacher.firstName,
      lastName: this.selectedTeacher.lastName
    }
    // address: this.studentDetails.address, housenr: this.studentDetails.housenr, postalcode: this.studentDetails.postalcode,
    // city: this.studentDetails.city, telephone: this.studentDetails.telephone, mobilephone: this.studentDetails.mobilephone,
    // birthdate: this.studentDetails.birthdate, emailAddress: this.studentDetails.emailAddress

    if (this.isNew) this.dataSource.add(definitive);
    else this.dataSource.update(this.selectedTeacher.id!, definitive);
    this.editDialogRef.close();
  }

  onDeleteButtonClick(): void {
    if (!this.selectedTeacher.id) throw new Error("Delete button should not be visible");
    var ids: string[];
    ids = [this.selectedTeacher.id];
    this.dataSource.remove(ids);
    this.editDialogRef.close();
  }

}
