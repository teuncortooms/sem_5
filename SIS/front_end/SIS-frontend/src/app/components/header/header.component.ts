import { AfterViewInit, Component, OnInit } from '@angular/core';
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';
import { SocialUser } from "angularx-social-login";
import { stringify } from '@angular/compiler/src/util';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  isUserAuthenticated: boolean = false;
  photo?: string = `assets/temp.png`;

  constructor(
    private authService: AuthenticationService,
  ) { }

  // private initPhoto() {
  //   const stored = localStorage.getItem("socialUser");
  //   const user = stored ? JSON.parse(stored) : {};
  //   this.photo = user.photoUrl ?? `assets/temp.png`;
  // }

  ngOnInit(): void {
    // this.authService.externalAuthService.authState.subscribe(user => {
    //   localStorage.setItem("socialUser", JSON.stringify(user));
    //   this.photo = user.photoUrl;
    // });
    this.authService.authChanged.subscribe(isAuth => {
      this.isUserAuthenticated = isAuth;
    });
  }
}
