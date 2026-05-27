import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';
import { CondicaoPagamento } from 'app/classes/pagamento/pagamento.classes';

@Injectable()
export class CondicaoPagamentoService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }

    listarFiltrados(pagina: number, porPagina: number, filtro?: string, hideLoading?: boolean) {
        return this.makePagedGetPrommisse<CondicaoPagamento>('v1/condicoes-pagamento?'
            +'pagina='+pagina
            +'&porPagina='+porPagina
            +(filtro ? '&'+filtro : ''),
            hideLoading);
    }

    delete(servicoCodigo: number, hideLoading?: boolean) {
        return this.makeDeletePrommisse<CondicaoPagamento>(`v1/condicoes-pagamento/${servicoCodigo}`, hideLoading);
    }

    possuiObrasUtilizando(servicoCodigo: number, hideLoading?: boolean) {
        return this.makeGetPrommisse<boolean>(`v1/condicoes-pagamento/possui-obras-utizando/${servicoCodigo}`, hideLoading);
    }

    atualizar(item: CondicaoPagamento, hideLoading?: boolean) {
        return this.makePatchPrommisse<any>('v1/condicoes-pagamento', JSON.stringify(item), hideLoading);
    }

    adicionar(item: CondicaoPagamento, hideLoading?: boolean) {
        return this.makePostPrommisse<any>('v1/condicoes-pagamento', JSON.stringify(item), hideLoading);
    }

}