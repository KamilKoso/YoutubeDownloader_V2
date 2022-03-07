// managehttp.interceptor.ts
import { Injectable } from "@angular/core";
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
} from "@angular/common/http";
import { Observable } from "rxjs";
import { Router, ActivationEnd } from "@angular/router";
import { takeUntil } from "rxjs/operators";
import { HttpRequestCancellingService } from "../services/http-request-cancelling.service";

@Injectable()
export class HttpRequestCancellingInterceptor implements HttpInterceptor {
  constructor(
    router: Router,
    private _httpRequestCancellingService: HttpRequestCancellingService
  ) {
    router.events.subscribe((event) => {
      // An event triggered at the end of the activation part of the Resolve phase of routing.
      if (event instanceof ActivationEnd) {
        // Cancel pending calls
        this._httpRequestCancellingService.cancelPendingRequests();
      }
    });
  }

  intercept<T>(
    req: HttpRequest<T>,
    next: HttpHandler
  ): Observable<HttpEvent<T>> {
    return next
      .handle(req)
      .pipe(
        takeUntil(this._httpRequestCancellingService.onCancelPendingRequests())
      );
  }
}
