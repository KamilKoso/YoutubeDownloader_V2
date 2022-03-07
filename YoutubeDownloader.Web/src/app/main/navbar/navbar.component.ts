import { Component, OnInit } from "@angular/core";
import { AuthService } from "@app/auth";
import { ThemeService } from "@app/core/services/theme/theme.service";
import { faYoutube } from "@fortawesome/free-brands-svg-icons";
import { Observable } from "rxjs";

@Component({
  selector: "app-navbar",
  templateUrl: "./navbar.component.html",
  styleUrls: ["./navbar.component.scss"],
})
export class NavbarComponent implements OnInit {
  faYoutube = faYoutube;
  authStatus$!: Observable<boolean>;

  constructor(private _authService: AuthService) {}

  signout(): void {
    this._authService.signout();
  }

  manageAccount(): void {
    this._authService.manageAccount();
  }

  ngOnInit(): void {
    this.authStatus$ = this._authService.authStatus$;
  }

  getUserName(): Nullable<string> {
    return this._authService.name;
  }

  login() {
    this._authService.login();
  }
}
