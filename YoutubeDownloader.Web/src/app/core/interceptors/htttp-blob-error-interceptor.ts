import { Injectable } from "@angular/core";
import { HttpInterceptor, HttpHandler, HttpRequest, HttpEvent, HttpErrorResponse } from "@angular/common/http";
import { Observable, throwError } from "rxjs";
import { catchError } from "rxjs/operators";

@Injectable()
export class BlobErrorHttpInterceptor implements HttpInterceptor {
  public intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((err) => {
        if (err instanceof HttpErrorResponse && err.error instanceof Blob && err.error.type === "application/json") {
          return new Promise<any>((resolve, reject) => {
            let reader = new FileReader();
            reader.onload = (event: Event) => {
              try {
                const errorMessage = JSON.parse((<any>event.target).result);
                reject(
                  new HttpErrorResponse({
                    error: errorMessage,
                    headers: err.headers,
                    status: err.status,
                    statusText: err.statusText,
                    url: err.url ?? undefined,
                  })
                );
              } catch (_) {
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
      })
    );
  }
}
