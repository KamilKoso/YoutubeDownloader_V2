import { Directive, Output, Input, HostListener, EventEmitter } from "@angular/core";

@Directive({
  // tslint:disable-next-line: directive-selector
  selector: "[scrollOffsetPercentage]",
})
export class WindowScrollOffsetDirective {
  @Output() windowScrollOffset = new EventEmitter<number>();
  @Input() scrollOffsetPercentage: number;

  // <summary>
  // Event listener for scroll event on the specific ui element
  // </summary>
  @HostListener("window:scroll", ["$event"])
  onListenerTriggered(): void {
    const scrollTop = document.documentElement.scrollTop;
    const scrollHeight = document.documentElement.scrollHeight;
    const clientHeight = document.documentElement.clientHeight;
    const percent = Math.round((scrollTop / (scrollHeight - clientHeight)) * 100);
    if (this.scrollOffsetPercentage <= percent) {
      this.windowScrollOffset.emit(percent);
    }
  }
}
