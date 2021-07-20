import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { MOCK_FORECASTS } from '../../data/mock-forecasts';
import { WeatherForecast } from '../../models/WeatherForecast';
import { MessageService } from '../message/message.service';
import { catchError, map, tap } from 'rxjs/operators';
import { environment } from '../../../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class WeatherService {
  private weatherUrl: string = environment.weatherEndpoint;  // URL to web api

  constructor(private http: HttpClient, private messageService: MessageService) {
  }

  public getForecasts(): Observable<WeatherForecast[]> {
    this.messageService.logMessage(`fetching forecasts from ${this.weatherUrl}`);
    return this.http.get<WeatherForecast[]>(this.weatherUrl)
      .pipe(
        tap(_ => this.messageService.logMessage('fetched forecasts')),
        catchError(this.handleError<WeatherForecast[]>('getForecasts', []))
      );
  }

  getForecast(date: string): Observable<WeatherForecast> {
    const hero = MOCK_FORECASTS.find(forecast => forecast.date === date)!;
    return of(hero);
  }

  
  /**
 * Handle Http operation that failed.
 * Let the app continue.
 * @param operation - name of the operation that failed
 * @param result - optional value to return as the observable result
 */
  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {

      console.error(error);
      this.messageService.logMessage(`${operation} failed: ${error.message}`);

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }
}



