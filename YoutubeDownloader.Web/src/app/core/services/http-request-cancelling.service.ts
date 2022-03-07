import { Injectable } from "@angular/core";
import { Observable, Subject } from "rxjs";

@Injectable({ providedIn: "root" })
export class HttpRequestCancellingService {
  private _cancellingSubject: Subject<void> = new Subject<void>();
  constructor() {}

  public cancelPendingRequests(): void {
    this._cancellingSubject.next();
  }

  public onCancelPendingRequests(): Observable<void> {
    return this._cancellingSubject.asObservable();
  }
}
