import { FormGroup } from '@angular/forms';

export class FormValidator {
  public form!: FormGroup;

  public isInvalidControl(controlName: string): boolean {
    if (!this.form) throw new Error("No registration form found!");
    const control = this.form.controls[controlName];
    return control.invalid && control.touched;
  }

  public hasError(controlName: string, errorName: string): boolean {
    if (!this.form) throw new Error("No registration form found!");
    const control = this.form.controls[controlName];
    return control.hasError(errorName);
  }
}
