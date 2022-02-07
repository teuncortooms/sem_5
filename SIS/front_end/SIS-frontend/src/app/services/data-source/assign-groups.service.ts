import { Inject, Injectable } from '@angular/core';
import { GroupsApiService } from '../api/groups-api.service';
import { GROUPS_API_TOKEN, STUDENTS_DATA_TOKEN } from 'src/app/services/dependency-injection/di-config';
import { PagedDataSource } from './paged-data-source';
import { Student, StudentDetails } from 'src/app/models/student';

@Injectable({
  providedIn: 'root'
})
export class AssignGroupsService {

  constructor(
    @Inject(GROUPS_API_TOKEN) private groupsApi: GroupsApiService,
    @Inject(STUDENTS_DATA_TOKEN) private studentsDataSource: PagedDataSource<Student, StudentDetails>
  ) { }

  public async assign(groupIds: string[], studentIds: string[]): Promise<void> {
    if (!groupIds || !groupIds.length || !studentIds || !studentIds.length)
      throw new Error("Missing IDs.");

    await this.groupsApi.assignGroupsToStudents(groupIds, studentIds)
      .catch(() => { throw new Error("Failed to assign groups.") })
      .then(() => {
        this.studentsDataSource.reload();
      });
  }

  public async unassign(groupIds: string[], studentIds: string[]): Promise<void> {
    if (!groupIds || !groupIds.length || !studentIds || !studentIds.length)
      throw new Error("Missing IDs.");

    await this.groupsApi.unassignGroupsFromStudents(groupIds, studentIds)
      .catch(() => { throw new Error("Failed to unassign groups.") })
      .then(() => {
        this.studentsDataSource.reload();
      });
  }
}

