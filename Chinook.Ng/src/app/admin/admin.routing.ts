import { Routes } from '@angular/router';
import { NotesComponent } from './notes/notes.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { AuthGuard } from '../helpers/auth.guard';
import { AdminComponent } from './admin/admin.component';
import { BlogCategoriesComponent } from './blog-categories/blog-categories.component';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { ProfileComponent } from './profile/profile.component';
import { MenusComponent } from './menus/menus.component';
import { UsersComponent } from './users/users.component';

export const AdminRoutes: Routes = [
  {
    path: "",
    component: AdminComponent,
    children: [
      {
        path: "",
        component: DashboardComponent,
        canActivate: [AuthGuard],
      },
      {
        path: "notes",
        component: NotesComponent,
        canActivate: [AuthGuard]
      },
      {
        path: "blog-categories",
        component: BlogCategoriesComponent,
        canActivate: [AuthGuard]
      },
      {
        path: "change-password",
        component: ChangePasswordComponent,
        canActivate: [AuthGuard]
      },
      {
        path: "profile",
        component: ProfileComponent,
        canActivate: [AuthGuard]
      },
      {
        path: "menus",
        component: MenusComponent,
        canActivate: [AuthGuard]
      },
      {
        path: "users",
        component: UsersComponent,
        canActivate: [AuthGuard]
      },
      {
        path: "blogs",
        loadChildren: () => import('./blogs/blog.module').then(m => m.BlogModule)
      },

      // {
      //   path: '**',
      //   redirectTo: ""
      // }
    ]
  }
];
