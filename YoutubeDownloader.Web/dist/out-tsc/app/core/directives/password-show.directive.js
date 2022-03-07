import { __decorate } from "tslib";
import { Directive, HostListener, Input } from "@angular/core";
let PasswordShowDirective = class PasswordShowDirective {
    onMouseOver() {
        this.passwordShowDirective.type = "password";
    }
    onMouseLeave() {
        this.passwordShowDirective.type = "password";
    }
    onMouseDown() {
        this.passwordShowDirective.type = "text";
    }
};
__decorate([
    Input()
], PasswordShowDirective.prototype, "passwordShowDirective", void 0);
__decorate([
    HostListener("mouseup")
], PasswordShowDirective.prototype, "onMouseOver", null);
__decorate([
    HostListener("mouseleave")
], PasswordShowDirective.prototype, "onMouseLeave", null);
__decorate([
    HostListener("mousedown")
], PasswordShowDirective.prototype, "onMouseDown", null);
PasswordShowDirective = __decorate([
    Directive({
        selector: "[passwordShowDirective]",
    })
], PasswordShowDirective);
export { PasswordShowDirective };
//# sourceMappingURL=password-show.directive.js.map