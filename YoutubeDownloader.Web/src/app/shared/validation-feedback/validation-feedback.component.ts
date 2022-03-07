import { Component, Input, OnInit, Optional } from "@angular/core";
import { AbstractControl, FormGroupDirective } from "@angular/forms";

@Component({
  selector: "app-validation-feedback",
  templateUrl: "./validation-feedback.component.html",
})
export class ValidationFeedbackComponent implements OnInit {
  _customValidationMessages: Map<string, string>;
  @Input() control: Nullable<AbstractControl>;
  @Input() controlName: Nullable<string>;
  @Input() set customValidationMessages(messages: Nullable<Map<string, string>>) {
    if (messages) {
      this._customValidationMessages = messages;
      this.setCustomMessages(messages);
    }
  }
  validationMessages: Map<string, string | undefined> = new Map([
    ["required", "Field is required."],
    ["email", "E-mail format is incorrect."],
    ["pattern", "Invalid pattern."],
  ]);

  constructor(@Optional() private _formGroup: FormGroupDirective) {}

  getValidationError(): string {
    this.setDefaultMessagesWithVariables();

    let errorMessage = "";
    if (this.control?.errors) {
      Object.keys(this.control?.errors).forEach((errorKey) => {
        errorMessage = this.validationMessages.get(errorKey) ?? "";
        if (!errorMessage) {
          throw new Error(`No error message provided for error key ${errorKey}`);
        }
      });
    }
    return errorMessage;
  }

  ngOnInit(): void {
    if (!this.control && !this.controlName) {
      throw new Error("Validation Feedback must have [control] or [controlName] inputs");
    } else if (this.controlName && this._formGroup) {
      this.control = this._formGroup.form.get(this.controlName);
    }
  }

  setDefaultMessagesWithVariables(): void {
    if (!this._customValidationMessages?.has("minlength")) {
      this.validationMessages.set(
        "minlength",
        `Field has to have at least ${this.control?.getError("minlength")?.requiredLength} characters.`
      );
    }
    if (!this._customValidationMessages?.has("maxlength")) {
      this.validationMessages.set(
        "maxlength",
        `Field can have at most ${this.control?.getError("maxlength")?.requiredLength} characters.`
      );
    }
  }

  setCustomMessages(messages: Map<string, string>): void {
    messages.forEach((value: string, key: string) => {
      this.validationMessages.set(key, value);
    });
  }
}
