import { Component, OnInit  } from '@angular/core';
declare var jquery: any;
declare var $: any;
@Component({
    selector: 'app-user-list',
    templateUrl: './user-list.component.html'
})
export class UserListComponent implements OnInit {
    public title: string;
    public ngOnInit() {
        this.title = 'users list';
    }
}
