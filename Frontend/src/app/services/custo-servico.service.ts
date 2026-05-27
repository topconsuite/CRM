import { Injectable, Injector } from '@angular/core';
import { CustoServico } from 'app/classes/custo-servico/custo-servico';

import { BaseService } from './base.service';

@Injectable()
export class CustoServicoService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }

    Adicionar(custoServico: CustoServico, hideLoading?: boolean) {
        return this.makePostPrommisse<CustoServico>('v1/custo-servico', JSON.stringify(custoServico), hideLoading);
    }

    Atualizar(custoServico: CustoServico, hideLoading?: boolean) {
        return this.makePatchPrommisse<CustoServico>('v1/custo-servico', JSON.stringify(custoServico), hideLoading);
    }

    Deletar(usinaCodigo: number, dataInicioVigencia: string, hideLoading?: boolean) {
        return this.makeDeletePrommisse<CustoServico>(
            `v1/custo-servico/usina/${usinaCodigo}/dataInicioVigencia/${dataInicioVigencia}`, hideLoading, );
    }

    ObterCustoServicoVigentePorUsina(usinaCodigo: number, hideLoading?: boolean) {
        return this.makeGetPrommisse<CustoServico>(
            `v1/custo-servico/usina/${usinaCodigo}`,hideLoading);
    }

    listarFiltrados(pagina: number, porPagina: number, filtro?: string, hideLoading?: boolean) {
        return this.makePagedGetPrommisse<CustoServico>(`v1/custo-servico?
            pagina=${pagina}
            &porPagina=${porPagina}
            ${filtro ? `&${filtro}` : ''}`,
            hideLoading);
    }

}