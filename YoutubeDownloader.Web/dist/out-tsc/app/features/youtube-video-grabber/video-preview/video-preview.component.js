import { __decorate } from "tslib";
import { Component, Input } from "@angular/core";
let VideoPreviewComponent = class VideoPreviewComponent {
    constructor() { }
    ngOnInit() { }
    formatDuration(duration) {
        let result = "";
        // Days
        if (duration.days > 0) {
            result += duration.days;
            result += ":";
        }
        // Hours
        if (duration.hours > 0) {
            result += duration.hours;
            result += ":";
        }
        // Minutes
        if (duration.minutes > 0) {
            result += duration.minutes < 10 ? "0" + duration.minutes : duration.minutes;
            result += ":";
        }
        else {
            result += "0:";
        }
        // Seconds
        if (duration.seconds > 0) {
            result += duration.seconds < 10 ? "0" + duration.seconds : duration.seconds;
        }
        else {
            result += "00";
        }
        return result;
    }
    formatViews(views) {
        if (views > 1000000) {
            return Math.floor(views / 1000000) + "M views";
        }
        else if (views > 100000) {
            return Math.floor(views / 1000) + "K views";
        }
        else if (views > 10000) {
            return Math.floor(views / 1000) + "K views";
        }
        else {
            return views;
        }
    }
};
__decorate([
    Input()
], VideoPreviewComponent.prototype, "videoMetadata", void 0);
VideoPreviewComponent = __decorate([
    Component({
        selector: "app-video-preview",
        templateUrl: "./video-preview.component.html",
        styleUrls: ["./video-preview.component.scss"],
    })
], VideoPreviewComponent);
export { VideoPreviewComponent };
//# sourceMappingURL=video-preview.component.js.map