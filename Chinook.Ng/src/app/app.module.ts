import { BrowserModule } from "@angular/platform-browser";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { LOCALE_ID, NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
import { AppRoutes } from "./app.routing";
import { JwtInterceptor } from "./helpers/jwt.interceptor";
import { ErrorInterceptor } from "./helpers/error.interceptor";

import { AppComponent } from "./app.component";
import {
  DxButtonModule,
  DxDataGridModule,
  DxDropDownButtonModule,
  DxFormModule,
  DxLoadIndicatorModule,
  DxMenuModule,
  DxToolbarModule,
  DxTreeViewModule,
} from "devextreme-angular";

import { registerLocaleData } from "@angular/common";
import localeTr from "@angular/common/locales/tr";
import { RouterModule } from "@angular/router";
import { NgbModule } from "@ng-bootstrap/ng-bootstrap";
registerLocaleData(localeTr);

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule.withServerTransition({ appId: "ng-cli-universal" }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    DxMenuModule,
    DxDataGridModule,
    DxTreeViewModule,
    DxFormModule,
    DxLoadIndicatorModule,
    DxToolbarModule,
    DxButtonModule,
    DxDropDownButtonModule,
    NgbModule,
    RouterModule.forRoot(AppRoutes, { relativeLinkResolution: "legacy" }),
  ],
  providers: [
    { provide: LOCALE_ID, useValue: "tr-TR" },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
