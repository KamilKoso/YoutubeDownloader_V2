import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";
import { tap } from "rxjs/operators";
import { ThemeEnum } from "./themes-enum";

@Injectable({
  providedIn: "root",
})
export class ThemeService {
  private currentTheme: string;
  private localStorageThemeKey = "theme";
  private body = document.getElementsByTagName("body")[0];
  private themeSubject$: BehaviorSubject<string> = new BehaviorSubject(
    localStorage.getItem(this.localStorageThemeKey) ?? ThemeEnum.Light
  );

  public theme$: Observable<string> = this.themeSubject$
    .asObservable()
    .pipe(tap((theme) => (this.currentTheme = theme)));

  constructor() {
    this.initializeTheme();
  }

  changeTheme(theme: string) {
    localStorage.setItem(this.localStorageThemeKey, theme);
    this.body.classList.remove(this.currentTheme);
    this.body.classList.add(theme);
    this.themeSubject$.next(theme);
  }

  private initializeTheme() {
    this.theme$.subscribe((themeClass: string) => {
      this.body.classList.add(themeClass);
    });
  }
}
