import { Directive, HostListener, Input } from "@angular/core";

@Directive({
  selector: "[passwordShowDirective]",
})
export class PasswordShowDirective {
  @Input() passwordShowDirective: any;

  @HostListener("mouseup")
  onMouseOver(): void {
    this.passwordShowDirective.type = "password";
  }

  @HostListener("mouseleave")
  onMouseLeave(): void {
    this.passwordShowDirective.type = "password";
  }

  @HostListener("mousedown")
  onMouseDown(): void {
    this.passwordShowDirective.type = "text";
  }
}
