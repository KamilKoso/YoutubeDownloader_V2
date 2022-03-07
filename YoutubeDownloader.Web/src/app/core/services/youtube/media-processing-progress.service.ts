import { Injectable } from "@angular/core";
import { Observable, of, Subject } from "rxjs";
import { SignalrService } from "../signalr.service";

@Injectable({ providedIn: "root" })
export class MediaProcessingProgressService {
  connection: signalR.HubConnection;
  url = `${appConfig.apiUrl}/mediaProcessingProgress`;

  constructor(_signalrService: SignalrService) {
    this.connection = _signalrService.startConnection(this.url);
    this.connection.start();
  }

  progressReported(): Observable<number> {
    const subject = new Subject<number>();
    this.connection.on("ReportProcessingProgress", (progress) => subject.next(progress));
    return subject.asObservable();
  }

  getConnectionId(): Promise<string> {
    return this.connection.invoke("getConnectionId");
  }
}
