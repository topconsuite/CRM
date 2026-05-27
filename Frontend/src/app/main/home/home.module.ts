import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';

import { FuseSharedModule } from '@fuse/shared.module';

import { FuseHomeComponent } from './home.component';

import { AuthGuardService } from 'app/services/auth/auth-guard.service';

const routes = [
    {
        path     : 'home',
        component: FuseHomeComponent,
        canActivate: [AuthGuardService]
    }
];

@NgModule({
    declarations: [
        FuseHomeComponent
    ],
    imports     : [
        FuseSharedModule,
        RouterModule.forChild(routes),
        TranslateModule
    ],
    exports     : [
        FuseHomeComponent
    ]
})

export class FuseHomeModule
{
}
