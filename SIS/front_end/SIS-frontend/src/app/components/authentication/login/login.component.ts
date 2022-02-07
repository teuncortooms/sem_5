import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { SocialUser } from 'angularx-social-login';
import { ExternalLoginCommand, LoginCommand, LoginResponse } from 'src/app/models/user';
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';
import { FormValidator } from '../base/form-validator';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent extends FormValidator implements OnInit {
  public errorMessage: string = '';
  public showError: boolean = false;
  private returnUrl: string = '/';

  constructor(
    private authService: AuthenticationService,
    private router: Router,
    private route: ActivatedRoute
  ) { super(); }

  ngOnInit(): void {
    this.form = new FormGroup({
      email: new FormControl("", [Validators.required]),
      password: new FormControl("", [Validators.required])
    })

    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

  public loginUser(formValues: any) {
    this.showError = false;
    this.authService.loginUser(formValues.email, formValues.password)
      .then(
        () => this.handleSuccess(),
        error => this.handleError(error)
      )
  }

  public loginWithMicrosoft() {
    this.showError = false;
    this.authService.logInWithMicrosoft().then(
      () => this.handleSuccess(),
      error => this.handleError(error));
  }

  public loginWithGoogle() {
    this.showError = false;
    this.authService.logInWithGoogle().then(
      () => this.handleSuccess(),
      error => this.handleError(error));
  }

  private handleError(error: any) {
    this.errorMessage = error;
    this.showError = true;
  }

  private handleSuccess() {
    this.router.navigate([this.returnUrl]);
  }
}
