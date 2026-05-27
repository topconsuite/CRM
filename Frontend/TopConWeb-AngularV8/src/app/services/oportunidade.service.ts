import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';
import { Oportunidade } from 'app/classes/oportunidade/oportunidade';
import { OportunidadeFase } from 'app/classes/oportunidade/oportunidade-fase';
import { OportunidadeInteracao } from 'app/classes/oportunidade/oportunidade-interacao';
import { Proposta } from 'app/classes/proposta/proposta';

@Injectable()
export class OportunidadeService extends BaseService { 

    constructor(injector: Injector) {
        super(injector);
    }

    Adicionar(oportunidade: Oportunidade) {
        return this.makePostPrommisse<any>('v1/oportunidade', JSON.stringify(oportunidade));
    }

    Atualizar(oportunidade: Oportunidade) {
        return this.makePatchPrommisse<any>('v1/oportunidade', JSON.stringify(oportunidade));
    }

    Listar(pagina?: number, porPagina?: number, filtro?: string, hideLoading?: boolean) {
        return this.makePagedGetPrommisse<Oportunidade>(`v1/oportunidade?`
            +'pagina='+(pagina ? pagina : 0)
            +'&porPagina='+(porPagina ? porPagina : 0)
            +(filtro ? '&'+filtro : ''),
            hideLoading);
    }

    ListarFases(hideLoading?: boolean) {
        return this.makeGetPrommisse<OportunidadeFase[]>(`v1/oportunidade/fases`, hideLoading)
    }

    ObterPorId(usina: number, ano: number, numero: number, hideLoading?: boolean) {
        return this.makeGetPrommisse<Oportunidade>(`v1/oportunidade/usina/${usina}/ano/${ano}/numero/${numero}`, hideLoading);
    }    adicionarInteracao(oportunidadeInteracao: OportunidadeInteracao, hideLoading?: boolean) {
        return this.makePostPrommisse<string>('v1/oportunidade/interacao', JSON.stringify(oportunidadeInteracao), hideLoading);
    }

    ObterPropostaDeOportunidade(usina: number, ano: number, numero: number, hideLoading?: boolean) {
        return this.makeGetPrommisse<Proposta>(`v1/oportunidade/usina/${usina}/ano/${ano}/numero/${numero}/gerar-proposta`, hideLoading);
    }

    listarInteracoes(pagina: number, porPagina: number, filtro?: string, hideLoading?: boolean) {
        return this.makePagedGetPrommisse<OportunidadeInteracao>('v1/oportunidade/interacoes?'
            +'pagina='+pagina
            +'&porPagina='+porPagina
            +(filtro ? '&'+filtro : '')
            , hideLoading);
    }
}