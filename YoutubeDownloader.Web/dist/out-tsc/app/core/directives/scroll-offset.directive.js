import { __decorate } from "tslib";
import { Directive, Output, Input, HostListener, EventEmitter } from "@angular/core";
let ScrollOffsetDirective = class ScrollOffsetDirective {
    constructor() {
        this.scrollOffset = new EventEmitter();
    }
    // <summary>
    // Event listener for scroll event on the specific ui element
    // </summary>
    onListenerTriggered(event) {
        const scrollTop = event.target.scrollTop;
        const scrollHeight = event.target.scrollHeight;
        const clientHeight = event.target.clientHeight;
        const percent = Math.round((scrollTop / (scrollHeight - clientHeight)) * 100);
        if (this.scrollOffsetPercentage <= percent) {
            this.scrollOffset.emit(percent);
        }
    }
};
__decorate([
    Output()
], ScrollOffsetDirective.prototype, "scrollOffset", void 0);
__decorate([
    Input()
], ScrollOffsetDirective.prototype, "scrollOffsetPercentage", void 0);
__decorate([
    HostListener("scroll", ["$event"])
], ScrollOffsetDirective.prototype, "onListenerTriggered", null);
ScrollOffsetDirective = __decorate([
    Directive({
        // tslint:disable-next-line: directive-selector
        selector: "[scrollOffsetPercentage]",
    })
], ScrollOffsetDirective);
export { ScrollOffsetDirective };
//# sourceMappingURL=scroll-offset.directive.js.map