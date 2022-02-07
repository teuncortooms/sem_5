import { HttpClient } from "@angular/common/http";
import { InjectionToken } from "@angular/core";
import { Teacher } from "src/app/models/teacher";
import { User } from "src/app/models/user";
import { HasId } from "../../interfaces/has-id";
import { Grade } from "../../models/grade";
import { Group, GroupDetails } from "../../models/group";
import { Student, StudentDetails } from "../../models/student";
import { GroupsApiService } from "../api/groups-api.service";
import { PagedApiService } from "../api/paged-api.service";
import { PagedDataSource } from "../data-source/paged-data-source";
import { LoggerService } from "../logger.service";


export const GROUPS_DATA_TOKEN = new InjectionToken<PagedDataSource<Group, GroupDetails>>("GROUPS_DATA_TOKEN");
export const STUDENTS_DATA_TOKEN = new InjectionToken<PagedDataSource<Student, StudentDetails>>("STUDENTS_DATA_TOKEN");
export const GRADES_DATA_TOKEN = new InjectionToken<PagedDataSource<Grade, Grade>>("GRADES_DATA_TOKEN");
export const TEACHERS_DATA_TOKEN = new InjectionToken<PagedDataSource<Teacher, Teacher>>("TEACHERS_DATA_TOKEN");
export const GENERIC_DATA_TOKEN = new InjectionToken<PagedDataSource<HasId, HasId>>("GENERIC_DATA_TOKEN");
export const GENERICSUB_DATA_TOKEN = new InjectionToken<PagedDataSource<HasId, HasId>>("GENERICSUB_DATA_TOKEN");
export const USER_DATA_TOKEN = new InjectionToken<PagedDataSource<User, User>>("USER_DATA_TOKEN");

export function dataSourceFactory<TModel extends HasId, TDetails extends HasId>(apiService: PagedApiService<TModel, TDetails>, logger: LoggerService) {
    return new PagedDataSource<TModel, TDetails>(apiService, logger);
}

export const STUDENTS_API_TOKEN = new InjectionToken<PagedApiService<Student, StudentDetails>>("STUDENTS_API_TOKEN");
export const GRADES_API_TOKEN = new InjectionToken<PagedApiService<Grade, Grade>>("GRADES_API_TOKEN");
export const TEACHERS_API_TOKEN = new InjectionToken<PagedApiService<Teacher, Teacher>>("TEACHERS_API_TOKEN");
export const GENERIC_API_TOKEN = new InjectionToken<PagedApiService<HasId, HasId>>("GENERIC_API_TOKEN");
export const GENERICSUB_API_TOKEN = new InjectionToken<PagedApiService<HasId, HasId>>("GENERICSUB_API_TOKEN");
export const USER_API_TOKEN = new InjectionToken<PagedApiService<User, User>>("USER_API_TOKEN");

export function apiServiceFactory<TModel extends HasId, TDetails extends HasId>(http: HttpClient, logger: LoggerService) {
    return new PagedApiService<TModel, TDetails>(http, logger);
}

export const GROUPS_API_TOKEN = new InjectionToken<GroupsApiService>("GROUPS_API_TOKEN");

export function groupsApiServiceFactory<TModel extends HasId, TDetails extends HasId>(http: HttpClient, logger: LoggerService) {
    return new GroupsApiService(http, logger);
}

