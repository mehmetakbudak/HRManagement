import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthenticationService } from '../../services/authentication.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertService, alertType } from '../../services/alert.service';
import notify from 'devextreme/ui/notify';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  formData: any = {};

  form: FormGroup;
  loading = false;
  returnUrl: string;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private alertService: AlertService,
    private authenticationService: AuthenticationService) { }

  ngOnInit() {
    const isLogout = this.route.snapshot.queryParams['cikis'];
    if (isLogout) {
      this.authenticationService.logout();
      this.router.navigateByUrl("/giris");
    }
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/admin';
  }

  public login(e) {
    e.preventDefault();
    this.authenticationService.login(this.formData)
      .then(() => {
        const currentUser = this.authenticationService.currentUserValue;
        const fullName = currentUser.firstName + ' ' + currentUser.lastName;
        notify('Hoşgeldiniz ' + fullName, alertType[alertType.success], 1000);
        setTimeout(() => {
          this.router.navigateByUrl(this.returnUrl);
        }, 1000);
      }).catch((error) => {
        this.loading = false;
        notify(error, alertType[alertType.error], 1000);
      });
  }

  public clearForm(): void {
    this.form.reset();
  }
}
