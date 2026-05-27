import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';
import { Pedra } from '../classes/traco/pedra';

@Injectable()
export class PedraService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }

    listarTodos(hideLoading?: boolean) {
        return this.makeGetPrommisse<Pedra[]>('v1/pedras', hideLoading);
    }

}