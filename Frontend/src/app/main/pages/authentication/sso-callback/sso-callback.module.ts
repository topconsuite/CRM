import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { FuseSharedModule } from '@fuse/shared.module';

import { SsoCallbackComponent } from './sso-callback.component';

const routes = [
    {
        path: 'auth/sso',
        component: SsoCallbackComponent
    }
];

@NgModule({
    declarations: [
        SsoCallbackComponent
    ],
    imports: [
        FuseSharedModule,
        RouterModule.forChild(routes)
    ]
})
export class SsoCallbackModule { }
