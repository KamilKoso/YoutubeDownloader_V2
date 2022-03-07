import { __decorate } from "tslib";
import { Component, EventEmitter, Input, Output } from "@angular/core";
import { Validators } from "@angular/forms";
import { MediaTypesEnum } from "@app/core/models/enums/media-types.enum";
import { combineLatest } from "rxjs";
import { startWith } from "rxjs/operators";
let MediaTypeSelectComponent = class MediaTypeSelectComponent {
    constructor(_fb) {
        this._fb = _fb;
        this.videoQualityLabels = [];
        this.bitratesKilobytesPerSecond = [];
        this.submitButtonText = "Download !";
        this.videoIsProcessing = false;
        this.mediaTypeSelected = new EventEmitter();
        this.mediaTypes = MediaTypesEnum;
        this.mediaTypeForm = this._fb.group({
            mediaType: [MediaTypesEnum.Audio, Validators.required],
            customBitrate: [false],
            videoQuality: [null],
            bitrate: [null, Validators.required],
        });
    }
    ngOnInit() {
        this.setValidators();
        this.resetFormOnMediaTypeChange();
    }
    ngOnDestroy() {
        var _a, _b;
        (_a = this.validatorsSwitcher$) === null || _a === void 0 ? void 0 : _a.unsubscribe();
        (_b = this.formResetter$) === null || _b === void 0 ? void 0 : _b.unsubscribe();
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
        const customBitrateValueChanges$ = this.mediaTypeForm.controls.customBitrate.valueChanges.pipe(startWith(this.mediaTypeForm.value.customBitrate));
        const mediaTypeValueChanges$ = this.mediaTypeForm.controls.mediaType.valueChanges.pipe(startWith(this.mediaTypeForm.value.mediaType));
        this.validatorsSwitcher$ = combineLatest([customBitrateValueChanges$, mediaTypeValueChanges$]).subscribe(([customBitrate, mediaType]) => {
            bitrateControl.clearValidators();
            videoQualityControl.clearValidators();
            if (mediaType === MediaTypesEnum.Video && customBitrate) {
                videoQualityControl.setValidators(Validators.required);
                bitrateControl.setValidators(Validators.required);
            }
            else if (mediaType === MediaTypesEnum.Audio) {
                bitrateControl.setValidators(Validators.required);
            }
            else if (mediaType === MediaTypesEnum.Video) {
                videoQualityControl.setValidators(Validators.required);
            }
            bitrateControl.updateValueAndValidity();
            videoQualityControl.updateValueAndValidity();
        });
    }
};
__decorate([
    Input()
], MediaTypeSelectComponent.prototype, "videoQualityLabels", void 0);
__decorate([
    Input()
], MediaTypeSelectComponent.prototype, "bitratesKilobytesPerSecond", void 0);
__decorate([
    Input()
], MediaTypeSelectComponent.prototype, "submitButtonText", void 0);
__decorate([
    Input()
], MediaTypeSelectComponent.prototype, "videoIsProcessing", void 0);
__decorate([
    Output()
], MediaTypeSelectComponent.prototype, "mediaTypeSelected", void 0);
MediaTypeSelectComponent = __decorate([
    Component({
        selector: "app-media-type-select",
        templateUrl: "./media-type-select.component.html",
        styleUrls: ["./media-type-select.component.scss"],
    })
], MediaTypeSelectComponent);
export { MediaTypeSelectComponent };
//# sourceMappingURL=media-type-select.component.js.map