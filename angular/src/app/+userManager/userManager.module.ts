import { NgModule } from '@angular/core';

import { SharedModule } from 'app/shared/shared.module';
import { routing } from './userManager.routing';
import { UserListComponent } from './+user-list/user-list.component';
import { RoleComponent } from './+role/role.component';
@NgModule({
    declarations: [
      UserListComponent,
      RoleComponent
    ],
    imports: [
        SharedModule,
        routing
    ]
})

export class UserManagerModule {

}
