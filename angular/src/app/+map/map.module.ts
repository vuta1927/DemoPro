import { NgModule } from '@angular/core';

import { SharedModule } from 'app/shared/shared.module';
import { routing } from './map.routes';
import { MapComponent } from './map.component';
@NgModule({
    declarations: [
        MapComponent
    ],
    imports: [
        SharedModule,
        routing
    ],
    providers:[]
})
export class MapModule {}
