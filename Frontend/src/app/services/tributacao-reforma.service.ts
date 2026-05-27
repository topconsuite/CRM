import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';
import { TributacaoReforma } from 'app/classes/tributacao-reforma/tributacao-reforma';

@Injectable()
export class TributacaoReformaService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }

    listarTodosProducao(imposto: string) {
        return this.makeGetPrommisse<TributacaoReforma[]>(`v1/tributacaoReforma/${imposto}`);
    }
}