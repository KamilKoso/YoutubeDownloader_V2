import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MaterialModule } from "@app/material.module";
import { FontAwesomeModule } from "@fortawesome/angular-fontawesome";
import { FileSaverModule } from "ngx-filesaver";

import { ProgressBarComponent } from "./progress-bar/progress-bar.component";
import { ValidationFeedbackComponent } from "./validation-feedback/validation-feedback.component";
import { VideoPreviewComponent } from "./video-preview/video-preview.component";
import { DirectivesModule } from "../core/directives/directives.module";
import { NgxSkeletonLoaderModule } from "ngx-skeleton-loader";
import { VideoPreviewSkeletonLoaderComponent } from "./video-preview/video-preview-skeleton-loader/video-preview-skeleton-loader.component";

@NgModule({
  imports: [
    CommonModule,
    MaterialModule,
    FontAwesomeModule,
    FormsModule,
    ReactiveFormsModule,
    FileSaverModule,
    NgxSkeletonLoaderModule.forRoot({
      animation: "progress",
    }),
  ],
  declarations: [
    ValidationFeedbackComponent,
    ProgressBarComponent,
    VideoPreviewComponent,
    VideoPreviewSkeletonLoaderComponent,
  ],
  exports: [
    ValidationFeedbackComponent,
    ProgressBarComponent,
    MaterialModule,
    FontAwesomeModule,
    FormsModule,
    ReactiveFormsModule,
    FileSaverModule,
    VideoPreviewComponent,
    DirectivesModule,
    NgxSkeletonLoaderModule,
    VideoPreviewSkeletonLoaderComponent,
  ],
})
export class SharedModule {}
