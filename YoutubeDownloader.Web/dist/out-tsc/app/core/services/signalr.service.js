import { __decorate } from "tslib";
import { Injectable } from "@angular/core";
import * as signalR from "@microsoft/signalr";
let SignalrService = class SignalrService {
    constructor() { }
    startConnection(url) {
        return new signalR.HubConnectionBuilder()
            .withUrl(url, {
            skipNegotiation: true,
            transport: signalR.HttpTransportType.WebSockets,
        })
            .withAutomaticReconnect([0, 1000, 5000, 10000, 20000])
            .build();
    }
};
SignalrService = __decorate([
    Injectable({ providedIn: "root" })
], SignalrService);
export { SignalrService };
//# sourceMappingURL=signalr.service.js.map