import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { FrontendComponent } from './frontend/frontend.component';
import { BlogSingleComponent } from './blog-single/blog-single.component';
import { BlogCategoryComponent } from './blog-category/blog-category.component';
import { PageComponent } from './page/page.component';

export const FrontendRoutes: Routes = [
  {
    path: "",
    component: FrontendComponent,
    children: [
      {
        path: "",
        component: HomeComponent,
        pathMatch: "full"
      },
      {
        path: "giris",
        component: LoginComponent
      },
      {
        path: "blog/:url/:id",
        component: BlogSingleComponent
      },
      {
        path: "blog/:url",
        component: BlogCategoryComponent
      },
      {
        path: "sayfalar/:url",
        component: PageComponent
      }
      // {
      //   path: '**',
      //   redirectTo: "/"
      // }
    ]
  }
];
