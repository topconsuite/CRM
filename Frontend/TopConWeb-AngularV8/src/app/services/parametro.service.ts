import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';
import { ParametroProposta } from '../classes/parametro/parametro';

@Injectable()
export class ParametroService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }

    obterParametroPropostaPorDataBase(dataBase: Date, hideLoading?: boolean) {

        return this.makeGetPrommisse<ParametroProposta>('v1/parametro-proposta'
            +'/data-base/'+dataBase.toISOString().substr(0, 10),
            hideLoading);
        
    }

    obterParametoN(group: string, key: string, hideLoading?: boolean) {
        return this.makeGetPrommisse<string>('v1/parametro'
            + `/group/${group}/key/${key}`);
    }
    
    Atualizar(group: string, key: string, value: string, hideLoading?: boolean) {
        
         return this.makeGetPrommisse<string>('v1/parametro'
         +'/group/'+group
         +'/key/'+key
         +'/value/'+value, hideLoading);
    }


}