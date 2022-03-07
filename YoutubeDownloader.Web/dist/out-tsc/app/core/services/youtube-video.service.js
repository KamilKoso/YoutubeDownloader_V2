import { __decorate } from "tslib";
import { Injectable } from "@angular/core";
let YoutubeVideoService = class YoutubeVideoService {
    constructor(_apiClientService) {
        this._apiClientService = _apiClientService;
    }
    getVideoMetadata(youtubeVideoUrl) {
        return this._apiClientService.get(`${appConfig.apiUrl}/youtubevideos/metadata`, { queryParams: { videoUrl: youtubeVideoUrl } });
    }
};
YoutubeVideoService = __decorate([
    Injectable({
        providedIn: "root"
    })
], YoutubeVideoService);
export { YoutubeVideoService };
//# sourceMappingURL=youtube-video.service.js.map