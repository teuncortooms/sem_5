import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { PagedList } from 'src/app/models/pagedlist';
import { HasId } from 'src/app/interfaces/has-id';
import { environment } from 'src/environments/environment';
import { LoggerService } from '../logger.service';
import { v4 as uuid } from 'uuid';

@Injectable({
  providedIn: 'root'
})
export class PagedApiService<TListItem extends HasId, TDetails extends HasId> {
  protected apiRoute: string | undefined;
  protected apiUrl: string | undefined;
  protected readonly httpOptions = environment.httpOptions;

  constructor(protected http: HttpClient, protected logger: LoggerService) { }

  configure(apiRoute: string) {
    this.apiRoute = apiRoute;
    this.apiUrl = environment.myAPI + apiRoute;
  }

  async query(pageIndex: number, pageSize: number, filter = '', sortHeader = '', sortDirection = 'asc'): Promise<PagedList<TListItem>> {
    if (!this.apiUrl) throw this.noUrlError();
    this.logger.log(`Initiating query: ${this.apiUrl}...`);

    try {
      const result = await this.http.get<PagedList<TListItem>>(this.apiUrl, {
        headers: this.httpOptions.headers,
        params: {
          page: pageIndex + 1,
          pageSize: pageSize,
          filter: filter,
          orderBy: sortHeader + ' ' + sortDirection // the api allows multi-sorting!
        }
      }).toPromise();

      this.logger.log(`Queried ${this.apiRoute}`);
      return result;
    }
    catch (error) {
      this.HandleError(error, 'query');
      return { resultCount: 0, totalCount: 0, results: [] }
    }
  }

  private noUrlError() {
    const errorMessage = `No API url specified. Configure the service first!`;
    this.logger.log(errorMessage);
    return new Error(errorMessage);
  }

  async get(id: string): Promise<TDetails | undefined> {
    const url = this.apiUrl + `/${id}`;
    this.logger.log(`Initiating get: ${url}...`);

    try {
      const result = await this.http.get<TDetails>(url, this.httpOptions).toPromise();
      this.logger.log(`Loaded details of ${id}`);

      return result;
    }
    catch (error) {
      this.HandleError(error, 'get');
      return undefined;
    }
  }

  async getMyDetails(): Promise<TDetails | undefined> {
    const url = this.apiUrl + `/getMyDetails`;
    this.logger.log(`Initiating get: ${url}...`);

    try {
      const result = await this.http.get<TDetails>(url, this.httpOptions).toPromise();
      this.logger.log(`Loaded your details`);

      return result;
    }
    catch (error) {
      this.HandleError(error, 'get');
      return undefined;
    }
  }


  async create(obj: TDetails): Promise<TDetails | undefined> {
    if (!this.apiUrl) throw this.noUrlError();
    this.logger.log(`Initiating create: ${this.apiUrl}...`);

    try {
      const result = this.http.post<TDetails>(this.apiUrl, obj, this.httpOptions).toPromise();
      this.logger.log("Added successfully");
      return result;
    }
    catch (error) {
      this.HandleError(error, 'create');
      return undefined;
    }
  }

  async remove(ids: string[]): Promise<Object | undefined> {
    const url = this.apiUrl + "/deleteMany"
    this.logger.log(`Initiating delete: ${url}...`);

    try {
      const result = this.http.post<string>(url, { ids: ids }, this.httpOptions).toPromise();
      this.logger.log("Remove complete");

      return result;
    }
    catch (error) {
      this.HandleError(error, 'removeMany');
      return undefined;
    }
  }

  async update(id: string, data: TDetails): Promise<TDetails | undefined> {
    const url = this.apiUrl + `/${id}`;
    this.logger.log(`Initiating update: ${url}...`);

    try {
      const result = this.http.put<TDetails>(url, data, this.httpOptions).toPromise();
      this.logger.log("Updated successfully");

      return result;
    }
    catch (error) {
      this.HandleError(error, 'update');
      return undefined;
    }
  }

  protected HandleError(error: any, operation: string = 'operation'): Error {
    const errorMsg = `${operation} failed: ${error.message}`;
    this.logger.log(errorMsg, true);
    return new Error(errorMsg);
  }
}
