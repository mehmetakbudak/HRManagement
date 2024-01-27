import { Component, OnInit } from "@angular/core";
import { AppService } from "src/app/app.service";
import {
  AlertService,
  alertType,
  defaultMessageType,
} from "src/app/services/alert.service";
import { ConfirmService } from "src/app/services/confirm.service";
import { Urls } from "src/app/models/consts";
import { environment } from "src/environments/environment";
import { Menu } from "src/app/models/menu";

@Component({
  selector: "app-menus",
  templateUrl: "./menus.component.html",
  styleUrls: ["./menus.component.css"],
})
export class MenusComponent implements OnInit {
  settings = [];
  items = [];
  menu: Menu = new Menu();
  screenWidth = "40vw";
  dataSource = [];
  isVisibleMenu: boolean = false;
  isVisibleMenuItems: boolean = false;
  titleMenu: string;
  titleMenuItems: string;

  constructor(
    private appService: AppService,
    private confirmService: ConfirmService,
    private alertService: AlertService
  ) {}

  ngOnInit() {
    this.settings = [
      ...this.appService.settingEditDelete(),
      { text: "Menü Elemanları", value: "menuItems" },
    ];
    this.bindGrid();
    if (screen.width < 768) {
      this.screenWidth = "90vw";
    }
  }

  bindGrid() {
    var token = this.appService.getToken();

  }

  getById(id) {
    this.appService.get(`${Urls.Menu}/${id}`).then((res: Menu) => {
      this.menu = res;
    });
  }

  getMenuByType(type) {
    this.appService.get(`${Urls.Menu}/gettype/${type}`).then((res: any) => {
      this.items = res.items;
    });
  }

  addMenu() {
    this.menu = new Menu();
    this.items = [];
    this.isVisibleMenu = true;
    this.titleMenu = "Yeni Menü Ekle";
  }

  addMenuItem() {
    this.isVisibleMenu = true;
    this.titleMenu = "Yeni Menü Elemanı Ekle";
  }

  save(e) {
    e.preventDefault();
    if (this.menu.id == 0) {
      this.appService.post(`${Urls.Menu}`, this.menu).then((res) => {
        this.bindGrid();
        this.isVisibleMenu = false;
      });
    } else {
      this.appService.put(`${Urls.Menu}`, this.menu).then((res) => {
        this.bindGrid();
        this.isVisibleMenu = false;
      });
    }
  }

  onItemClick(e, c) {
    if (e.itemData.value === "edit") {
      this.isVisibleMenu = true;
      this.titleMenu = "Menü Düzenle";
      this.getById(c.data.id);
    } else if (e.itemData.value === "delete") {
      if (!c.data.isDeletable) {
        this.alertService.showMessage("Bu menü silinemez!", alertType.error);
        return;
      }
    } else if (e.itemData.value === "menuItems") {
      this.titleMenuItems = "Menü Elemanları";
      this.isVisibleMenuItems = true;
      this.getMenuByType(c.data.type);
    }
  }
}
