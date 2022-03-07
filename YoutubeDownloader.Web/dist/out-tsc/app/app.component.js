import { __decorate } from "tslib";
import { Component } from "@angular/core";
import { ThemeEnum } from "./core/services/theme/themes-enum";
let AppComponent = class AppComponent {
    constructor(_mediaProcessingProgressService, _themeService) {
        this._mediaProcessingProgressService = _mediaProcessingProgressService;
        this._themeService = _themeService;
    }
    changeTheme(isDarkTheme) {
        if (!isDarkTheme) {
            this._themeService.changeTheme(ThemeEnum.Dark);
        }
        else {
            this._themeService.changeTheme(ThemeEnum.Light);
        }
    }
};
AppComponent = __decorate([
    Component({
        selector: "app-root",
        templateUrl: "./app.component.html",
        styleUrls: ["./app.component.scss"],
    })
], AppComponent);
export { AppComponent };
//# sourceMappingURL=app.component.js.map