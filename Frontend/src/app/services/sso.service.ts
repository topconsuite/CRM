import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';
import { Sso } from 'app/classes/sso/Sso';

@Injectable()
export class SsoService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }

    listarConfiguracaoAzureAd(hideLoading?: boolean) {
        return this.makeGetPrommisse<Sso>(`v1/sso/parametros/azure-ad`, hideLoading);
    }
}