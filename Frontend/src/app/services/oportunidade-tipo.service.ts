import { Injectable, Injector } from '@angular/core';
import { OportunidadeTipo } from 'app/classes/oportunidade/oportunidade-tipo';

import { BaseService } from './base.service';

@Injectable()
export class OportunidadeTipoService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }

    Adicionar(oportunidadeTipo: OportunidadeTipo) {
        return this.makePostPrommisse<any>('v1/oportunidade/tipo', JSON.stringify(oportunidadeTipo));
    }

    Atualizar(oportunidadeTipo: OportunidadeTipo) {
        return this.makePatchPrommisse<any>('v1/oportunidade/tipo', JSON.stringify(oportunidadeTipo));
    }

    Deletar(oportunidadeTipoCodigo: number, hideLoading?: boolean) {
        return this.makeDeletePrommisse<OportunidadeTipo>(`v1/oportunidade/tipo/${oportunidadeTipoCodigo}`, hideLoading);
    }

    Listar(pagina?: number, porPagina?: number, filtro?: string, hideLoading?: boolean) {
        return this.makePagedGetPrommisse<OportunidadeTipo>(`v1/oportunidade/tipos?`
            +'pagina='+(pagina ? pagina : 0)
            +'&porPagina='+(porPagina ? porPagina : 0)
            +(filtro ? '&'+filtro : ''),
            hideLoading);
    }

    ListarAtivos(hideLoading?: boolean) {
        return this.makeGetPrommisse<OportunidadeTipo[]>(`v1/oportunidade/tipos-ativos`, hideLoading)
    }

    ObterPorId(oportunidadeTipoCodigo: number, hideLoading?: boolean) {
        return this.makeGetPrommisse<OportunidadeTipo>(`v1/oportunidade/tipo/${oportunidadeTipoCodigo}`, hideLoading);
    }
    
}