import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";

import { TableModule } from "primeng/table";
import { CheckboxModule } from "primeng/checkbox";
import { ButtonModule } from "primeng/button";
import { MenuModule } from "primeng/menu";
import { PanelModule } from "primeng/panel";
import { DialogModule } from "primeng/dialog";
import { TreeModule } from "primeng/tree";
import { InputTextModule } from "primeng/inputtext";
import { TriStateCheckboxModule } from 'primeng/tristatecheckbox';

import { DashboardComponent } from "./dashboard/dashboard.component";
import { AdminRoutes } from "./admin.routing";
import { NotesComponent } from "./notes/notes.component";
import { AdminComponent } from "./admin/admin.component";
import { BlogCategoriesComponent } from "./blog-categories/blog-categories.component";
import { AlertService } from "../services/alert.service";
import { ConfirmService } from "../services/confirm.service";
import { ChangePasswordComponent } from "./change-password/change-password.component";
import { ProfileComponent } from "./profile/profile.component";
import { MenusComponent } from "./menus/menus.component";
import { UsersComponent } from "./users/users.component";

import {
  DxButtonModule,
  DxDateBoxModule,
  DxDropDownButtonModule,
  DxFormModule,
  DxSelectBoxModule,
  DxTextBoxModule,
  DxToolbarModule,
  DxTreeViewModule,
  DxValidatorModule,
  DxTextAreaModule,
  DxCheckBoxModule,
  DxSwitchModule,
  DxDataGridModule,
  DxPopupModule,
  DxHtmlEditorModule,
  DxValidationGroupModule,
  DxScrollViewModule
} from "devextreme-angular";
import { BlogsComponent } from "./blogs/blogs.component";
import { PagesComponent } from "./pages/pages.component";

@NgModule({
  imports: [
    FormsModule,
    CommonModule,
    ReactiveFormsModule,
    TableModule,
    CheckboxModule,
    ButtonModule,
    MenuModule,
    InputTextModule,
    PanelModule,
    DialogModule,
    TreeModule,
    TriStateCheckboxModule,
    
    DxTreeViewModule,
    DxToolbarModule,
    DxDropDownButtonModule,
    DxButtonModule,
    DxFormModule,
    DxTextBoxModule,
    DxDateBoxModule,
    DxSelectBoxModule,
    DxValidationGroupModule,
    DxValidatorModule,
    DxTextAreaModule,
    DxCheckBoxModule,
    DxSwitchModule,
    DxDataGridModule,
    DxPopupModule,
    DxHtmlEditorModule,
    DxScrollViewModule,
    RouterModule.forChild(AdminRoutes),
  ],
  declarations: [
    AdminComponent,
    DashboardComponent,
    NotesComponent,
    BlogCategoriesComponent,
    ChangePasswordComponent,
    ProfileComponent,
    MenusComponent,
    UsersComponent,
    BlogsComponent,
    PagesComponent
  ],
  providers: [
    AlertService,
    ConfirmService    
  ],
})
export class AdminModule { }
