import { Component, OnInit  } from '@angular/core';

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
