import { __decorate } from "tslib";
import { Component } from "@angular/core";
import { indicate } from "@app/core/rxjs/indicate";
import { Subject, BehaviorSubject } from "rxjs";
import { takeUntil, exhaustMap, scan, finalize } from "rxjs/operators";
let VideoDownloadHistoryComponent = class VideoDownloadHistoryComponent {
    constructor(downloadHistoryService) {
        this.downloadHistoryService = downloadHistoryService;
        this.searchCriteria = {
            pageNumber: 1,
            orderBy: "CreatedOn",
            isAscending: false,
            pageSize: 20,
        };
        this.destroySubject = new Subject();
        this.searchCriteria$ = new BehaviorSubject(this.searchCriteria);
        this.videoHistoryLoading$ = new BehaviorSubject(true);
    }
    ngOnInit() {
        this.getvideoHistory();
    }
    ngOnDestroy() {
        this.destroySubject.next();
        this.destroySubject.complete();
    }
    getvideoHistory() {
        this.videoHistory$ = this.searchCriteria$.pipe(takeUntil(this.destroySubject), exhaustMap((searchCriteria) => {
            return this.downloadHistoryService
                .getVideoDownloadHistory(searchCriteria)
                .pipe(indicate(this.videoHistoryLoading$));
        }), scan((acc, page) => {
            if (acc.length + page.items.length === page.totalCount) {
                this.destroySubject.next();
                this.destroySubject.complete();
            }
            else {
                this.searchCriteria.pageNumber++;
            }
            return [...acc, ...page.items];
        }, []), finalize(() => { }));
    }
    scrollOffsetHit() {
        this.searchCriteria$.next(this.searchCriteria);
    }
};
VideoDownloadHistoryComponent = __decorate([
    Component({
        selector: "app-video-download-history",
        templateUrl: "./video-download-history.component.html",
        styleUrls: ["./video-download-history.component.scss"],
    })
], VideoDownloadHistoryComponent);
export { VideoDownloadHistoryComponent };
//# sourceMappingURL=video-download-history.component.js.map