import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';
import { Vendedor } from '../classes/vendedor/vendedor';

@Injectable()
export class VendedorService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }

    listarAtivos() {
        return this.makeGetPrommisse<Vendedor[]>('v1/vendedores?filter=$(ativo==S)');
    }

    listarVinculados() {
        return this.makeGetPrommisse<Vendedor[]>('v1/vendedores-vinculados');
    }

    listarPermitidos() {
        return this.makeGetPrommisse<Vendedor[]>('v1/vendedores-permitidos');
    }

    listarTodosPermitidos() {
        return this.makeGetPrommisse<Vendedor[]>('v1/todos-vendedores-permitidos');
    }

}