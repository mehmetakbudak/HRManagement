import { Component, OnInit } from "@angular/core";
import { AlertService, alertType } from "src/app/services/alert.service";
import { AppService } from "src/app/app.service";
import { LookupService, Lookup } from "src/app/services/lookup.service";
import notify from "devextreme/ui/notify";
import { Urls } from "src/app/models/consts";
export class User {
  id: number;
  emailAddress: string;
  firstName: string;
  lastName: string;
  titleId: number;
  provinceId: number;
  cityId: number;
  address: string;
  birthDate: Date;
  hireDate: Date;
  isActive: boolean;
  phone: string;
}

@Component({
  selector: "app-profile",
  templateUrl: "./profile.component.html",
  styleUrls: ["./profile.component.css"],
})
export class ProfileComponent implements OnInit {
  user: User;
  provinces: [];
  cities;
  titles;
  selectProvinceId;
  selectCityName = "";

  constructor(
    private alertService: AlertService,
    private lookupService: LookupService,
    private appService: AppService
  ) {}

  ngOnInit() {
    this.user = new User();
    this.getProvinces();
    this.getProfile();
    this.getTitle();
  }

  getProfile() {
    this.appService.get(Urls.Profile).then((res: any) => {
      this.getCities(res.provinceId);
      this.user = res;
    });
  }

  getProvinces() {
    this.appService.get(`${Urls.Lookup}/Provinces`).then((res: any) => {
      this.provinces = res;
    });
  }

  getCities(id) {
    this.appService.get(`${Urls.Lookup}/Cities/${id}`).then((res: any) => {
      this.cities = res;
    });
  }

  getTitle() {
    this.appService.get(`${Urls.Lookup}/Titles`).then((res: any) => {
      this.titles = res;
    });
  }

  selectProvince(e) {
    this.selectProvinceId = e.value;
    this.getCities(e.value);
  }

  save(e) {
    this.appService.put(Urls.Profile, this.user).then(() => {
      notify(
        "Kullanıcı bilgileri başarıyla güncellendi.",
        alertType[alertType.success],
        1000
      );
    });
  }
}
