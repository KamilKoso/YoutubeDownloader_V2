import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ToastrService } from "ngx-toastr";
import { Observable, throwError } from "rxjs";
import { catchError } from "rxjs/operators";

@Injectable()
export class ErrorHandlingInterceptor implements HttpInterceptor {
  constructor(private _toastr: ToastrService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((err: HttpErrorResponse) => {
        if (err?.error.description && err?.error.showToast) {
          this._toastr.error(err?.error.description);
        } else if (!err?.error?.showToast) {
          return throwError(err);
        } else {
          this._toastr.error("Something went wrong. Try again.", "Error");
        }
        return throwError(err);
      })
    );
  }
}
