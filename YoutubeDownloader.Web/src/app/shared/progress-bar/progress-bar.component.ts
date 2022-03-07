import { Component, Input } from "@angular/core";

@Component({
  // tslint:disable-next-line: component-selector
  selector: "progress-bar",
  templateUrl: "./progress-bar.component.html",
  styleUrls: ["./progress-bar.component.scss"],
})
export class ProgressBarComponent {
  @Input() progress = 0;
  @Input() height: string = "50px";
  @Input() progressBarText: Nullable<string>;
  @Input() mode: "text" | "text-percent" | "percent";

  constructor() {}
}
