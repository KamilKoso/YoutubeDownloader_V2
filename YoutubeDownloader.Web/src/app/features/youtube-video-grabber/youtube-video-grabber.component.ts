import { Component, OnInit } from "@angular/core";
import { FormControl, Validators } from "@angular/forms";
import { MediaTypesEnum } from "@app/core/models/enums/media-types.enum";
import { GetAudioRequest } from "@app/core/models/get-audio-request";
import { GetVideoRequest } from "@app/core/models/get-video-request";
import { MediaTypeRequest } from "@app/core/models/media-type-request";
import { VideoMetadata } from "@app/core/models/video-metadata";
import { indicate } from "@app/core/rxjs/indicate";
import { MediaProcessingProgressService } from "@app/core/services/youtube/media-processing-progress.service";
import { YoutubeVideoService } from "@app/core/services/youtube/youtube-video.service";
import { FileSaverService } from "ngx-filesaver";
import { Subject, Observable, merge, of, ReplaySubject } from "rxjs";
import { catchError, exhaustMap, filter, switchMapTo } from "rxjs/operators";

@Component({
  selector: "app-youtube-video-grabber",
  templateUrl: "./youtube-video-grabber.component.html",
  styleUrls: ["./youtube-video-grabber.component.scss"],
})
export class YoutubeVideoGrabberComponent implements OnInit {
  videoUrl = new FormControl(null, [
    Validators.required,
    Validators.pattern(
      /^((?:https?:)?\/\/)?((?:www|m)\.)?((?:youtube\.com|youtu.be))(\/(?:[\w\-]+\?v=|embed\/|v\/)?)([\w\-]+)(\S+)?$/
    ),
  ]);
  videoUrl$ = new Subject<string>();
  videoMetadata$: Observable<Nullable<VideoMetadata>>;
  isLoading$ = new ReplaySubject<boolean>(1);
  isVideoProcessing$ = new Subject<boolean>();
  progress$: Observable<number>;
  videoUrlValidationMessage = new Map([["pattern", "Incorrect youtube video url"]]);

  constructor(
    private _youtubeVideoService: YoutubeVideoService,
    private _mediaProcessingProgressService: MediaProcessingProgressService,
    private _fileSaverService: FileSaverService
  ) {}

  ngOnInit(): void {
    this.videoMetadata$ = merge(
      this.getVideoMetadata(),
      this.isLoading$.pipe(
        filter((x) => x),
        switchMapTo(of(null))
      )
    );
    this.progressReported();
  }

  getVideoMetadata(): Observable<Nullable<VideoMetadata>> {
    return this.videoUrl$.pipe(
      exhaustMap((videoUrl: string) =>
        this._youtubeVideoService.getVideoMetadata(videoUrl).pipe(
          indicate(this.isLoading$),
          catchError(() => of(null))
        )
      )
    );
  }

  async processMediaTypeRequest(mediaTypeRequest: MediaTypeRequest, author: string, title: string) {
    if (mediaTypeRequest.mediaType === MediaTypesEnum.Audio) {
      this.processAudioRequest({
        videoUrl: this.videoUrl.value,
        videoTitle: title,
        videoAuthor: author,
        bitrate: mediaTypeRequest.bitrate,
        signalrConnectionId: await this._mediaProcessingProgressService.getConnectionId(),
      });
    } else if (mediaTypeRequest.mediaType === MediaTypesEnum.Video) {
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

  processAudioRequest(request: GetAudioRequest) {
    this._youtubeVideoService
      .getAudio(request)
      .pipe(indicate(this.isVideoProcessing$))
      .subscribe((data: any) => {
        this._fileSaverService.save(data, `${request.videoAuthor} - ${request.videoTitle}.mp3`);
      });
  }

  processVideoRequest(request: GetVideoRequest) {
    this._youtubeVideoService
      .getVideo(request)
      .pipe(indicate(this.isVideoProcessing$))
      .subscribe((data: any) => {
        this._fileSaverService.save(data, `${request.videoAuthor} - ${request.videoTitle}.mp4`);
      });
  }

  progressReported() {
    this.progress$ = this._mediaProcessingProgressService.progressReported();
  }
}
