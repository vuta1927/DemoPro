import { Component, OnInit } from '@angular/core';
@Component({
    selector: 'app-role',
    templateUrl: './role.component.html'
})

export class RoleComponent implements OnInit  {
    public title: string = 'Roles';
    public ngOnInit() {
        this.title = 'Roles';
    }
}
