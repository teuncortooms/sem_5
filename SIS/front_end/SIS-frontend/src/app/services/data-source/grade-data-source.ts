import { CollectionViewer, DataSource } from "@angular/cdk/collections";
import { Inject } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";
import { Grade } from "src/app/models/grade";
import { PagedList } from "src/app/models/pagedlist";
import { PagedApiService } from "../api/paged-api.service";
import { GRADES_API_TOKEN } from "../dependency-injection/di-config";

export class GradeDatasource implements DataSource<Grade> {
  private gradesSubject = new BehaviorSubject<Grade[]>([]);
  private loadingSubject = new BehaviorSubject<boolean>(false);
  private result?: PagedList<Grade>;

  public filter: string = '';
  public loading$ = this.loadingSubject.asObservable();

  constructor(@Inject(GRADES_API_TOKEN) private apiService: PagedApiService<Grade, Grade>) { }

  count(): number {
    return this.result?.totalCount ?? 0;
  }

  connect(collectionViewer: CollectionViewer): Observable<Grade[]> {
    return this.gradesSubject.asObservable();
  }

  disconnect(collectionViewer: CollectionViewer): void {
    this.gradesSubject.complete();
    this.loadingSubject.complete();
  }

  loadGrades(sortDirection = 'asc', pageIndex = 0, pageSize = 10) {

    this.loadingSubject.next(true);

    this.apiService.query(pageIndex, pageSize, this.filter, undefined, sortDirection)
      .then(r => {
        const pagedList = r as PagedList<Grade>;
        this.gradesSubject.next(pagedList.results);
        this.result = pagedList;

        this.loadingSubject.next(false);
      })
  }
}
