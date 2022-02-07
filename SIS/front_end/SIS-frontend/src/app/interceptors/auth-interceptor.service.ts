


// TODO: REMOVE. Already implemented by jwt module!



// import { HttpInterceptor, HttpRequest, HttpHandler } from '@angular/common/http';
// import { Injectable } from '@angular/core';
// import { environment } from 'src/environments/environment';
// import { AuthenticationService } from './authentication.service';

// @Injectable()
// export class AuthInterceptor implements HttpInterceptor {
//     private secureRoutes = environment.jwtInterceptorConfig.allowedDomains;

//     constructor(private authService: AuthenticationService) { }

//     intercept(request: HttpRequest<any>, next: HttpHandler) {
//         if (!this.secureRoutes.find((x) => request.url.startsWith(x))) {
//             return next.handle(request);
//         }

//         const token = this.authService.token;

//         if (!token) {
//             return next.handle(request);
//         }

//         request = request.clone({
//             headers: request.headers.set('Authorization', 'Bearer ' + token),
//         });

//         return next.handle(request);
//     }
// }

