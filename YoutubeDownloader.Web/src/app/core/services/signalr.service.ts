import { Injectable } from "@angular/core";
import * as signalR from "@microsoft/signalr";

@Injectable({ providedIn: "root" })
export class SignalrService {
  constructor() {}

  startConnection(url: string): signalR.HubConnection {
    return new signalR.HubConnectionBuilder()
      .withUrl(url, {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets,
      })
      .withAutomaticReconnect([0, 1000, 5000, 10000, 20000])
      .build();
  }
}
