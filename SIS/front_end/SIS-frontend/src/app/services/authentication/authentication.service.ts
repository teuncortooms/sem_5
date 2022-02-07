import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { from, of, Subject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ExternalLoginCommand, LoginCommand, LoginResponse, RegisterCommand, RegisterResponse, UserDetailsResponse } from '../../models/user';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Router } from '@angular/router';
import { GoogleLoginProvider, MicrosoftLoginProvider, SocialAuthService, SocialUser } from 'angularx-social-login';
import { LoggerService } from '../logger.service';


@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private authChangeSubject = new Subject<boolean>()
  public authChanged = this.authChangeSubject.asObservable();

  constructor(
    private http: HttpClient,
    private jwtHelper: JwtHelperService,
    private router: Router,
    private socialAuthService: SocialAuthService,
    private logger: LoggerService
  ) { }

  public isUserAuthenticated() {
    const token = localStorage.getItem("token");
    const regularAuthenticated = token != undefined && token != null && !this.jwtHelper.isTokenExpired(token);

    this.sendAuthStateChangeNotification(regularAuthenticated);
    return regularAuthenticated;
  }

  public retrieveClaims():string[]
  {
    const tokenStr = localStorage.getItem("token");
    if(tokenStr == null) return [];

    const token = this.jwtHelper.decodeToken(tokenStr);
    var claims:string[] = token['SIS-Claims'].constructor.name != "Array" ? [token['SIS-Claims']] : token['SIS-Claims'];

    return claims;
  }

  public hasClaimPart(part:string)
  {
    const tokenStr = localStorage.getItem("token");
    if(tokenStr == null) return false;

    const token = this.jwtHelper.decodeToken(tokenStr);
    var claims:string[] =  token['SIS-Claims'].constructor.name != "Array" ? [token['SIS-Claims']] : token['SIS-Claims'];
    var hasAllPermissions = false;
    for(let permission of claims)
    {
      if(permission.includes(part))
        hasAllPermissions = true;
    }
    const isAdmin = tokenStr != undefined && tokenStr != null && hasAllPermissions;

    return isAdmin;

  }

  public isAdmin(): boolean {
    return this.hasClaimPart('p_all');
  }

  public sendAuthStateChangeNotification(isAuthenticated: boolean) {
    this.authChangeSubject.next(isAuthenticated);
  }

  public async registerUser(body: RegisterCommand): Promise<RegisterResponse> {
    const url = environment.registrationEndpoint;
    var result = await this.http.post<RegisterResponse>(url, body).toPromise();
    this.logger.log("Registratie succesvol. Probeer in te loggen.", true);
    return result; // errors are formatted by interceptor
  }

  public async loginUser(email: string, password: string): Promise<void> {
    const url = environment.loginEndpoint;
    const body: LoginCommand = { email: email, password: password };
    const response = await this.http.post<LoginResponse>(url, body).toPromise();
    this.handleLoginResponse(response);
  }

  public async externalLogin(externalLoginPayload: ExternalLoginCommand): Promise<void> {
    const url = environment.externalLoginEndpoint;
    const response = await this.http.post<LoginResponse>(url, externalLoginPayload).toPromise();
    this.handleLoginResponse(response);
  }

  private handleLoginResponse(response: LoginResponse) {
    localStorage.setItem("token", response.token);
    const isLoggedIn: boolean = response.statusCode == 0;
    this.sendAuthStateChangeNotification(isLoggedIn);
  }

  public logout() {
    localStorage.removeItem("token");
    this.sendAuthStateChangeNotification(false);
    this.router.navigate(['/authentication/login']);
  }

  public async getUserDetails(): Promise<UserDetailsResponse> {
    const url = environment.userDetailsEndpoint;
    return await this.http.get<UserDetailsResponse>(url).toPromise();
  }


  // social options

  public async logInWithGoogle(): Promise<void> {
    return await this.getSocialIdTokenAndLogin(GoogleLoginProvider.PROVIDER_ID);
  }

  public async logInWithMicrosoft(): Promise<void> {
    return await this.getSocialIdTokenAndLogin(MicrosoftLoginProvider.PROVIDER_ID);
  }

  public async getSocialIdTokenAndLogin(providerId: string): Promise<void> {
    try {
      const response = await this.socialAuthService.signIn(providerId);
      const user: SocialUser = { ...response };

      await this.externalLogin({ provider: user.provider, idToken: user.idToken });
    }
    catch (e: unknown) {
      this.signOutSocial();
      throw e;
    }
  }

  public signOutSocial(): void {
    this.socialAuthService.signOut();
  }
}
