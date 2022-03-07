import { Injectable } from "@angular/core";
import { DomSanitizer } from "@angular/platform-browser";
import { AudioDownloadHistory } from "@app/core/models/audio-download-history";
import { Page } from "@app/core/models/page";
import { SearchCriteria } from "@app/core/models/search-criteria";
import { VideoDownloadHistory } from "@app/core/models/video-download-history";
import { Observable } from "rxjs";
import { map, shareReplay } from "rxjs/operators";
import { ApiClientService } from "../api-client.service";

@Injectable({
  providedIn: "root",
})
export class DownloadHistoryService {
  constructor(private _apiClientService: ApiClientService, private _sanitizer: DomSanitizer) {}

  getAudioDownloadHistory(searchCriteria: SearchCriteria): Observable<Page<AudioDownloadHistory>> {
    return this._apiClientService
      .get<Page<AudioDownloadHistory>>(`${appConfig.apiUrl}/downloadhistory/audio`, { queryParams: searchCriteria })
      .pipe(
        map((page: Page<AudioDownloadHistory>) => {
          page.items.forEach((history) => {
            history.videoMetadata.thumbnailUrl = this._sanitizer.bypassSecurityTrustUrl(
              history.videoMetadata.thumbnailUrl
            );
          });
          return page;
        })
      );
  }

  getVideoDownloadHistory(searchCriteria: SearchCriteria): Observable<Page<VideoDownloadHistory>> {
    return this._apiClientService
      .get<Page<VideoDownloadHistory>>(`${appConfig.apiUrl}/downloadhistory/video`, { queryParams: searchCriteria })
      .pipe(
        map((page: Page<VideoDownloadHistory>) => {
          page.items.forEach((history) => {
            history.videoMetadata.thumbnailUrl = this._sanitizer.bypassSecurityTrustUrl(
              history.videoMetadata.thumbnailUrl
            );
          });
          return page;
        })
      );
  }
}
