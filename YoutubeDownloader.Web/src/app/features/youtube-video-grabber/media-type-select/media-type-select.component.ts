import { ChangeDetectorRef, Component, EventEmitter, Input, OnDestroy, OnInit, Output } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { MediaTypesEnum } from "@app/core/models/enums/media-types.enum";
import { MediaTypeRequest } from "@app/core/models/media-type-request";
import { combineLatest, merge, Subscription } from "rxjs";
import { startWith } from "rxjs/operators";

@Component({
  selector: "app-media-type-select",
  templateUrl: "./media-type-select.component.html",
  styleUrls: ["./media-type-select.component.scss"],
})
export class MediaTypeSelectComponent implements OnInit, OnDestroy {
  @Input() videoQualityLabels: string[] = [];
  @Input() bitratesKilobytesPerSecond: number[] = [];
  @Input() submitButtonText: string = "Download !";
  @Input() videoIsProcessing: boolean = false;
  @Output() mediaTypeSelected: EventEmitter<MediaTypeRequest> = new EventEmitter();
  mediaTypes = MediaTypesEnum;
  selectedMediaType: number;
  validatorsSwitcher$: Subscription;
  formResetter$: Subscription;

  mediaTypeForm: FormGroup = this._fb.group({
    mediaType: [MediaTypesEnum.Audio, Validators.required],
    customBitrate: [false],
    videoQuality: [null],
    bitrate: [null, Validators.required],
  });

  constructor(private _fb: FormBuilder) {}

  ngOnInit() {
    this.setValidators();
    this.resetFormOnMediaTypeChange();
  }

  ngOnDestroy(): void {
    this.validatorsSwitcher$?.unsubscribe();
    this.formResetter$?.unsubscribe();
  }

  onSubmit() {
    if (this.mediaTypeForm.invalid) {
      return;
    }

    var result = this.mediaTypeForm.getRawValue();
    if (result.mediaType === MediaTypesEnum.Video && !result.customBitrate) {
      result.bitprate = null;
    }

    this.mediaTypeSelected.emit(result);
  }

  resetFormOnMediaTypeChange() {
    this.formResetter$ = this.mediaTypeForm.controls.mediaType.valueChanges.subscribe(() => {
      this.mediaTypeForm.controls.bitrate.reset();
      this.mediaTypeForm.controls.videoQuality.reset();
      this.mediaTypeForm.controls.customBitrate.reset();
    });
  }

  setValidators() {
    const bitrateControl = this.mediaTypeForm.controls.bitrate;
    const videoQualityControl = this.mediaTypeForm.controls.videoQuality;

    const customBitrateValueChanges$ = this.mediaTypeForm.controls.customBitrate.valueChanges.pipe(
      startWith(this.mediaTypeForm.value.customBitrate)
    );
    const mediaTypeValueChanges$ = this.mediaTypeForm.controls.mediaType.valueChanges.pipe(
      startWith(this.mediaTypeForm.value.mediaType)
    );

    this.validatorsSwitcher$ = combineLatest([customBitrateValueChanges$, mediaTypeValueChanges$]).subscribe(
      ([customBitrate, mediaType]) => {
        bitrateControl.clearValidators();
        videoQualityControl.clearValidators();

        if (mediaType === MediaTypesEnum.Video && customBitrate) {
          videoQualityControl.setValidators(Validators.required);
          bitrateControl.setValidators(Validators.required);
        } else if (mediaType === MediaTypesEnum.Audio) {
          bitrateControl.setValidators(Validators.required);
        } else if (mediaType === MediaTypesEnum.Video) {
          videoQualityControl.setValidators(Validators.required);
        }

        bitrateControl.updateValueAndValidity();
        videoQualityControl.updateValueAndValidity();
      }
    );
  }
}
