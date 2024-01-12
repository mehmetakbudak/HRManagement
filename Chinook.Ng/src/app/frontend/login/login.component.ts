import { Component, OnInit } from "@angular/core";
import { AuthenticationService } from "../../services/authentication.service";
import { ActivatedRoute, Router } from "@angular/router";
import { MessageService } from "primeng/api";
import { AbstractControl, FormBuilder, FormGroup, Validators } from "@angular/forms";

@Component({
  selector: "app-login",
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.css"],
})
export class LoginComponent implements OnInit {
  form: FormGroup;
  submitted = false;
  loading = false;
  returnUrl: string;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private messageService: MessageService,
    private formBuilder: FormBuilder,
    private authenticationService: AuthenticationService
  ) { }

  ngOnInit() {
    const isLogout = this.route.snapshot.queryParams["cikis"];
    this.form = this.formBuilder.group({
      emailAddress: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
    });

    if (isLogout) {
      this.authenticationService.logout();
      this.router.navigateByUrl("/giris");
    }
    this.returnUrl = this.route.snapshot.queryParams["returnUrl"] || "/admin";
  }

  get f(): { [key: string]: AbstractControl } {
    return this.form.controls;
  }

  onSubmit() {
    this.submitted = true;
    if (this.form?.invalid) {
      return;
    }
    this.authenticationService
      .login(this.form.value)
      .then(() => {
        const currentUser = this.authenticationService.currentUserValue;
        const fullName = currentUser.firstName + " " + currentUser.lastName;
        this.messageService.add({ severity: 'info', summary: 'Success', detail: "Welcome " + fullName });
        setTimeout(() => {
          this.router.navigateByUrl(this.returnUrl);
        }, 1000);
      })
      .catch((error) => {
        this.loading = false;
        this.messageService.add({ severity: 'error', summary: 'Error', detail: error });
      });
  }

  public clearForm(): void {
    this.form.reset();
  }

  goTo() {

  }
}
