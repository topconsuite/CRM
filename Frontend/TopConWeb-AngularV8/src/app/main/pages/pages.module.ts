import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { LoginModule } from './authentication/login/login.module';
import { ComercialModule } from './comercial/comercial.module';
import { MatIconModule } from '@angular/material';

import { UsuarioSemPermissaoPageComponent } from './usuario-sem-permissao-page/usuario-sem-permissao-page.component';
import { IntegracaoCartaoPropagandaPageComponent } from './propagandas/integracao-cartao-propaganda-page/integracao-cartao-propaganda-page.component';
import { RefreshAppPageComponent } from './refresh-app-page/refresh-app-page.component';

const routes = [
    {
        path     : 'pages/sem-permissao',
        component: UsuarioSemPermissaoPageComponent
    },
    {
        path     : 'pages/integracao-cartao',
        component: IntegracaoCartaoPropagandaPageComponent
    },
    {
        path     : 'pages/refreshapp',
        component: RefreshAppPageComponent
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(routes),
        LoginModule,
        ComercialModule,
        MatIconModule
    ],
    declarations: [
        UsuarioSemPermissaoPageComponent,
        IntegracaoCartaoPropagandaPageComponent,
        RefreshAppPageComponent
    ],
    exports: [
        UsuarioSemPermissaoPageComponent,
        IntegracaoCartaoPropagandaPageComponent,
        RefreshAppPageComponent
    ]
})
export class PagesModule
{
}
