import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { RegisterCommand } from 'src/app/models/user';
import { AuthenticationService } from 'src/app/services/authentication/authentication.service';
import { PasswordConfirmationValidatorService } from 'src/app/services/authentication/password-confirmation-validator.service';
import { LoggerService } from 'src/app/services/logger.service';
import { FormValidator } from '../base/form-validator';


@Component({
  selector: 'app-register-user',
  templateUrl: './register-user.component.html',
  styleUrls: ['./register-user.component.scss']
})
export class RegisterUserComponent extends FormValidator implements OnInit {
  public errorMessage: string = '';
  public showError: boolean = false;

  constructor(
    private authService: AuthenticationService,
    private pwConfirmValidator: PasswordConfirmationValidatorService,
    private router: Router,
    private logger: LoggerService
  ) { super(); }

  ngOnInit(): void {
    this.form = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required]),
      confirm: new FormControl('')
    });

    this.setConfirmValidators();
  }

  private setConfirmValidators() {
    const passwordControl = this.form.get('password');
    const confirmControl = this.form.get('confirm');

    if (!confirmControl || !passwordControl)
      throw new Error("Password controls not found!");

    confirmControl.setValidators([Validators.required,
    this.pwConfirmValidator.validateConfirmPassword(passwordControl)]);
  }

  public registerUser(registerFormValue: any) {
    this.showError = false;
    const formValues = { ...registerFormValue };
    const registerCommand: RegisterCommand = {
      email: formValues.email,
      password: formValues.password,
      confirmPassword: formValues.confirm
    };

    this.authService.registerUser(registerCommand).then(
      () => this.router.navigate(["/authentication/login"]),
      error => {
        this.errorMessage = error;
        this.showError = true;
      }
    );
  }
}
