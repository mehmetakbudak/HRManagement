import { FormGroup } from "@angular/forms";

export class ValidateHelper {

  valid(form: FormGroup) {
    for (const i in form.controls) {
      form.controls[i].markAsDirty();
      form.controls[i].updateValueAndValidity();
    }
  }


}
