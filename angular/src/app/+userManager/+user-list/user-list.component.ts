import { Component, ViewEncapsulation, OnInit, ViewChild } from '@angular/core';
import { DatatableComponent } from '@swimlane/ngx-datatable/release';
import { DataService } from '../../core/services/data.service';
import { Constants } from '../../constants';
import { IUser } from '../../core/models/IUser';
import { GtConfig } from '@angular-generic-table/core';
import { GenericTableComponent } from '@angular-generic-table/core';
import { GtCheckboxComponent } from '@angular-generic-table/core/components/gt-checkbox/gt-checkbox.component';
import { GtOptions } from '@angular-generic-table/core/interfaces/gt-options';
declare var jquery: any;
declare var $: any;
@Component({
    selector: 'app-user-list',
    templateUrl: './user-list.component.html',
    styleUrls: ['./user-list.component.scss'],
    encapsulation: ViewEncapsulation.None
})
export class UserListComponent implements OnInit {
    public configObject: GtConfig<any>;
    public userList = [];

    public pageSize: number = 10;

    public controls: any = {
        pageSize: 10,
        filter: '',
    };


    @ViewChild(GenericTableComponent)
    private myTable: GenericTableComponent<any, any>;
    constructor(private dataService: DataService) {
        this.FetchData();

    }

    public FetchData() {
        this.dataService.get(Constants.USERS)
            .subscribe(data => {
                this.userList = data['result'];
                this.configObject = {
                    settings: [{
                        objectKey: 'checkbox',
                        // sort: 'disable'
                    },{
                        objectKey: 'edit',
                    }, {
                        objectKey: 'username',
                        sort: 'enable',
                        columnOrder: 0,
                        search: true
                    }, {
                        objectKey: 'email',
                        sort: 'enable',
                        columnOrder: 1,
                        search: true
                    }, {
                        objectKey: 'firstname',
                        sort: 'enable',
                        columnOrder: 2,
                        search: true
                    }, {
                        objectKey: 'lastname',
                        sort: 'enable',
                        columnOrder: 3,
                        search: true
                    }, {
                        objectKey: 'isActive',
                        sort: 'enable',
                        columnOrder: 4
                    }],
                    fields: [
                        {
                            name: '',
                            objectKey: 'checkbox',
                            columnClass: 'text-right',
                            columnComponent: {
                                type: 'checkbox'
                            },
                            value: (row) => this.myTable.isRowSelected(row)
                        }, {
                            name: '',
                            columnClass: 'gt-button',
                            objectKey: 'edit',
                            value: () => { return 'up'; },
                            render: (row) => { return '<button class="btn btn-sm btn-primary ' + (row.order === 1 ? 'disabled' : '') + '"><i class="fa fa-arrow-up"></i></button>'; },
                            click: (row) => { return; }
                        }, {
                            name: 'User Name',
                            objectKey: 'username'
                        }, {
                            name: 'Email Address',
                            objectKey: 'email'
                        }, {
                            name: 'First Name',
                            objectKey: 'firstname'
                        }, {
                            name: 'Last Name',
                            objectKey: 'lastname'
                        }, {
                            name: 'Is Active',
                            objectKey: 'isActive'
                        }],
                    data: this.userList,
                };
            },
            err => console.log(err),
            () => console.log('getUser Complete')
            );
    }

    public ngOnInit() {

    }
}
