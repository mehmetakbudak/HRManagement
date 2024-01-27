import { Component, OnInit } from "@angular/core";
import { AppService } from "src/app/app.service";
import { Urls } from "src/app/models/consts";
import { FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { MessageService } from "primeng/api";

@Component({
  selector: "app-profile",
  templateUrl: "./profile.component.html",
  styleUrls: ["./profile.component.css"],
})
export class ProfileComponent implements OnInit {
  form: FormGroup;
  provinces: [];
  cities;
  titles;
  selectProvinceId;
  selectCityName = "";

  constructor(
    private formBuilder: FormBuilder,
    private messageService: MessageService,
    private appService: AppService
  ) { }

  ngOnInit() {
    this.form = this.formBuilder.group({
      id: [0, []],
      emailAddress: new FormControl({ value: '', disabled: true }),
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      birthDate: new FormControl<Date | null>({ value: null, disabled: true }),
      titleId: new FormControl({ value: null, disabled: true }),
      hireDate: new FormControl<Date | null>({ value: null, disabled: true }),
      phone: ['', [Validators.required]],
      provinceId: [null, []],
      cityId: [null, []],
      address: ['', []],
      reportedUserId: [null, []],
      isActive: [false, []]
    });
    this.getProvinces();
    this.getProfile();
    this.getTitle();
  }

  getProfile() {
    this.appService.get(`${Urls.User}/Profile`).then((res: any) => {
      this.getCities(res.provinceId);
      this.form.setValue(res);
      this.form.get('hireDate').setValue(new Date(res.hireDate));
      this.form.get('birthDate').setValue(new Date(res.birthDate));
    });
  }

  getProvinces() {
    this.appService.get(`${Urls.Lookup}/Provinces`).then((res: any) => {
      this.provinces = res;
    });
  }

  getCities(id) {
    if (id) {
      this.appService.get(`${Urls.Lookup}/Cities/${id}`).then((res: any) => {
        this.cities = res;
      });
    }
  }

  getTitle() {
    this.appService.get(`${Urls.Lookup}/Titles`).then((res: any) => {
      this.titles = res;
    });
  }

  selectProvince(e) {
    if (e.value) {
      this.selectProvinceId = e.value;
      this.getCities(e.value);
    } else {
      this.cities = [];
    }
  }

  save(e) {
    this.appService.put(`${Urls.User}/Profile`, this.form.value).then(() => {
      this.messageService.add({ severity: 'info', summary: 'Success', detail: 'Profile updated successfully.' });
    });
  }
}
