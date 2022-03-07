import { __decorate } from "tslib";
import { Injectable } from "@angular/core";
import { map } from "rxjs/operators";
let DownloadHistoryService = class DownloadHistoryService {
    constructor(_apiClientService, _sanitizer) {
        this._apiClientService = _apiClientService;
        this._sanitizer = _sanitizer;
    }
    getAudioDownloadHistory(searchCriteria) {
        return this._apiClientService
            .get(`${appConfig.apiUrl}/downloadhistory/audio`, { queryParams: searchCriteria })
            .pipe(map((page) => {
            page.items.forEach((history) => {
                history.videoMetadata.thumbnailUrl = this._sanitizer.bypassSecurityTrustUrl(history.videoMetadata.thumbnailUrl);
            });
            return page;
        }));
    }
    getVideoDownloadHistory(searchCriteria) {
        return this._apiClientService
            .get(`${appConfig.apiUrl}/downloadhistory/video`, { queryParams: searchCriteria })
            .pipe(map((page) => {
            page.items.forEach((history) => {
                history.videoMetadata.thumbnailUrl = this._sanitizer.bypassSecurityTrustUrl(history.videoMetadata.thumbnailUrl);
            });
            return page;
        }));
    }
};
DownloadHistoryService = __decorate([
    Injectable({
        providedIn: "root",
    })
], DownloadHistoryService);
export { DownloadHistoryService };
//# sourceMappingURL=download-history.service.js.map