import { __decorate } from "tslib";
import { Injectable } from "@angular/core";
import { map } from "rxjs/operators";
let YoutubeVideoService = class YoutubeVideoService {
    constructor(_apiClientService, _sanitizer) {
        this._apiClientService = _apiClientService;
        this._sanitizer = _sanitizer;
    }
    getVideoMetadata(youtubeVideoUrl) {
        return this._apiClientService
            .get(`${appConfig.apiUrl}/youtubedownloader/metadata`, {
            queryParams: { videoUrl: youtubeVideoUrl },
        })
            .pipe(map((metadata) => {
            metadata.thumbnailUrl = this._sanitizer.bypassSecurityTrustUrl(metadata.thumbnailUrl);
            return metadata;
        }));
    }
    getAudio(audioRequest) {
        return this._apiClientService.getBlob(`${appConfig.apiUrl}/youtubedownloader/get-audio`, {
            queryParams: audioRequest,
        });
    }
    getVideo(videoRequest) {
        return this._apiClientService.getBlob(`${appConfig.apiUrl}/youtubedownloader/get-video`, {
            queryParams: videoRequest,
        });
    }
};
YoutubeVideoService = __decorate([
    Injectable({
        providedIn: "root",
    })
], YoutubeVideoService);
export { YoutubeVideoService };
//# sourceMappingURL=youtube-video.service.js.map