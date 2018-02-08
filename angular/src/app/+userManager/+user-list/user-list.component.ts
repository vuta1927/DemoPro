import { Component, ViewEncapsulation, OnInit, ViewChild } from '@angular/core';
import { DatatableComponent } from '@swimlane/ngx-datatable/release';
import { DataService } from '../../core/services/data.service';
import { Constants } from '../../constants';
import { IUser } from '../../core/models/IUser';
declare var jquery: any;
declare var $: any;
@Component({
    selector: 'app-user-list',
    templateUrl: './user-list.component.html',
    styleUrls: ['./user-list.component.css'],
    encapsulation: ViewEncapsulation.None
})
export class UserListComponent implements OnInit {
    public rows = [];
    public temp = [];
    public userList = [];
    public timeout: any;
    public loadingIndicator = true;

    public reorderable: boolean = true;

    public pageSize: number = 10;

    public controls: any = {
        pageSize: 10,
        filter: '',
    };

    public columns = [
        { prop: 'username' },
        { name: 'email' },
        { name: 'firstname' },
        { name: 'lastname' },
        { name: 'isActive' }
    ];
    @ViewChild(DatatableComponent) table: DatatableComponent;

    constructor(private dataService: DataService) {
        this.FetchData();
    }

    public FetchData() {
        this.dataService.get(Constants.USERS)
            .subscribe(data => {
                this.userList = data['result'];
                // push our inital complete list
                this.rows = this.userList;
                // cache our list
                this.temp = [...this.userList];
                // this.loadingIndicator = false;
            },
            err => console.log(err),
            () => console.log('getUser Complete')
            );
    }

    public ngOnInit() {

    }

    public updateFilter(event) {
        const val = event.target.value.toLowerCase();

        // filter our data
        const temp = this.temp.filter(function (d) {
            return !val || ['name', 'gender', 'company'].some((field: any) => {
                return d[field].toLowerCase().indexOf(val) !== -1
            })
        });

        // update the rows
        this.rows = temp;
        // Whenever the filter changes, always go back to the first page
        this.table.offset = 0;
    }

    public updatePageSize(value) {
        if (!this.controls.filter) {
            // update the rows
            this.rows = [...this.temp];
            // Whenever the filter changes, always go back to the first page
            this.table.offset = 0;
        }

        this.controls.pageSize = parseInt(value);
        this.table.limit = this.controls.pageSize;
        window.dispatchEvent(new Event('resize'));

    }
}
