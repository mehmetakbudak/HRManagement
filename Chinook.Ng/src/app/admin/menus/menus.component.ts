import { Component, OnInit } from "@angular/core";
import { AppService } from "src/app/app.service";
import {
  AlertService,
  alertType,
  defaultMessageType,
} from "src/app/services/alert.service";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { ConfirmService } from "src/app/services/confirm.service";
import { Urls } from "src/app/models/consts";

@Component({
  selector: "app-menus",
  templateUrl: "./menus.component.html",
  styleUrls: ["./menus.component.css"],
})
export class MenusComponent implements OnInit {
  menuSettings = [
    { text: "Düzenle", value: "edit" },
    { text: "Sil", value: "delete" },
  ];
  menuItemSettings = [
    { text: "Alt Menü Elemanı Ekle", value: "addSubMenuItem" },
    { text: "Yukarı Taşı", value: "up" },
    { text: "Aşağı Taşı", value: "down" },
    { text: "Düzenle", value: "edit" },
    { text: "Sil", value: "delete" },
  ];
  loading = false;
  screenWidth = screen.width;
  data;
  titleMenu;
  titleMenuItem;
  isMenuVisible;
  isMenuItemVisible;
  formMenu: FormGroup;
  formMenuItem: FormGroup;
  toggleText: string = "Hide";
  show: boolean = false;

  constructor(
    private appService: AppService,
    private confirmService: ConfirmService,
    private alertService: AlertService
  ) {}

  ngOnInit() {
    this.bindGrid();
    this.formMenu = new FormGroup({
      id: new FormControl(0),
      type: new FormControl(),
      name: new FormControl(null, Validators.required),
      isActive: new FormControl(true),
      isDeletable: new FormControl(),
      items: new FormControl(),
    });
    this.formMenuItem = new FormGroup({
      id: new FormControl(0),
      name: new FormControl(null, Validators.required),
      url: new FormControl(),
      isActive: new FormControl(true),
    });
  }

  bindGrid() {
    this.appService.getApi(Urls.Menu).then((res: any) => {
      this.data = res;
    });
  }

  addMenu() {
    this.isMenuVisible = true;
    this.titleMenu = "Yeni Menü Ekle";
  }

  addMenuItem() {
    this.isMenuItemVisible = true;
    this.titleMenuItem = "Yeni Menü Elemanı Ekle";
  }

  saveMenu() {
    this.formMenu.markAllAsTouched();
    if (this.formMenu.valid) {
      if (this.formMenu.value.id) {
        this.appService.put(Urls.Menu, this.formMenu.value).then(
          (res) => {
            this.bindGrid();
            this.isMenuVisible = false;
            this.alertService.showDefaultMessage(
              defaultMessageType.update,
              alertType.success
            );
          },
          (error) => {
            this.isMenuVisible = false;
            this.alertService.showMessage(error, alertType.error);
          }
        );
      } else {
        this.appService.post(Urls.Menu, this.formMenu.value).then(
          (res) => {
            this.bindGrid();
            this.isMenuVisible = false;
            this.alertService.showDefaultMessage(
              defaultMessageType.save,
              alertType.success
            );
          },
          (error) => {
            this.isMenuVisible = false;
            this.alertService.showMessage(error, alertType.error);
          }
        );
      }
    }
  }

  saveMenuItem() {}

  onMenuClick(e, c) {
    if (e.value == "edit") {
      this.isMenuVisible = true;
      this.formMenu.setValue(c);
    } else if (e.value == "delete") {
      if (!c.isDeletable) {
        this.alertService.showMessage("Bu menü silinemez!", alertType.error);
        return;
      }
      // this.confirmService.delete().subscribe((x: any) => {
      //   if (x.primary) {
      //     this.appService.delete(this.appService.menu, c.id).then(res => {
      //       this.bindGrid();
      //       this.alertService.showDefaultMessage(defaultMessageType.delete, alertType.success);
      //     });
      //   }
      // });
    }
  }

  onMenuItemClick(e, c) {
    if (e.value == "edit") {
      console.log(c);
    } else if (e.value == "delete") {
    } else if (e.value == "up") {
    } else if (e.value == "down") {
    }
  }

  public onToggle(): void {
    this.show = !this.show;
    this.toggleText = this.show ? "Hidе" : "Show";
  }
}
