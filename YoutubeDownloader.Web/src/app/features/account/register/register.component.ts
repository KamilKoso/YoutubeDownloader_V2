import { Component } from "@angular/core";
import { FormBuilder, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { AuthService } from "@app/auth";
import { ToastrService } from "ngx-toastr";
import { switchMap } from "rxjs/operators";

import { PasswordValidator } from "./password-strength.validator";

@Component({
  selector: "app-register",
  templateUrl: "./register.component.html",
  styleUrls: ["./register.component.scss"],
})
export class RegisterComponent {
  registerForm = this._fb.group(
    {
      email: ["", [Validators.required, Validators.email]],
      username: ["", [Validators.required, Validators.minLength(6)]],
      password: ["", PasswordValidator.strength],
      confirmPassword: ["", [Validators.required]],
    },
    {
      validators: PasswordValidator.repeat("password", "confirmPassword"),
    }
  );

  passwordValidationErrors = new Map([
    ["upperCaseCharacters", "Password must contain at least one upper character"],
    ["numberCharacters", "Password must contain at least one number"],
    ["specialCharacters", "Password must contain at least one special character"],
    ["lowerCaseCharacters", "Password must contain at least one lower character"],
    ["minlength", "Password must be at least 8 characters long"],
  ]);

  confirmPasswordValidationErrors = new Map([["repeat", "Password and confirm password does not match"]]);

  constructor(private _fb: FormBuilder, private _authService: AuthService, private _toastr: ToastrService) {}

  register(): void {
    if (this.registerForm.valid) {
      this._authService
        .register({
          email: this.registerForm.value.email,
          username: this.registerForm.value.username,
          password: this.registerForm.value.password,
        })
        .subscribe(
          (_) => {
            this._toastr.success("Confirm your email before first login", "Registration completed !");
          },
          (error) => {
            this._toastr.error(error.error[0].description);
          }
        );
    }
  }
}
