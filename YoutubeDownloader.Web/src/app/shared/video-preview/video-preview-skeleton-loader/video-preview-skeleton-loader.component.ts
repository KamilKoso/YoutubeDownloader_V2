import { Component, Input, OnInit } from "@angular/core";
import { ThemeService } from "@app/core/services/theme/theme.service";
import { ThemeEnum } from "@app/core/services/theme/themes-enum";

@Component({
  selector: "app-video-preview-skeleton-loader",
  templateUrl: "./video-preview-skeleton-loader.component.html",
  styleUrls: ["./video-preview-skeleton-loader.component.scss"],
})
export class VideoPreviewSkeletonLoaderComponent {
  constructor() {}
}
