import { Component, Inject, Input, OnInit } from '@angular/core';
import { Student, StudentDetails } from 'src/app/models/student';
import { LoggerService } from 'src/app/services/logger.service';
import { PagedDataSource } from 'src/app/services/data-source/paged-data-source';
import { STUDENTS_DATA_TOKEN } from 'src/app/services/dependency-injection/di-config';

@Component({
  selector: 'app-student-details',
  templateUrl: './student-details.component.html',
  styleUrls: ['./student-details.component.scss']
})
export class StudentDetailsComponent implements OnInit {
  details?: StudentDetails;
  
  constructor(
    @Inject(STUDENTS_DATA_TOKEN) private dataSource: PagedDataSource<Student, StudentDetails>,
    private logger: LoggerService
  ) { }

  ngOnInit(): void {
    this.dataSource.configure("students");
    this.loadDetails();
  }

  loadDetails() {
    this.dataSource.getMyDetails().then((details) => {
      if (!details) this.logger.log(`Could not find details`);
      this.details = details;
    });
  }
}
