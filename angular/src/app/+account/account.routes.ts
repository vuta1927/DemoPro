import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './+login/login.component';
import { RegisterComponent } from 'app/+account/+register/register.component';
import { AuthGuard } from 'app/core/guards/auth.guard';
export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', canActivate: [AuthGuard], component: RegisterComponent },
  // { path: 'forgot-password', loadChildren: './+forgot/forgot.module#ForgotModule' },
  // { path: 'locked', loadChildren: './+locked/locked.module#LockedModule' }
];

export const routing = RouterModule.forChild(routes);
