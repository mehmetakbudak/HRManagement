import { Component, OnInit } from '@angular/core';
import { AppService } from 'src/app/app.service';
import { AlertService, alertType } from 'src/app/services/alert.service';
import notify from 'devextreme/ui/notify';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit {
  formData: any = {};

  constructor(private alertService: AlertService,
    private appService: AppService) { }

  ngOnInit() { }

  save(e) {
    e.preventDefault();
    if (this.formData.newPassword !== this.formData.reNewPassword) {
      notify("Şifre alanları uyuşmamaktadır.", alertType[alertType.error], 1000);
      return;
    }
    this.appService.post(this.appService.user + "/changePassword", this.formData).then(res => {
      notify("Şifre başarıyla güncellendi.", alertType[alertType.success], 1000);
      this.formData = {};
    }, (error) => {
      notify(error, alertType[alertType.error], 1000);
    });
  }
}
