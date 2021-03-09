import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

import { FrontendRoutes } from "./frontend.routing";
import { HomeComponent } from "./home/home.component";
import { LoginComponent } from "./login/login.component";
import { NavMenuComponent } from "./nav-menu/nav-menu.component";
import { FrontendComponent } from "./frontend/frontend.component";


import { AlertService } from "../services/alert.service";
import { BlogSingleComponent } from "./blog-single/blog-single.component";
import { BlogCategoryComponent } from "./blog-category/blog-category.component";
import { PageComponent } from './page/page.component';
import { DxDataGridModule, DxFormModule, DxLoadIndicatorModule, DxMenuModule, DxTreeViewModule } from 'devextreme-angular';
import { NgxPaginationModule } from 'ngx-pagination';

@NgModule({
  imports: [
    FormsModule,
    CommonModule,
    ReactiveFormsModule,
    DxMenuModule,
    DxDataGridModule,
    DxTreeViewModule,
    DxFormModule,
    DxLoadIndicatorModule,
    NgxPaginationModule,
    RouterModule.forChild(FrontendRoutes)
  ],
  declarations: [
    FrontendComponent,
    HomeComponent,
    LoginComponent,
    NavMenuComponent,
    BlogSingleComponent,
    BlogCategoryComponent,
    PageComponent
  ],
  providers: [
    AlertService,
  ]
})
export class FrontendModule { }
