import { CollectionViewer, DataSource } from "@angular/cdk/collections";
import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable, of } from "rxjs";
import { PagedList } from "src/app/models/pagedlist";
import { LoggerService } from "../logger.service";
import { PagedApiService } from "../api/paged-api.service";
import { v4 as uuid } from 'uuid';
import { HasId } from "src/app/interfaces/has-id";


@Injectable()
export class PagedDataSource<TListItem extends HasId, TDetails extends HasId> implements DataSource<TListItem> {
  private dataSubject = new BehaviorSubject<TListItem[]>([]);
  public get currentData() { return this.dataSubject.value };

  private countSubject = new BehaviorSubject<number>(0);
  public countStream = this.countSubject.asObservable();

  private loadingSubject = new BehaviorSubject<boolean>(false);
  public loadingStream = this.loadingSubject.asObservable();

  private apiRoute: string | undefined;

  private savedArgs?: { pageIndex: number, pageSize: number, filter?: string, sortHeader?: string, sortDirection?: string };

  constructor(private apiService: PagedApiService<TListItem, TDetails>, private logger: LoggerService) { }

  configure(apiRoute: string) {
    this.apiService.configure(apiRoute);
    this.apiRoute = apiRoute;
  }

  connect(collectionViewer: CollectionViewer): Observable<TListItem[]> {
    return this.dataSubject.asObservable();
  }

  disconnect(collectionViewer: CollectionViewer): void {
    // FIXME: Why is disconnect called?? or why is it not reconnecting?
    // this.dataSubject.complete();
    // this.loadingSubject.complete();
  }

  load(pageIndex: number, pageSize: number, filter?: string, sortHeader?: string, sortDirection?: string): void {
    this.loadingSubject.next(true);

    this.savedArgs = { pageIndex, pageSize, filter, sortHeader, sortDirection };

    this.apiService.query(pageIndex, pageSize, filter, sortHeader, sortDirection)
      .then(response => {
        const pagedList = response as PagedList<TListItem>;
        this.dataSubject.next(pagedList.results);
        this.countSubject.next(pagedList.totalCount);
        
        this.loadingSubject.next(false);
      });
  }


  reload(): void {
    if (!this.savedArgs) {
      this.logger.log("Nothing to reload");
      return;
    }

    const s = this.savedArgs;
    this.load(s.pageIndex, s.pageSize, s.filter, s.sortHeader, s.sortDirection);
  }

  public async getMyDetails(): Promise<TDetails | undefined> {
    return this.apiService.getMyDetails();
  }

  public async getDetails(id: string): Promise<TDetails | undefined> {
    return this.apiService.get(id);
  }

  // The next functions use optimistic update, which means updating the record locally before 
  // actually getting a response from the server. This way, the interface seems blazing fast 
  // to the end-user and we just assume that the server will mostly return success responses anyway.
  // If the server returns an error, we just revert back the changes in the catch statement.

  public async add(input: TDetails): Promise<TDetails | undefined> {
    this.logger.log(`Adding ${this.apiRoute}...`);

    // optimistic local update
    const tmpId = uuid();
    const tmpModel: TListItem = { ...input, id: tmpId } as unknown as TListItem;
    // somehow HasId blocks this mapping without a convulated cast??

    this.dataSubject.next([tmpModel, ...this.dataSubject.value]);
    this.countSubject.next(this.countSubject.value + 1);

    try {
      // update server
      const details = await this.apiService.create(input);
      if (!details) throw "Failure";
      const model: TListItem = { ...details } as unknown as TListItem;

      // we swap the local tmp record with the record from the server (id must be updated)
      const index = this.dataSubject.value.indexOf(this.dataSubject.value.find(g => g.id === tmpId)!);
      const newData = this.dataSubject.value;
      newData[index] = { ...model };
      this.dataSubject.next(newData); // triggers subscribers

      return details;
    } catch (e) {
      this.logger.log(e as string);
      // restore original locally
      this.remove([tmpId], false);
      return undefined;
    }
  }

  public async remove(ids: string[], serverRemove = true): Promise<void> {
    // optimistic local update
    const selectedModels = this.dataSubject.value.filter(m => ids.includes(m.id));
    const dataMinusModels = this.dataSubject.value.filter(m => !ids.includes(m.id));
    this.dataSubject.next(dataMinusModels); // triggers subscribers
    this.countSubject.next(this.countSubject.value - selectedModels.length);

    if (serverRemove && selectedModels) {
      try {
        // update server
        let response = await this.apiService.remove(ids);
        if (response === undefined) throw "Failure";
      } catch (e) {
        this.logger.log(e as string);
        // restore original locally
        const oldData = [...selectedModels, ...this.dataSubject.value];
        this.dataSubject.next(oldData);
        this.countSubject.next(this.countSubject.value + selectedModels.length);
      }
    }
  }

  async update(id: string, data: TDetails): Promise<TDetails | undefined> {
    if (id != data.id) throw `${id} does not match with data details!`;

    let original = this.dataSubject.value.find(m => m.id === id);
    if (!original) throw `Cannot find id ${id}!`;

    // optimistic local update
    const index = this.dataSubject.value.indexOf(original);
    const updates: TListItem = { ...data } as unknown as TListItem;

    const newData = this.dataSubject.value;
    newData[index] = { ...updates };
    this.dataSubject.next(newData); // triggers subscribers

    try {
      // update server
      let response = await this.apiService.update(id, data);
      if (!response) throw "Failure";
      return response;
    } catch (e) {
      this.logger.log(e as string);
      // restore original locally
      const oldData = this.dataSubject.value;
      oldData[index] = { ...original };
      this.dataSubject.next(oldData);

      return undefined;
    }
  }
}
