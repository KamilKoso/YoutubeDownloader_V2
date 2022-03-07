import { Component, Input, OnInit } from "@angular/core";
import { VideoMetadata } from "@app/core/models/video-metadata";
import { TimeSpan } from "@app/shared/helpers/timespan";

@Component({
  selector: "app-video-preview",
  templateUrl: "./video-preview.component.html",
  styleUrls: ["./video-preview.component.scss"],
})
export class VideoPreviewComponent implements OnInit {
  @Input() videoMetadata: VideoMetadata;

  constructor() {}

  ngOnInit() {}

  formatDuration(duration: TimeSpan): string {
    let result = "";

    // Days
    if (duration.days > 0) {
      result += duration.days;
      result += ":";
    }

    // Hours
    if (duration.hours > 0) {
      result += duration.hours;
      result += ":";
    }

    // Minutes
    if (duration.minutes > 0) {
      result += duration.minutes < 10 ? "0" + duration.minutes : duration.minutes;
      result += ":";
    } else {
      result += "00:";
    }

    // Seconds
    if (duration.seconds > 0) {
      result += duration.seconds < 10 ? "0" + duration.seconds : duration.seconds;
    } else {
      result += "00";
    }

    return result;
  }

  formatViews(views: number) {
    if (views > 1_000_000) {
      return Math.floor(views / 1_000_000) + "M views";
    } else if (views > 100_000) {
      return Math.floor(views / 1000) + "K views";
    } else if (views > 10_000) {
      return Math.floor(views / 1000) + "K views";
    } else {
      return views + " views";
    }
  }
}
