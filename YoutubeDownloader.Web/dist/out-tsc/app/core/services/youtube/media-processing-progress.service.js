import { __decorate } from "tslib";
import { Injectable } from "@angular/core";
import { Subject } from "rxjs";
let MediaProcessingProgressService = class MediaProcessingProgressService {
    constructor(_signalrService) {
        this.url = `${appConfig.apiUrl}/mediaProcessingProgress`;
        this.connection = _signalrService.startConnection(this.url);
        this.connection.start();
    }
    progressReported() {
        const subject = new Subject();
        this.connection.on("ReportProcessingProgress", (progress) => subject.next(progress));
        return subject.asObservable();
    }
    getConnectionId() {
        return this.connection.invoke("getConnectionId");
    }
};
MediaProcessingProgressService = __decorate([
    Injectable({ providedIn: "root" })
], MediaProcessingProgressService);
export { MediaProcessingProgressService };
//# sourceMappingURL=media-processing-progress.service.js.map