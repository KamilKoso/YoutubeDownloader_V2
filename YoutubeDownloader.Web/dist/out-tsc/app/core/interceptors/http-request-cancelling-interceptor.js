import { __decorate } from "tslib";
// managehttp.interceptor.ts
import { Injectable } from "@angular/core";
import { ActivationEnd } from "@angular/router";
import { takeUntil } from "rxjs/operators";
let HttpRequestCancellingInterceptor = class HttpRequestCancellingInterceptor {
    constructor(router, _httpRequestCancellingService) {
        this._httpRequestCancellingService = _httpRequestCancellingService;
        router.events.subscribe((event) => {
            // An event triggered at the end of the activation part of the Resolve phase of routing.
            if (event instanceof ActivationEnd) {
                // Cancel pending calls
                this._httpRequestCancellingService.cancelPendingRequests();
            }
        });
    }
    intercept(req, next) {
        return next
            .handle(req)
            .pipe(takeUntil(this._httpRequestCancellingService.onCancelPendingRequests()));
    }
};
HttpRequestCancellingInterceptor = __decorate([
    Injectable()
], HttpRequestCancellingInterceptor);
export { HttpRequestCancellingInterceptor };
//# sourceMappingURL=http-request-cancelling-interceptor.js.map