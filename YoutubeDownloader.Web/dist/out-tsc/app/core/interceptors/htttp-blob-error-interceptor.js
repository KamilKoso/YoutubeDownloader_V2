import { __decorate } from "tslib";
import { Injectable } from "@angular/core";
import { HttpErrorResponse } from "@angular/common/http";
import { throwError } from "rxjs";
import { catchError } from "rxjs/operators";
let BlobErrorHttpInterceptor = class BlobErrorHttpInterceptor {
    intercept(req, next) {
        return next.handle(req).pipe(catchError((err) => {
            if (err instanceof HttpErrorResponse && err.error instanceof Blob && err.error.type === "application/json") {
                return new Promise((resolve, reject) => {
                    let reader = new FileReader();
                    reader.onload = (event) => {
                        var _a;
                        try {
                            const errorMessage = JSON.parse(event.target.result);
                            reject(new HttpErrorResponse({
                                error: errorMessage,
                                headers: err.headers,
                                status: err.status,
                                statusText: err.statusText,
                                url: (_a = err.url) !== null && _a !== void 0 ? _a : undefined,
                            }));
                        }
                        catch (_) {
                            reject(err);
                        }
                    };
                    reader.onerror = (_) => {
                        reject(err);
                    };
                    reader.readAsText(err.error);
                });
            }
            return throwError(err);
        }));
    }
};
BlobErrorHttpInterceptor = __decorate([
    Injectable()
], BlobErrorHttpInterceptor);
export { BlobErrorHttpInterceptor };
//# sourceMappingURL=htttp-blob-error-interceptor.js.map