import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';
import { MotivoPerda } from 'app/classes/motivo-perda/motivo-perda';

@Injectable()
export class MotivoPerdaService extends BaseService {
    constructor(injector: Injector) {
        super(injector);
    }

    Adicionar(motivoPerda: MotivoPerda) {
        return this.makePostPrommisse<any>('v1/motivo-perda', JSON.stringify(motivoPerda));
    }

    Atualizar(motivoPerda: MotivoPerda) {
        return this.makePatchPrommisse<any>('v1/motivo-perda', JSON.stringify(motivoPerda));
    }

    Deletar(motivoPerdaCodigo: number, hideLoading?: boolean) {
        return this.makeDeletePrommisse<MotivoPerda>(`v1/motivo-perda/${motivoPerdaCodigo}`, hideLoading);
    }

    Listar(pagina?: number, porPagina?: number, filtro?: string, hideLoading?: boolean) {
        return this.makePagedGetPrommisse<MotivoPerda>(`v1/motivos-perda?`
            +'pagina='+(pagina ? pagina : 0)
            +'&porPagina='+(porPagina ? porPagina : 0)
            +(filtro ? '&'+filtro : ''),
            hideLoading);
    }

    ObterPorId(motivoPerdaCodigo: number, hideLoading?: boolean) {
        return this.makeGetPrommisse<MotivoPerda>(`v1/motivo-perda/${motivoPerdaCodigo}`, hideLoading);
    }

    listarMotivos(){
        return this.makeGetPrommisse<MotivoPerda[]>('v1/motivo-perda');
    }

    listarMotivosAtivos(){
        return this.makeGetPrommisse<MotivoPerda[]>('v1/motivo-perda-ativos');
    }
}