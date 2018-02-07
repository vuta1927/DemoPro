import { Routes, RouterModule } from '@angular/router';
import { UserListComponent } from './+user-list/user-list.component';
import { RoleComponent } from './+role/role.component';
import { AuthGuard } from 'app/core/guards/auth.guard';
export const routes: Routes = [
    { path: 'users', canActivate: [AuthGuard], component: UserListComponent },
    { path: 'roles', canActivate: [AuthGuard], component: RoleComponent }
];

export const routing = RouterModule.forChild(routes);
