import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from "@angular/common";
import { BlogListComponent } from "./blog-list/blog-list.component";
import { BlogAddComponent } from "./blog-add/blog-add/blog-add.component";
import { RouterModule } from "@angular/router";
import { BlogRoutes } from "./blog.routing";
import { BlogEditComponent } from "./blog-edit/blog-edit.component";
import {
  DxButtonModule, DxDateBoxModule, DxDropDownButtonModule, DxFormModule,
  DxSelectBoxModule, DxTextBoxModule, DxToolbarModule, DxTreeViewModule,
  DxValidatorModule, DxTextAreaModule, DxCheckBoxModule, DxSwitchModule,
  DxDataGridModule, DxPopupModule, DxHtmlEditorModule, DxValidationGroupModule
} from 'devextreme-angular';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
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
    RouterModule.forChild(BlogRoutes)],
  declarations: [BlogListComponent, BlogAddComponent, BlogEditComponent],
})
export class BlogModule {}
