import { __decorate } from "tslib";
import { Injectable } from "@angular/core";
let YoutubeVideoService = class YoutubeVideoService {
    constructor(_apiClientService) {
        this._apiClientService = _apiClientService;
    }
    getVideoMetadata(youtubeVideoUrl) {
        return this._apiClientService.get(`${appConfig.apiUrl}/youtubevideos/metadata`, { queryParams: { youtubeVideoUrl } });
    }
};
YoutubeVideoService = __decorate([
    Injectable({
        providedIn: "root"
    })
], YoutubeVideoService);
export { YoutubeVideoService };
//# sourceMappingURL=api-sample.service.js.map