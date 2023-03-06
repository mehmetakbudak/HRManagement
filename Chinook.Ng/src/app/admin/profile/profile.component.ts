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
    this.appService.getApi(Urls.Profile).then((res: any) => {
      this.getCities(res.provinceId);
      this.user = res;
    });
  }

  getProvinces(name?) {
    let filterName = "";
    if (name) {
      filterName = "$filter=contains(label,'" + name + "')&";
    }
    this.appService.getApi(`${Urls.Lookup}/provinces`).then((res: any) => {
      this.provinces = res.value;
    });
  }

  getCities(id, name?) {
    let filterProvinceId = "";
    let filterName = "";
    if (id) {
      filterProvinceId = "$filter=provinceId eq " + id + "";
    }
    if (name) {
      filterName = " and contains(label,'" + name + "')&";
    }
    this.appService.getApi(`${Urls.Lookup}/cities`).then((res: any) => {
      this.cities = res.value;
    });
  }

  getTitle() {
    this.lookupService.get(Lookup.Title).then((res) => {
      this.titles = res.value;
    });
  }

  selectProvince(e) {
    this.selectProvinceId = e.value;
    this.getCities(e.value, this.selectCityName);
  }

  filterProvince(e) {
    this.getProvinces(e);
  }

  filterCity(e) {
    this.selectCityName = e;
    this.getCities(this.selectProvinceId, e);
  }

  save(e) {
    e.preventDefault();
    this.appService.put(Urls.Profile, this.user).then(() => {
      notify(
        "Kullanıcı bilgileri başarıyla güncellendi.",
        alertType[alertType.success],
        1000
      );
    });
  }
}
