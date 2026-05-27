import { NgModule } from '@angular/core';
import { FuseSharedModule } from '@fuse/shared.module';
import { RouterModule } from '@angular/router';
import { MatInputModule, MatCheckboxModule, MatButtonModule } from '@angular/material';

import { FuseLoginComponent } from './login.component';

const routes = [
    {
        path     : 'pages/auth/login',
        component: FuseLoginComponent
    }
];

@NgModule({
    declarations: [
        FuseLoginComponent
    ],
    imports     : [
        FuseSharedModule,
        RouterModule.forChild(routes),
        MatInputModule, MatCheckboxModule, MatButtonModule
    ]
})

export class LoginModule
{

}
