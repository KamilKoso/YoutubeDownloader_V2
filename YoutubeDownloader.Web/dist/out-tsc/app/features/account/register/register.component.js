import { __decorate } from "tslib";
import { Component } from "@angular/core";
import { Validators } from "@angular/forms";
import { PasswordValidator } from "./password-strength.validator";
let RegisterComponent = class RegisterComponent {
    constructor(_fb, _authService, _toastr) {
        this._fb = _fb;
        this._authService = _authService;
        this._toastr = _toastr;
        this.registerForm = this._fb.group({
            email: ["", [Validators.required, Validators.email]],
            username: ["", [Validators.required, Validators.minLength(6)]],
            password: ["", PasswordValidator.strength],
            confirmPassword: ["", [Validators.required]],
        }, {
            validators: PasswordValidator.repeat("password", "confirmPassword"),
        });
        this.passwordValidationErrors = new Map([
            ["upperCaseCharacters", "Password must contain at least one upper character"],
            ["numberCharacters", "Password must contain at least one number"],
            ["specialCharacters", "Password must contain at least one special character"],
            ["lowerCaseCharacters", "Password must contain at least one lower character"],
            ["minlength", "Password must be at least 8 characters long"],
        ]);
        this.confirmPasswordValidationErrors = new Map([["repeat", "Password and confirm password does not match"]]);
    }
    register() {
        if (this.registerForm.valid) {
            this._authService
                .register({
                email: this.registerForm.value.email,
                username: this.registerForm.value.username,
                password: this.registerForm.value.password,
            })
                .subscribe((_) => {
                this._toastr.success("Confirm your email before first login", "Registration completed !");
            }, (error) => {
                this._toastr.error(error.error[0].description);
            });
        }
    }
};
RegisterComponent = __decorate([
    Component({
        selector: "app-register",
        templateUrl: "./register.component.html",
        styleUrls: ["./register.component.scss"],
    })
], RegisterComponent);
export { RegisterComponent };
//# sourceMappingURL=register.component.js.map