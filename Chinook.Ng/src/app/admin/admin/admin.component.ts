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
  ) { }

  ngOnInit() {
    this.screen = screen;
    if (screen.width >= 768) {
      this.isShowMenu = true;
    }
    this.currentUser = this.authenticationService.currentUserValue;
    this.fullName = this.currentUser.firstName + " " + this.currentUser.lastName;

    this.appService.get(`${Urls.Menu}/gettype/${MenuType.Admin}`).then((res: any) => {
      this.items = res.children;
    });

    this.userMenu = [
      { label: "Go To Homepage", routerLink: "/" },
      { label: "Profile", routerLink: "/admin/profile" },
      { label: "Change Password", routerLink: "/admin/change-password" },
      {
        label: "Logout", command: () => {
          this.router.navigateByUrl("/giris?cikis=true");
        }
      },
    ];
  }

  menuShow() {
    if (screen.width <= 768) {
      this.isShowContent = false;
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
