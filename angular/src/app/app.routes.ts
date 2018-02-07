import { Routes } from '@angular/router';
import { MainLayoutComponent } from './shared/layout/app-layout/main-layout.component';
import { AuthLayoutComponent } from './shared/layout/app-layout/auth-layout.component';
import { AuthGuard } from 'app/core/guards/auth.guard';
import { NotFoundComponent } from './not-found';

export const ROUTES: Routes = [
  {
    path: '',
    component: MainLayoutComponent,
    data: { pageTitle: 'Home' },
    canActivate: [AuthGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      {
        path: 'dashboard',
        loadChildren: 'app/+dashboard/dashboard.module#DashboardModule',
        data: { pageTitle: 'Dashboard' },
        canActivate: [AuthGuard]
      }
    ]
  },
  {
    path: 'auth',
    component: AuthLayoutComponent,
    loadChildren: 'app/+account/account.module#AccountModule'
  },
  {
    path: 'usermanager',
    component: MainLayoutComponent,
    data: { pageTitle: 'Home' },
    canActivate: [AuthGuard],
    children: [
      {
        path: '',
        loadChildren: 'app/+userManager/userManager.module#UserManagerModule',
        data: { pageTitle: 'User Manager' },
        canActivate: [AuthGuard]
      },
    ]
  },
  { path: '**', component: NotFoundComponent }
];
