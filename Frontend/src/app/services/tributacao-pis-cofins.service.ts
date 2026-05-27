import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';

import {TributacaoPisCofins} from "../classes/tributacao-pis-cofins/tributacao-pis-cofins";
import {Vendedor} from "../classes/vendedor/vendedor";

@Injectable()
export class TributacaoPisCofinsService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }

    listarTributacoesDeSaida() {
        return this.makeGetPrommisse<TributacaoPisCofins[]>('v1/tributacaoPisCofins');
    }
}