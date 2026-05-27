import { Injectable, Injector } from '@angular/core';
import { GrupoEconomico } from 'app/classes/grupo-economico/grupo-economico';

import { BaseService } from './base.service';

@Injectable()
export class GrupoEconomicoService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }

    Adicionar(grupoEconomico: GrupoEconomico) {
        return this.makePostPrommisse<any>('v1/grupos-economicos', JSON.stringify(grupoEconomico));
    }

    Atualizar(grupoEconomico: GrupoEconomico) {
        return this.makePatchPrommisse<any>('v1/grupos-economicos', JSON.stringify(grupoEconomico));
    }

    Deletar(grupoEconomicoCodigo: number, hideLoading?: boolean) {
        return this.makeDeletePrommisse<GrupoEconomico>(`v1/grupos-economicos/${grupoEconomicoCodigo}`, hideLoading);
    }

    Listar(pagina?: number, porPagina?: number, filtro?: string, hideLoading?: boolean) {
        return this.makePagedGetPrommisse<GrupoEconomico>(`v1/grupos-economicos?`
            +'pagina='+(pagina ? pagina : 0)
            +'&porPagina='+(porPagina ? porPagina : 0)
            +(filtro ? '&'+filtro : ''),
            hideLoading);
    }

    ObterPorId(grupoEconomicoCodigo: number, hideLoading?: boolean) {
        return this.makeGetPrommisse<GrupoEconomico>('v1/grupos-economicos/'
            +grupoEconomicoCodigo,
            hideLoading);
    }
    
}