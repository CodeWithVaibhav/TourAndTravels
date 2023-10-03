import { Injectable, Output, EventEmitter } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseHttpService } from './base-http.service';
import { StorageService } from './storage.service';
import { UntypedFormArray, UntypedFormGroup } from '@angular/forms';

@Injectable({ providedIn: "root" })
export class UtilService extends BaseHttpService {

  constructor() {
    super();
  }

  markFormGroupTouched(formGroup: UntypedFormGroup) {
    Object.keys(formGroup.controls).map(ctrl => {
      if (formGroup.controls[ctrl].status !== 'DISABLED') {
        formGroup.controls[ctrl].markAsTouched();
      }
      if (formGroup.controls[ctrl] instanceof UntypedFormGroup) {
        Object.keys((formGroup.controls[ctrl] as UntypedFormGroup).controls).map(c => {
          if ((formGroup.controls[ctrl] as UntypedFormGroup).controls[c] instanceof UntypedFormGroup) {
            this.markFormGroupTouched((formGroup.controls[ctrl] as UntypedFormGroup).controls[c] as UntypedFormGroup);
          } else {
            if ((formGroup.controls[ctrl] as UntypedFormGroup).controls[c].status !== 'DISABLED') {
              (formGroup.controls[ctrl] as UntypedFormGroup).controls[c].markAsTouched();
            }
          }
        });
      } else if (formGroup.controls[ctrl] instanceof UntypedFormArray) {
        Object.keys((formGroup.controls[ctrl] as UntypedFormArray).controls).map(c => {
          this.markFormGroupTouched((formGroup.controls[ctrl] as UntypedFormArray).controls[c] as UntypedFormGroup);
        });
      }
    });
  }
}
