import { __decorate } from "tslib";
// tslint:disable:no-any directive-selector
import { Directive, Input } from "@angular/core";
let NgVarDirective = class NgVarDirective {
    constructor(vcRef, templateRef) {
        this.vcRef = vcRef;
        this.templateRef = templateRef;
        this.context = {};
    }
    set ngVar(context) {
        this.context.$implicit = this.context.ngVar = context;
        this.updateView();
    }
    updateView() {
        this.vcRef.clear();
        this.vcRef.createEmbeddedView(this.templateRef, this.context);
    }
};
__decorate([
    Input()
], NgVarDirective.prototype, "ngVar", null);
NgVarDirective = __decorate([
    Directive({ selector: "[ngVar]" })
], NgVarDirective);
export { NgVarDirective };
//# sourceMappingURL=ng-var.directive.js.map