import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { LoggerService } from '../logger.service';
import { environment } from 'src/environments/environment';
import { Grade } from 'src/app/models/grade';
import { PagedList } from 'src/app/models/pagedlist';

@Injectable({
  providedIn: 'root'
})
export class GradeApiService {
  private readonly gradesUrl = environment.gradesEndpoint;
  private readonly httpOptions = environment.httpOptions;

  constructor(private http: HttpClient, private logger: LoggerService) { }

  query(filter = '', sortOrder = 'asc',
  pageNumber = 0, pageSize = 3): Observable<PagedList<Grade>>
  {
    this.logger.log("Query grades...");
    return this.http.get<PagedList<Grade>>(this.gradesUrl+`?page=${pageNumber+1}&pageSize=${pageSize}&filter=${filter}`)
      .pipe(
        tap({
          complete: () => this.logger.log("Queried grades")
        }),
        catchError(error => this.HandleError<PagedList<Grade>>(error, undefined, 'query'))
      );
  }

  getAll(): Observable<Grade[]> {
    this.logger.log("Loading grades...");
    return this.http.get<Grade[]>(this.gradesUrl+`?page=1&pageSize=10`)
      .pipe(
        tap({
          complete: () => this.logger.log("Loaded grades")
        }),
        catchError(error => this.HandleError<Grade[]>(error, undefined, 'getAll'))
      );
  }

  get(id: string): Observable<Grade | undefined> {
    this.logger.log(`Loading details of ${id}`);
    const url = this.gradesUrl + `/ ${id}`;
    return this.http.get<Grade>(url, this.httpOptions)
      .pipe(
        tap({
          complete: () => this.logger.log(`Loaded details of ${id}`)
        }),
        catchError(error => this.HandleError<Grade>(error, undefined, 'get'))
      );
  }

  create(group: Grade): Observable<Grade> {
    return this.http.post<Grade>(this.gradesUrl, group, this.httpOptions)
      .pipe(
        tap({
          complete: () => this.logger.log("Grade added successfully")
        }),
        catchError(error => this.HandleError<Grade>(error, undefined, 'create'))
      );
  }

  remove(id: string): Observable<Object> {
    this.logger.log(`Removing grade (${id})...`);
    const url = this.gradesUrl + `/ ${id}`;
    return this.http.delete<string>(url, this.httpOptions)
      .pipe(
        tap({
          complete: () => this.logger.log("Remove grade complete")
        }),
        catchError(error => this.HandleError<string>(error, undefined, 'remove'))
      );
  }

  update(id: string, data: Grade): Observable<Grade> {
    this.logger.log(`Updating grade (${id})...`);

    const url = this.gradesUrl + `/ ${id}`;
    return this.http.put<Grade>(url, data, this.httpOptions)
      .pipe(
        tap({
          complete: () => this.logger.log("Grade updated successfully")
        }),
        catchError(error => this.HandleError<Grade>(error, undefined, 'update'))
      );
  }

  private HandleError<T>(error: any, result?: T, operation: string = 'operation'): Observable<T> {
    this.logger.log(`${operation} failed: ${error.message}`);

    // Let the app continue by returning an empty result.
    return of(result as T);
  }

}
