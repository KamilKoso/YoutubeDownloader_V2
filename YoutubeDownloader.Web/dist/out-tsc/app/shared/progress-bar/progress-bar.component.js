import { __decorate } from "tslib";
import { Component, Input } from "@angular/core";
let ProgressBarComponent = class ProgressBarComponent {
    constructor() {
        this.progress = 0;
        this.height = "50px";
    }
};
__decorate([
    Input()
], ProgressBarComponent.prototype, "progress", void 0);
__decorate([
    Input()
], ProgressBarComponent.prototype, "height", void 0);
__decorate([
    Input()
], ProgressBarComponent.prototype, "progressBarText", void 0);
__decorate([
    Input()
], ProgressBarComponent.prototype, "mode", void 0);
ProgressBarComponent = __decorate([
    Component({
        // tslint:disable-next-line: component-selector
        selector: "progress-bar",
        templateUrl: "./progress-bar.component.html",
        styleUrls: ["./progress-bar.component.scss"],
    })
], ProgressBarComponent);
export { ProgressBarComponent };
//# sourceMappingURL=progress-bar.component.js.map