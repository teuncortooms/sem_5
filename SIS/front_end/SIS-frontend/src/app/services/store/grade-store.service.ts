import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Grade } from 'src/app/models/grade';
import { LoggerService } from '../logger.service';
import { GradeApiService } from '../api/grade-api.service';
import { v4 as uuid } from 'uuid';

@Injectable({
  providedIn: 'root'
})
export class GradeStoreService {
  // source: https://dev.to/avatsaev/simple-state-management-in-angular-with-only-services-and-rxjs-41p8

  // - private _groupsData starts empty
  // - make datastream available to subscribers
  private readonly _grades = new BehaviorSubject<Grade[]>([]);
  readonly gradesStream = this._grades.asObservable();


  // - getter in case you need the last value emitted in _todos subject
  // - setter to push a value which will trigger BehaviorSubject subscribers
  get grades(): Grade[] { return this._grades.getValue(); }
  set grades(val: Grade[]) { this._grades.next(val); }

  constructor(private apiService: GradeApiService, private logger: LoggerService) {
    this.fetchAll();
   }


  private async fetchAll() {
    this.grades = await this.apiService.getAll().toPromise();
  }

  public async addGrade(input: Grade): Promise<Grade | undefined> {
    this.logger.log(`Adding grade ${input.score}...`);
    if (!input.group ||
      !input.score ||
      !input.student) {
      throw "Missing property!";
    }

    // This is called an optimistic update
    // updating the record locally before actually getting a response from the server
    // this way, the interface seems blazing fast to the enduser
    // and we just assume that the server will return success responses anyway most of the time.
    // if server returns an error, we just revert back the changes in the catch statement
    const tmpId = uuid();
    const tmpGrade: Grade = {
      ...input, id: tmpId
    };

    this.grades = [
      ...this.grades,
      tmpGrade
    ];

    try {
      const details = await this.apiService.create(input).toPromise();
      if(!details) throw "Failure";
      const grade: Grade = {
        ...details
      };

      // we swap the local tmp record with the record from the server (id must be updated)
      const index = this.grades.indexOf(this.grades.find(g => g.id === tmpId)!);
      this.grades[index] = {
        ...grade
      }
      this.grades = [...this.grades]; // triggers change detection

      return details;
    } catch (e) {
      // if server sends back an error, we revert the changes
      this.logger.log(e as string);
      this.removeGrade(tmpId, false);
      return undefined;
    }
  }

  public async removeGrade(id: string, serverRemove = true): Promise<void> {
    // optimistic update
    const grade = this.grades.find(g => g.id === id);
    this.grades = this.grades.filter(g => g.id !== id);

    if (serverRemove && grade) {
      try {
        let response = await this.apiService.remove(id).toPromise();
        if(!response) throw "Failure";
      } catch (e) {
        this.logger.log(e as string);
        this.grades = [grade, ...this.grades];
      }
    }
  }

  async update(id: string, data: Grade): Promise<Grade | undefined> {
    if(id != data.id) throw `${id} does not match with data details!`;

    let original = this.grades.find(g => g.id === id);
    if (!original) throw `Cannot find grade ${id}!`;

    if (!data.group || !data.score || !data.student) {
      throw "Missing property!";
    }

    // optimistic update
    const index = this.grades.indexOf(original);
    const updates: Grade = { ...data };
    this.grades[index] = {
      ...updates
    }

    this.grades = [...this.grades]; // trigger subscribers

    try {
       let response = await this.apiService.update(id, data).toPromise();
       if(!response) throw "Failure";
       return response;
    } catch (e) {
      this.logger.log(e as string);
      this.grades[index] = {
        ...original
      }
      return undefined;
    }
  }

}
