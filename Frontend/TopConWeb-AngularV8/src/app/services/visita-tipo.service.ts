import { Injectable, Injector } from '@angular/core';
import { VisitaTipo } from 'app/classes/visita/visita-tipo';

import { BaseService } from './base.service';

@Injectable()
export class VisitaTipoService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }

    Adicionar(tipoVisita: VisitaTipo) {
        return this.makePostPrommisse<any>('v1/visita/tipo', JSON.stringify(tipoVisita));
    }

    Atualizar(tipoVisita: VisitaTipo) {
        return this.makePatchPrommisse<any>('v1/visita/tipo', JSON.stringify(tipoVisita));
    }

    Deletar(tipoVisitaCodigo: number, hideLoading?: boolean) {
        return this.makeDeletePrommisse<VisitaTipo>(`v1/visita/tipo/${tipoVisitaCodigo}`, hideLoading);
    }

    Listar(pagina?: number, porPagina?: number, filtro?: string, hideLoading?: boolean) {
        return this.makePagedGetPrommisse<VisitaTipo>(`v1/visita/tipos?`
            +'pagina='+(pagina ? pagina : 0)
            +'&porPagina='+(porPagina ? porPagina : 0)
            +(filtro ? '&'+filtro : ''),
            hideLoading);
    }

    ListarAtivos(hideLoading?: boolean) {
        return this.makeGetPrommisse<VisitaTipo[]>(`v1/visita/tipo/ativos`, hideLoading);
    }

    ObterPorId(tipoVisitaCodigo: number, hideLoading?: boolean) {
        return this.makeGetPrommisse<VisitaTipo>(`v1/visita/tipo/${tipoVisitaCodigo}`, hideLoading);
    }
    
}