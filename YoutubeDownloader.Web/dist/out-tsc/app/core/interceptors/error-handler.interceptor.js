import { __decorate } from "tslib";
import { Injectable } from "@angular/core";
import { throwError } from "rxjs";
import { catchError } from "rxjs/operators";
let ErrorHandlingInterceptor = class ErrorHandlingInterceptor {
    constructor(_toastr) {
        this._toastr = _toastr;
    }
    intercept(req, next) {
        return next.handle(req).pipe(catchError((err) => {
            var _a;
            if ((err === null || err === void 0 ? void 0 : err.error.description) && (err === null || err === void 0 ? void 0 : err.error.showToast)) {
                this._toastr.error(err === null || err === void 0 ? void 0 : err.error.description);
            }
            else if (!((_a = err === null || err === void 0 ? void 0 : err.error) === null || _a === void 0 ? void 0 : _a.showToast)) {
                return throwError(err);
            }
            else {
                this._toastr.error("Something went wrong. Try again.", "Error");
            }
            return throwError(err);
        }));
    }
};
ErrorHandlingInterceptor = __decorate([
    Injectable()
], ErrorHandlingInterceptor);
export { ErrorHandlingInterceptor };
//# sourceMappingURL=error-handler.interceptor.js.map