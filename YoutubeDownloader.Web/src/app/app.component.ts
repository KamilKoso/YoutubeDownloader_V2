import { Component } from "@angular/core";
import { MatSlideToggleChange } from "@angular/material/slide-toggle";
import { ThemeService } from "./core/services/theme/theme.service";
import { ThemeEnum } from "./core/services/theme/themes-enum";
import { MediaProcessingProgressService } from "./core/services/youtube/media-processing-progress.service";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.scss"],
})
export class AppComponent {
  constructor(
    public _mediaProcessingProgressService: MediaProcessingProgressService,
    public _themeService: ThemeService
  ) {}

  changeTheme(isDarkTheme: boolean) {
    if (!isDarkTheme) {
      this._themeService.changeTheme(ThemeEnum.Dark);
    } else {
      this._themeService.changeTheme(ThemeEnum.Light);
    }
  }
}
