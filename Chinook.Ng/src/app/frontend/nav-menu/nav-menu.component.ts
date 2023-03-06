import { Component, OnInit, Input } from "@angular/core";
import { Router } from "@angular/router";
import { AuthenticationService } from "../../services/authentication.service";

@Component({
  selector: "app-nav-menu",
  templateUrl: "./nav-menu.component.html",
  styleUrls: ["./nav-menu.component.css"],
})
export class NavMenuComponent implements OnInit {
  @Input() drawer;
  @Input() items = [];
  @Input() userMenu;
  @Input() isLogin = false;

  constructor(private router: Router) {}

  ngOnInit() {}

  toggle() {
    this.drawer.toggle();
  }

  menuItemSelect(e) {
    if (e.itemData.url) {
      this.router.navigateByUrl(e.itemData.url);
    }
  }
}
