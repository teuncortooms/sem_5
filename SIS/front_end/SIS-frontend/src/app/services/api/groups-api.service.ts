import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoggerService } from '../logger.service';
import { Group, GroupDetails } from 'src/app/models/group';
import { environment } from 'src/environments/environment';
import { PagedApiService } from '../api/paged-api.service';


@Injectable({
  providedIn: 'root'
})
export class GroupsApiService extends PagedApiService<Group, GroupDetails> {
  constructor(http: HttpClient, logger: LoggerService) {
    super(http, logger);
  }

  async assignGroupsToStudents(groupIds: string[], studentIds: string[]): Promise<Object> {
    const url: string = environment.assignStudentsToGroupsEndPoint;
    this.logger.log(`Initiating assign: ${url}...`);

    if (!groupIds || !groupIds.length || !studentIds || !studentIds.length)
      throw new Error("Missing IDs.");

    const request: { studentIds: string[], groupIds: string[] } =
    {
      studentIds: studentIds,
      groupIds: groupIds
    }

    try {
      const result = await this.http.patch<string>(url, request, this.httpOptions).toPromise();
      this.logger.log(`Student-Groep koppelingen bijgewerkt, aantal: ${result}`, true);

      return result;
    }
    catch (error) {
      throw this.HandleError(error, 'assignGroupsToStudents');
    }
  }

  async unassignGroupsFromStudents(groupIds: string[], studentIds: string[]): Promise<Object> {
    const url: string = environment.unassignStudentsFromGroupsEndPoint;
    this.logger.log(`Initiating unassign: ${url}...`);

    if (!groupIds || !groupIds.length || !studentIds || !studentIds.length)
      throw new Error("Missing IDs.");

    const request: { studentIds: string[], groupIds: string[] } =
    {
      studentIds: studentIds,
      groupIds: groupIds
    }

    try {
      const result = await this.http.patch<string>(url, request, this.httpOptions).toPromise();
      this.logger.log(`Student-Groep koppelingen bijgewerkt, aantal: ${result}`, true);

      return result;
    }
    catch (error) {
      throw this.HandleError(error, 'assignGroupsToStudents');
    }
  }
}
