import { __decorate } from "tslib";
import { Directive, Output, Input, HostListener, EventEmitter } from "@angular/core";
let WindowScrollOffsetDirective = class WindowScrollOffsetDirective {
    constructor() {
        this.windowScrollOffset = new EventEmitter();
    }
    // <summary>
    // Event listener for scroll event on the specific ui element
    // </summary>
    onListenerTriggered() {
        const scrollTop = document.documentElement.scrollTop;
        const scrollHeight = document.documentElement.scrollHeight;
        const clientHeight = document.documentElement.clientHeight;
        const percent = Math.round((scrollTop / (scrollHeight - clientHeight)) * 100);
        if (this.scrollOffsetPercentage <= percent) {
            this.windowScrollOffset.emit(percent);
        }
    }
};
__decorate([
    Output()
], WindowScrollOffsetDirective.prototype, "windowScrollOffset", void 0);
__decorate([
    Input()
], WindowScrollOffsetDirective.prototype, "scrollOffsetPercentage", void 0);
__decorate([
    HostListener("window:scroll", ["$event"])
], WindowScrollOffsetDirective.prototype, "onListenerTriggered", null);
WindowScrollOffsetDirective = __decorate([
    Directive({
        // tslint:disable-next-line: directive-selector
        selector: "[scrollOffsetPercentage]",
    })
], WindowScrollOffsetDirective);
export { WindowScrollOffsetDirective };
//# sourceMappingURL=window-scroll-offset.directive.js.map