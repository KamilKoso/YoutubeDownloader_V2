import { Component, OnInit } from "@angular/core";
import { AudioDownloadHistory } from "@app/core/models/audio-download-history";
import { Page } from "@app/core/models/page";
import { SearchCriteria } from "@app/core/models/search-criteria";
import { indicate } from "@app/core/rxjs/indicate";
import { DownloadHistoryService } from "@app/core/services/youtube/download-history.service";
import { Subject, BehaviorSubject, Observable } from "rxjs";
import { exhaustMap, finalize, scan, takeUntil, tap } from "rxjs/operators";

@Component({
  selector: "app-audio-download-history",
  templateUrl: "./audio-download-history.component.html",
  styleUrls: ["./audio-download-history.component.scss"],
})
export class AudioDownloadHistoryComponent implements OnInit {
  searchCriteria: SearchCriteria = {
    pageNumber: 1,
    orderBy: "CreatedOn",
    isAscending: false,
    pageSize: 20,
  };

  destroySubject = new Subject<void>();
  searchCriteria$ = new BehaviorSubject(this.searchCriteria);
  audioHistory$: Observable<AudioDownloadHistory[]>;
  audioHistoryLoading$ = new BehaviorSubject<boolean>(true);

  constructor(private downloadHistoryService: DownloadHistoryService) {}

  ngOnInit(): void {
    this.getAudioHistory();
  }

  ngOnDestroy(): void {
    this.destroySubject.next();
    this.destroySubject.complete();
  }

  getAudioHistory(): void {
    this.audioHistory$ = this.searchCriteria$.pipe(
      takeUntil(this.destroySubject),
      exhaustMap((searchCriteria: SearchCriteria) => {
        return this.downloadHistoryService
          .getAudioDownloadHistory(searchCriteria)
          .pipe(indicate(this.audioHistoryLoading$));
      }),
      scan((acc: AudioDownloadHistory[], page: Page<AudioDownloadHistory>) => {
        if (acc.length + page.items.length === page.totalCount) {
          this.destroySubject.next();
          this.destroySubject.complete();
        } else {
          this.searchCriteria.pageNumber++;
        }
        return [...acc, ...page.items];
      }, [])
    );
  }

  scrollOffsetHit() {
    this.searchCriteria$.next(this.searchCriteria);
  }
}
