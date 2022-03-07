import { Component, OnDestroy, OnInit } from "@angular/core";
import { AudioDownloadHistory } from "@app/core/models/audio-download-history";
import { Page } from "@app/core/models/page";
import { SearchCriteria } from "@app/core/models/search-criteria";
import { VideoDownloadHistory } from "@app/core/models/video-download-history";
import { indicate } from "@app/core/rxjs/indicate";
import { DownloadHistoryService } from "@app/core/services/youtube/download-history.service";
import { Subject, BehaviorSubject, Observable } from "rxjs";
import { takeUntil, exhaustMap, scan, finalize } from "rxjs/operators";

@Component({
  selector: "app-video-download-history",
  templateUrl: "./video-download-history.component.html",
  styleUrls: ["./video-download-history.component.scss"],
})
export class VideoDownloadHistoryComponent implements OnInit, OnDestroy {
  searchCriteria: SearchCriteria = {
    pageNumber: 1,
    orderBy: "CreatedOn",
    isAscending: false,
    pageSize: 20,
  };

  destroySubject = new Subject<void>();
  searchCriteria$ = new BehaviorSubject(this.searchCriteria);
  videoHistory$: Observable<VideoDownloadHistory[]>;
  videoHistoryLoading$ = new BehaviorSubject<boolean>(true);

  constructor(private downloadHistoryService: DownloadHistoryService) {}

  ngOnInit(): void {
    this.getvideoHistory();
  }

  ngOnDestroy(): void {
    this.destroySubject.next();
    this.destroySubject.complete();
  }

  getvideoHistory(): void {
    this.videoHistory$ = this.searchCriteria$.pipe(
      takeUntil(this.destroySubject),
      exhaustMap((searchCriteria: SearchCriteria) => {
        return this.downloadHistoryService
          .getVideoDownloadHistory(searchCriteria)
          .pipe(indicate(this.videoHistoryLoading$));
      }),
      scan((acc: VideoDownloadHistory[], page: Page<VideoDownloadHistory>) => {
        if (acc.length + page.items.length === page.totalCount) {
          this.destroySubject.next();
          this.destroySubject.complete();
        } else {
          this.searchCriteria.pageNumber++;
        }
        return [...acc, ...page.items];
      }, []),
      finalize(() => {})
    );
  }

  scrollOffsetHit() {
    this.searchCriteria$.next(this.searchCriteria);
  }
}
