import { Component, OnInit } from "@angular/core";
import { AuthenticationService } from "src/app/services/authentication.service";
import {
  defaultMessageType,
  alertType,
  AlertService,
} from "src/app/services/alert.service";
import { AppService } from "src/app/app.service";
import { MenuType } from "src/app/models/enums";
import { Router } from "@angular/router";
import { Urls } from "src/app/models/consts";

@Component({
  selector: "app-admin",
  templateUrl: "./admin.component.html",
  styleUrls: ["./admin.component.css"],
})
export class AdminComponent implements OnInit {
  isShowMenu = false;
  isShowContent = true;
  contentClass = "";

  items = [];
  userMenu = [];
  currentUser;
  fullName;
  screen;

  constructor(
    private authenticationService: AuthenticationService,
    private appService: AppService,
    private router: Router
  ) {}

  ngOnInit() {
    this.screen = screen;
    if (screen.width >= 768) {
      this.isShowMenu = true;
    }
    this.currentUser = this.authenticationService.currentUserValue;
    this.fullName =
      this.currentUser.firstName + " " + this.currentUser.lastName;

    this.appService.getApi(`${Urls.Menu}/MenuType.Admin`).then((res: any) => {
      this.items = res.items;
    });

    this.userMenu = [
      { name: "Siteye Git", url: "/" },
      { name: "Hesabım", url: "/admin/profile" },
      { name: "Şifre Değiştir", url: "/admin/change-password" },
      { name: "Çıkış Yap", url: "/giris?cikis=true", isParam: true },
    ];
  }

  menuShow() {
    this.isShowMenu = !this.isShowMenu;
    if (screen.width <= 768) {
      this.isShowContent = !this.isShowContent;
    } else {
      this.isShowContent = true;
    }
  }

  selectUserMenu(e) {
    if (e.itemData.isParam) {
      this.router.navigateByUrl(e.itemData.url);
    } else {
      this.router.navigate([e.itemData.url]);
    }
  }

  selectMenuItem() {
    if (screen.width <= 768) {
      this.isShowMenu = false;
      this.isShowContent = true;
    }
  }
}
