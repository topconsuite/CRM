import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';
import { TituloContasAReceber } from '../classes/titulo/titulo.classes';

@Injectable()
export class TituloContasAReceberService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }

    ListarPorNumeroCartaoAutorizacao(numeroCartao: number, autorizacao: string, hideLoading?: boolean) {
        return this.makeGetPrommisse<TituloContasAReceber[]>(`v1/contas-a-receber/numero-cartao/${numeroCartao}/autorizacao/${autorizacao}`, hideLoading);
    }

    ListarPorNumeroCartaoAutorizacaoDuplicado(contratoUsina: number, contratoAno: number, contratoNumero: number, numeroCartao: number, autorizacao: string, hideLoading?: boolean) {
        return this.makeGetPrommisse<TituloContasAReceber[]>(`v1/contas-a-receber/numero-cartao/${numeroCartao}/autorizacao/${autorizacao}`, hideLoading);
    }

}