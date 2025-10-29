import { AbstractControl, ValidationErrors, ValidatorFn, FormArray } from '@angular/forms';

export function minMaxSelectedArray(min = 1, max = 5): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    if (control instanceof FormArray) {
      const count = control.length;
      if (count < min) {
        return { minSelected: { required: min, actual: count } };
      }
      if (count > max) {
        return { maxSelected: { required: max, actual: count } };
      }
    }
    return null;
  };
}
