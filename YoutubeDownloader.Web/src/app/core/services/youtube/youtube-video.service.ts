import { HttpHeaderResponse, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { DomSanitizer } from "@angular/platform-browser";
import { GetAudioRequest } from "@app/core/models/get-audio-request";
import { GetVideoRequest } from "@app/core/models/get-video-request";
import { VideoMetadata } from "@app/core/models/video-metadata";
import { Observable, of } from "rxjs";
import { catchError, map, shareReplay } from "rxjs/operators";

import { ApiClientService } from "../api-client.service";

@Injectable({
  providedIn: "root",
})
export class YoutubeVideoService {
  constructor(private _apiClientService: ApiClientService, private _sanitizer: DomSanitizer) {}

  getVideoMetadata(youtubeVideoUrl: string): Observable<VideoMetadata> {
    return this._apiClientService
      .get<VideoMetadata>(`${appConfig.apiUrl}/youtubedownloader/metadata`, {
        queryParams: { videoUrl: youtubeVideoUrl },
      })
      .pipe(
        map((metadata: VideoMetadata) => {
          metadata.thumbnailUrl = this._sanitizer.bypassSecurityTrustUrl(metadata.thumbnailUrl);
          return metadata;
        })
      );
  }

  getAudio(audioRequest: GetAudioRequest): Observable<any> {
    return this._apiClientService.getBlob(`${appConfig.apiUrl}/youtubedownloader/get-audio`, {
      queryParams: audioRequest,
    });
  }

  getVideo(videoRequest: GetVideoRequest) {
    return this._apiClientService.getBlob(`${appConfig.apiUrl}/youtubedownloader/get-video`, {
      queryParams: videoRequest,
    });
  }
}
