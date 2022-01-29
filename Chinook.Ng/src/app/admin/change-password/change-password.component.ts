import { Component, OnInit } from "@angular/core";
import { AppService } from "src/app/app.service";
import { AlertService, alertType } from "src/app/services/alert.service";
import notify from "devextreme/ui/notify";

export class ChangePassword {
  oldPassword: string;
  newPassword: string;
  reNewPassword: string;
}

@Component({
  selector: "app-change-password",
  templateUrl: "./change-password.component.html",
  styleUrls: ["./change-password.component.css"],
})
export class ChangePasswordComponent implements OnInit {
  changePassword = new ChangePassword();

  constructor(
    private alertService: AlertService,
    private appService: AppService
  ) {}

  ngOnInit() {}

  save(e) {
    const result = e.validationGroup.validate();
    if (result.isValid) {
      if (
        this.changePassword.newPassword !== this.changePassword.reNewPassword
      ) {
        notify(
          "Şifre alanları uyuşmamaktadır.",
          alertType[alertType.error],
          1000
        );
        return;
      }
      this.appService
        .post(this.appService.user + "/changePassword", this.changePassword)
        .then(
          (res) => {
            notify(
              "Şifre başarıyla güncellendi.",
              alertType[alertType.success],
              1000
            );
            this.changePassword = new ChangePassword();
          },
          (error) => {
            notify(error, alertType[alertType.error], 1000);
          }
        );
    }
  }
}
