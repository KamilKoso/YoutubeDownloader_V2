import { __decorate } from "tslib";
import { Component } from "@angular/core";
import { FormControl, Validators } from "@angular/forms";
import { MediaTypesEnum } from "@app/core/models/enums/media-types.enum";
import { indicate } from "@app/core/rxjs/indicate";
import { Subject, merge, of, ReplaySubject } from "rxjs";
import { catchError, exhaustMap, filter, switchMapTo } from "rxjs/operators";
let YoutubeVideoGrabberComponent = class YoutubeVideoGrabberComponent {
    constructor(_youtubeVideoService, _mediaProcessingProgressService, _fileSaverService) {
        this._youtubeVideoService = _youtubeVideoService;
        this._mediaProcessingProgressService = _mediaProcessingProgressService;
        this._fileSaverService = _fileSaverService;
        this.videoUrl = new FormControl(null, [
            Validators.required,
            Validators.pattern(/^((?:https?:)?\/\/)?((?:www|m)\.)?((?:youtube\.com|youtu.be))(\/(?:[\w\-]+\?v=|embed\/|v\/)?)([\w\-]+)(\S+)?$/),
        ]);
        this.videoUrl$ = new Subject();
        this.isLoading$ = new ReplaySubject(1);
        this.isVideoProcessing$ = new Subject();
        this.videoUrlValidationMessage = new Map([["pattern", "Incorrect youtube video url"]]);
    }
    ngOnInit() {
        this.videoMetadata$ = merge(this.getVideoMetadata(), this.isLoading$.pipe(filter((x) => x), switchMapTo(of(null))));
        this.progressReported();
    }
    getVideoMetadata() {
        return this.videoUrl$.pipe(exhaustMap((videoUrl) => this._youtubeVideoService.getVideoMetadata(videoUrl).pipe(indicate(this.isLoading$), catchError(() => of(null)))));
    }
    async processMediaTypeRequest(mediaTypeRequest, author, title) {
        if (mediaTypeRequest.mediaType === MediaTypesEnum.Audio) {
            this.processAudioRequest({
                videoUrl: this.videoUrl.value,
                videoTitle: title,
                videoAuthor: author,
                bitrate: mediaTypeRequest.bitrate,
                signalrConnectionId: await this._mediaProcessingProgressService.getConnectionId(),
            });
        }
        else if (mediaTypeRequest.mediaType === MediaTypesEnum.Video) {
            this.processVideoRequest({
                videoUrl: this.videoUrl.value,
                videoQualityLabel: mediaTypeRequest.videoQuality,
                videoTitle: title,
                videoAuthor: author,
                bitrate: mediaTypeRequest.bitrate,
                signalrConnectionId: await this._mediaProcessingProgressService.getConnectionId(),
            });
        }
    }
    processAudioRequest(request) {
        this._youtubeVideoService
            .getAudio(request)
            .pipe(indicate(this.isVideoProcessing$))
            .subscribe((data) => {
            this._fileSaverService.save(data, `${request.videoAuthor} - ${request.videoTitle}.mp3`);
        });
    }
    processVideoRequest(request) {
        this._youtubeVideoService
            .getVideo(request)
            .pipe(indicate(this.isVideoProcessing$))
            .subscribe((data) => {
            this._fileSaverService.save(data, `${request.videoAuthor} - ${request.videoTitle}.mp4`);
        });
    }
    progressReported() {
        this.progress$ = this._mediaProcessingProgressService.progressReported();
    }
};
YoutubeVideoGrabberComponent = __decorate([
    Component({
        selector: "app-youtube-video-grabber",
        templateUrl: "./youtube-video-grabber.component.html",
        styleUrls: ["./youtube-video-grabber.component.scss"],
    })
], YoutubeVideoGrabberComponent);
export { YoutubeVideoGrabberComponent };
//# sourceMappingURL=youtube-video-grabber.component.js.map