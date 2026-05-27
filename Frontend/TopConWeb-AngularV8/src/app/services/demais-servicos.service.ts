import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';
import { DemaisServicos } from '../classes/demais-servicos/demais-servicos';
import { Usina } from 'app/classes/usina/usina';

@Injectable()
export class DemaisServicosService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }

    adicionar(item: DemaisServicos, hideLoading?: boolean) {
        item['usinaCodigo'] = item.usina.codigo;
        item['mercadoriaCodigo'] = item.mercadoria.codigo;
        item['unidadeSigla'] = item.unidade.sigla;
        return this.makePostPrommisse<any>('v1/demais-servicos', JSON.stringify(item), hideLoading);
    }

    atualizar(item: DemaisServicos, hideLoading?: boolean) {
        item['usinaCodigo'] = item.usina.codigo;
        item['mercadoriaCodigo'] = item.mercadoria.codigo;
        item['unidadeSigla'] = item.unidade.sigla;
        return this.makePatchPrommisse<any>('v1/demais-servicos', JSON.stringify(item), hideLoading);
    }

    delete(servicoCodigo: number, hideLoading?: boolean) {
        return this.makeDeletePrommisse<DemaisServicos>(`v1/demais-servicos/${servicoCodigo}`, hideLoading);
    }

    obter(usinaCodigo: number, servicoCodigo: number, hideLoading?: boolean) {
        return this.makeDeletePrommisse<any>(`v1/demais-servicos/usina/${usinaCodigo}/servico/${servicoCodigo}`, hideLoading);
    }

    listarFiltrados(pagina: number, porPagina: number, filtro?: string, hideLoading?: boolean) {
        return this.makePagedGetPrommisse<DemaisServicos>('v1/demais-servicos?'
            +'pagina='+pagina
            +'&porPagina='+porPagina
            +(filtro ? '&'+filtro : ''),
            hideLoading);
    }

    listarPorUsina(usina: Usina, hideLoading?: boolean) {
        return this.makePagedGetPrommisse<DemaisServicos>('v1/demais-servicos?'
            +'pagina='+1
            +'&porPagina='+9999
            +`&filter=$(usinaCodigo==${usina.codigo})`,
            hideLoading);
    }

}