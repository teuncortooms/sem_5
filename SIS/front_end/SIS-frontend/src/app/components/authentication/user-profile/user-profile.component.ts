import { Component, OnInit } from '@angular/core';
import { Claim } from 'src/app/models/claim';
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit {
  username?: string;
  roles?: string[];
  claims?: string[];

  constructor(
    private authService: AuthenticationService
  ) { }

  ngOnInit(): void {
    this.getUserDetails();
  }

  private async getUserDetails() {
    var response = await this.authService.getUserDetails();
    this.username = response.username;
    this.roles = response.roles;
    this.claims = this.authService.retrieveClaims();

  }

  public convertClaim(str:string):string
  {
    var claim:Claim = { name: str, id:""}
    return Claim.ClaimToDisplayName(claim);
  }

  public logout() {
    this.authService.logout();
  }

}
