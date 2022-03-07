import { __decorate } from "tslib";
import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";
import { tap } from "rxjs/operators";
import { ThemeEnum } from "./themes-enum";
let ThemeService = class ThemeService {
    constructor() {
        var _a;
        this.localStorageThemeKey = "theme";
        this.body = document.getElementsByTagName("body")[0];
        this.themeSubject$ = new BehaviorSubject((_a = localStorage.getItem(this.localStorageThemeKey)) !== null && _a !== void 0 ? _a : ThemeEnum.Light);
        this.theme$ = this.themeSubject$
            .asObservable()
            .pipe(tap((theme) => (this.currentTheme = theme)));
        this.initializeTheme();
    }
    changeTheme(theme) {
        localStorage.setItem(this.localStorageThemeKey, theme);
        this.body.classList.remove(this.currentTheme);
        this.body.classList.add(theme);
        this.themeSubject$.next(theme);
    }
    initializeTheme() {
        this.theme$.subscribe((themeClass) => {
            this.body.classList.add(themeClass);
        });
    }
};
ThemeService = __decorate([
    Injectable({
        providedIn: "root",
    })
], ThemeService);
export { ThemeService };
//# sourceMappingURL=theme.service.js.map