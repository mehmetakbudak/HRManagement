import { Component, OnInit } from "@angular/core";
import { AuthenticationService } from "../../services/authentication.service";
import { ActivatedRoute, Router } from "@angular/router";
import { AlertService, alertType } from "../../services/alert.service";
import notify from "devextreme/ui/notify";

export class Login {
  emailAddress: string;
  password: string;
}
@Component({
  selector: "app-login",
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.css"],
})
export class LoginComponent implements OnInit {
  login = new Login();
  loading = false;
  returnUrl: string;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private alertService: AlertService,
    private authenticationService: AuthenticationService
  ) {}

  ngOnInit() {
    const isLogout = this.route.snapshot.queryParams["cikis"];
    if (isLogout) {
      this.authenticationService.logout();
      this.router.navigateByUrl("/giris");
    }
    this.returnUrl = this.route.snapshot.queryParams["returnUrl"] || "/admin";
  }

  save(e) {
    const result = e.validationGroup.validate();
    if (result.isValid) {
      this.authenticationService
        .login(this.login)
        .then(() => {
          const currentUser = this.authenticationService.currentUserValue;
          const fullName = currentUser.firstName + " " + currentUser.lastName;
          notify("Hoşgeldiniz " + fullName, alertType[alertType.success], 1000);
          setTimeout(() => {
            this.router.navigateByUrl(this.returnUrl);
          }, 1000);
        })
        .catch((error) => {
          this.loading = false;
          notify(error, alertType[alertType.error], 1000);
        });
    }
  }

  public clearForm(): void {
    this.login = new Login();
  }

  goTo() {

  }
}
