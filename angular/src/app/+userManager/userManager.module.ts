import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SharedModule } from 'app/shared/shared.module';
import { routing } from './userManager.routing';
import { UserListComponent } from './+user-list/user-list.component';
import { RoleComponent } from './+role/role.component';

// import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { GenericTableModule } from '@angular-generic-table/core';

@NgModule({
    declarations: [
      UserListComponent,
      RoleComponent
    ],
    imports: [
        SharedModule,
        FormsModule,
        GenericTableModule,
        routing
    ]
})

export class UserManagerModule {

}
