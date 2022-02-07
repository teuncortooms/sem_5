import { NgModule } from '@angular/core';
import { JwtModule } from "@auth0/angular-jwt";
import { environment } from 'src/environments/environment';


export function tokenGetter() {
  return localStorage.getItem("token");
}

const options = {
  config: {
    tokenGetter: tokenGetter,
    allowedDomains: environment.jwtInterceptorConfig.allowedDomains,
  }
};

@NgModule({
  imports: [JwtModule.forRoot(options)],
  exports: [JwtModule]
})
export class AppJwtModule { }
