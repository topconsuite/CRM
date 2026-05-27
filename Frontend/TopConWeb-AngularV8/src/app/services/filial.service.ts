import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';
import { Filial } from "app/classes/filial/Filial";

@Injectable()
export class FilialService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }

    obterPorId(idFilial: number) {
        return this.makeGetPrommisse<Filial>(`v1/filial/${idFilial}`);
    }

    listar() {
        return this.makeGetPrommisse<Filial[]>(`v1/filiais`);
    }

}