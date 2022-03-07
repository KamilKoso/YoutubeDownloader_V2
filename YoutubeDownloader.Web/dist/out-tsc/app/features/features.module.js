import { __decorate } from "tslib";
import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { MaterialModule } from "@app/material.module";
import { SharedModule } from "@app/shared/shared.module";
import { LogoutModule } from "./account/logout/logout.module";
import { RegisterModule } from "./account/register/register.module";
import { FeaturesRoutingModule } from "./features-routing.module";
import { YoutubeVideoGrabberComponent } from "./youtube-video-grabber/youtube-video-grabber.component";
import { DownloadHistoryComponent } from "./download-history/download-history.component";
import { AudioDownloadHistoryComponent } from "./download-history/audio-download-history/audio-download-history.component";
import { VideoDownloadHistoryComponent } from "./download-history/video-download-history/video-download-history.component";
import { MediaTypeSelectComponent } from "./youtube-video-grabber/media-type-select/media-type-select.component";
import { MediaTypeSelectSkeletonLoaderComponent } from "./youtube-video-grabber/media-type-select/media-type-select-skeleton-loader/media-type-select-skeleton-loader.component";
let FeaturesModule = class FeaturesModule {
};
FeaturesModule = __decorate([
    NgModule({
        imports: [CommonModule, FeaturesRoutingModule, MaterialModule, RegisterModule, LogoutModule, SharedModule],
        declarations: [
            YoutubeVideoGrabberComponent,
            DownloadHistoryComponent,
            AudioDownloadHistoryComponent,
            VideoDownloadHistoryComponent,
            MediaTypeSelectComponent,
            MediaTypeSelectSkeletonLoaderComponent,
        ],
        exports: [YoutubeVideoGrabberComponent],
    })
], FeaturesModule);
export { FeaturesModule };
//# sourceMappingURL=features.module.js.map