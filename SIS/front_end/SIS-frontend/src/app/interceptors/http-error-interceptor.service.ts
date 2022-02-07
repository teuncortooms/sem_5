import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { LoggerService } from '../services/logger.service';

@Injectable({
  providedIn: 'root'
})
export class HttpErrorInterceptorService implements HttpInterceptor {

  constructor(
    private router: Router,
    private logger: LoggerService
  ) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request)
      .pipe(
        catchError((error: HttpErrorResponse) => {
          let errorMessage = this.getErrorMessage(error);
          return throwError(errorMessage);
        })
      )
  }

  private getErrorMessage(error: HttpErrorResponse): string {
    if (error.status === 404) {
      return this.getNotFoundMessage(error);
    }
    else if (error.status === 400) {
      return this.getBadRequestMessage(error);
    }
    else if (error.status === 401) {
      return this.getUnauthorizedMessage(error);
    }
    return "Onverwachte fout. Neem contact op met de helpdesk.";
  }

  private getUnauthorizedMessage(error: HttpErrorResponse): string {
    if (this.router.url.startsWith('/authentication/login')) { // FIXME: hard-code is foutgevoelig
      return 'Toegang geweigerd';
    }
    else {
      this.router.navigate(['/authentication/login']);
      return error.message;
    }
  }

  private getNotFoundMessage(error: HttpErrorResponse): string {
    this.router.navigate(['/404']);
    return error.message;
  }

  private getBadRequestMessage(error: HttpErrorResponse): string {
    if (this.router.url === '/authentication/register' // FIXME: hard-code is foutgevoelig
      || this.router.url === '/authentication/login') {
      let message = '';
      const values: string[] = Object.values(error.error.errors);
      values.map((m: string) => message += m + '<br>');
      return message.slice(0, -4);
    }
    else {
      console.log(error);
      this.logger.log(error.error, true);
      return error.error ? error.error : error.message;
    }
  }
}
