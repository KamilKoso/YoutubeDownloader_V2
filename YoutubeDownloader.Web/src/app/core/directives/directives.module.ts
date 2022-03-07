import { NgModule } from "@angular/core";
import { PasswordShowDirective } from "./password-show.directive";
import { WindowScrollOffsetDirective } from "./window-scroll-offset.directive";
import { NgVarDirective } from "./ng-var.directive";

const directives = [WindowScrollOffsetDirective, PasswordShowDirective, NgVarDirective];

@NgModule({
  declarations: directives,
  exports: directives,
})
export class DirectivesModule {}
