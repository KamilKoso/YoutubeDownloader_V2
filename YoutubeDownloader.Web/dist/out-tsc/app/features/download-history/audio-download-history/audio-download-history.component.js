import { __decorate } from "tslib";
import { Component } from "@angular/core";
import { indicate } from "@app/core/rxjs/indicate";
import { Subject, BehaviorSubject } from "rxjs";
import { exhaustMap, scan, takeUntil } from "rxjs/operators";
let AudioDownloadHistoryComponent = class AudioDownloadHistoryComponent {
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
        this.audioHistoryLoading$ = new BehaviorSubject(true);
    }
    ngOnInit() {
        this.getAudioHistory();
    }
    ngOnDestroy() {
        this.destroySubject.next();
        this.destroySubject.complete();
    }
    getAudioHistory() {
        this.audioHistory$ = this.searchCriteria$.pipe(takeUntil(this.destroySubject), exhaustMap((searchCriteria) => {
            return this.downloadHistoryService
                .getAudioDownloadHistory(searchCriteria)
                .pipe(indicate(this.audioHistoryLoading$));
        }), scan((acc, page) => {
            if (acc.length + page.items.length === page.totalCount) {
                this.destroySubject.next();
                this.destroySubject.complete();
            }
            else {
                this.searchCriteria.pageNumber++;
            }
            return [...acc, ...page.items];
        }, []));
    }
    scrollOffsetHit() {
        this.searchCriteria$.next(this.searchCriteria);
    }
};
AudioDownloadHistoryComponent = __decorate([
    Component({
        selector: "app-audio-download-history",
        templateUrl: "./audio-download-history.component.html",
        styleUrls: ["./audio-download-history.component.scss"],
    })
], AudioDownloadHistoryComponent);
export { AudioDownloadHistoryComponent };
//# sourceMappingURL=audio-download-history.component.js.map