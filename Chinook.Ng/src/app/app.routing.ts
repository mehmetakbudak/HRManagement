import { Routes } from '@angular/router';

export const AppRoutes: Routes = [
  {
    path: '',
    loadChildren: () => import('./frontend/frontend.module').then(m => m.FrontendModule)
  },
  {
    path: 'admin',
    loadChildren: () => import('./admin/admin.module').then(m => m.AdminModule)
  }];
