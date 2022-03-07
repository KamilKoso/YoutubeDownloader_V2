import { __decorate } from "tslib";
import { Injectable } from "@angular/core";
import { Subject } from "rxjs";
let HttpRequestCancellingService = class HttpRequestCancellingService {
    constructor() {
        this._cancellingSubject = new Subject();
    }
    cancelPendingRequests() {
        this._cancellingSubject.next();
    }
    onCancelPendingRequests() {
        return this._cancellingSubject.asObservable();
    }
};
HttpRequestCancellingService = __decorate([
    Injectable({ providedIn: "root" })
], HttpRequestCancellingService);
export { HttpRequestCancellingService };
//# sourceMappingURL=http-request-cancelling.service.js.map