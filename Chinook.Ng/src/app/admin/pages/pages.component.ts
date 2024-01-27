import { Component, OnInit } from "@angular/core";
import { AppService } from "src/app/app.service";
import { Urls } from "src/app/models/consts";
import { Page } from "src/app/models/page";
import { environment } from "src/environments/environment";

@Component({
  selector: "app-pages",
  templateUrl: "./pages.component.html",
  styleUrls: ["./pages.component.css"],
})
export class PagesComponent implements OnInit {
  dataSource = [];
  isVisible: boolean = false;
  title: string = "";
  screenWidth = "60vw";
  page: Page = new Page();
  menus: [];

  constructor(private appService: AppService) {}

  ngOnInit() {
    this.bindGrid();
    if (screen.width < 768) {
      this.screenWidth = "90vw";
    }
  }

  bindGrid() {
    var token = this.appService.getToken();

  }

  getById(id) {
    this.appService.get(`${Urls.Page}/${id}`).then((res: any) => {
      this.page = res;
    });
  }

  getMenus() {
    this.appService.get(`${Urls.Lookup}/Menus`).then((res: any) => {
      this.menus = res;
    });
  }

  add() {
    this.isVisible = true;
    this.title = "Sayfa Ekle";
    this.getMenus();
    this.page = new Page();
  }

  edit(e) {
    this.isVisible = true;
    this.title = "Sayfa Düzenle";
    this.getMenus();
    this.getById(e.data.id);
  }

  save(e) {}
}
