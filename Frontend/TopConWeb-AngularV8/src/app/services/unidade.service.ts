import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';
import { Unidade } from '../classes/mercadoria/mercadoria';

@Injectable()
export class UnidadeService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }

    listarTodos( hideLoading?: boolean) {
        return this.makeGetPrommisse<Unidade[]>('v1/unidades', hideLoading);
    }

}