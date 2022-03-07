import { __decorate } from "tslib";
import { NgModule } from "@angular/core";
import { PasswordShowDirective } from "./password-show.directive";
import { WindowScrollOffsetDirective } from "./window-scroll-offset.directive";
import { NgVarDirective } from "./ng-var.directive";
const directives = [WindowScrollOffsetDirective, PasswordShowDirective, NgVarDirective];
let DirectivesModule = class DirectivesModule {
};
DirectivesModule = __decorate([
    NgModule({
        declarations: directives,
        exports: directives,
    })
], DirectivesModule);
export { DirectivesModule };
//# sourceMappingURL=directives.module.js.map