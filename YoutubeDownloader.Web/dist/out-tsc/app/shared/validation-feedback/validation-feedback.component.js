import { __decorate, __param } from "tslib";
import { Component, Input, Optional } from "@angular/core";
let ValidationFeedbackComponent = class ValidationFeedbackComponent {
    constructor(_formGroup) {
        this._formGroup = _formGroup;
        this.validationMessages = new Map([
            ["required", "Field is required."],
            ["email", "E-mail format is incorrect."],
            ["pattern", "Invalid pattern."],
        ]);
    }
    set customValidationMessages(messages) {
        if (messages) {
            this._customValidationMessages = messages;
            this.setCustomMessages(messages);
        }
    }
    getValidationError() {
        var _a, _b;
        this.setDefaultMessagesWithVariables();
        let errorMessage = "";
        if ((_a = this.control) === null || _a === void 0 ? void 0 : _a.errors) {
            Object.keys((_b = this.control) === null || _b === void 0 ? void 0 : _b.errors).forEach((errorKey) => {
                var _a;
                errorMessage = (_a = this.validationMessages.get(errorKey)) !== null && _a !== void 0 ? _a : "";
                if (!errorMessage) {
                    throw new Error(`No error message provided for error key ${errorKey}`);
                }
            });
        }
        return errorMessage;
    }
    ngOnInit() {
        if (!this.control && !this.controlName) {
            throw new Error("Validation Feedback must have [control] or [controlName] inputs");
        }
        else if (this.controlName && this._formGroup) {
            this.control = this._formGroup.form.get(this.controlName);
        }
    }
    setDefaultMessagesWithVariables() {
        var _a, _b, _c, _d, _e, _f;
        if (!((_a = this._customValidationMessages) === null || _a === void 0 ? void 0 : _a.has("minlength"))) {
            this.validationMessages.set("minlength", `Field has to have at least ${(_c = (_b = this.control) === null || _b === void 0 ? void 0 : _b.getError("minlength")) === null || _c === void 0 ? void 0 : _c.requiredLength} characters.`);
        }
        if (!((_d = this._customValidationMessages) === null || _d === void 0 ? void 0 : _d.has("maxlength"))) {
            this.validationMessages.set("maxlength", `Field can have at most ${(_f = (_e = this.control) === null || _e === void 0 ? void 0 : _e.getError("maxlength")) === null || _f === void 0 ? void 0 : _f.requiredLength} characters.`);
        }
    }
    setCustomMessages(messages) {
        messages.forEach((value, key) => {
            this.validationMessages.set(key, value);
        });
    }
};
__decorate([
    Input()
], ValidationFeedbackComponent.prototype, "control", void 0);
__decorate([
    Input()
], ValidationFeedbackComponent.prototype, "controlName", void 0);
__decorate([
    Input()
], ValidationFeedbackComponent.prototype, "customValidationMessages", null);
ValidationFeedbackComponent = __decorate([
    Component({
        selector: "app-validation-feedback",
        templateUrl: "./validation-feedback.component.html",
    }),
    __param(0, Optional())
], ValidationFeedbackComponent);
export { ValidationFeedbackComponent };
//# sourceMappingURL=validation-feedback.component.js.map