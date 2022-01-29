import { Routes } from "@angular/router";
import { AuthGuard } from "src/app/helpers/auth.guard";
import { BlogAddComponent } from "./blog-add/blog-add/blog-add.component";
import { BlogEditComponent } from "./blog-edit/blog-edit.component";
import { BlogListComponent } from "./blog-list/blog-list.component";

export const BlogRoutes: Routes = [
  {
    path: "",
    children: [
      {
        path: "",
        component: BlogListComponent,
        canActivate: [AuthGuard],
      },
      {
        path: "add",
        component: BlogAddComponent,
        canActivate: [AuthGuard],
      },
      {
        path: "edit/:id",
        component: BlogEditComponent,
        canActivate: [AuthGuard],
      }
    ],
  },
];
