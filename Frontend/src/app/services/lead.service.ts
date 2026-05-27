import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';
import { Lead } from 'app/classes/lead/lead';
import { Usina } from 'app/classes/usina/usina';
import { LeadLog } from 'app/classes/lead/lead-log';
import { LeadAnexo } from 'app/classes/lead/lead-anexo';
import { LeadInteracao } from 'app/classes/lead/lead-interacao';
import { Oportunidade } from 'app/classes/oportunidade/oportunidade';

@Injectable()
export class LeadService extends BaseService {
    constructor(injector: Injector) {
        super(injector);
    }

    Adicionar(lead: Lead) {
        return this.makePostPrommisse<Lead>('v1/lead', JSON.stringify(lead));
    }

    Atualizar(lead: Lead) {
        return this.makePatchPrommisse<any>('v1/lead', JSON.stringify(lead));
    }

    ListarEmOrdemDecrescente(pagina: number, porPagina: number, filtro?: string, hideLoading?: boolean) {
        return this.makePagedGetPrommisse<Lead>('v1/leads?'
            +'pagina='+pagina
            +'&porPagina='+porPagina
            +(filtro ? '&'+filtro : '')
            , hideLoading);
    }

    ObterPorUsinaAnoNumero(usina: Usina, ano: number, numero: number, hideLoading?: boolean){
        return this.makeGetPrommisse<Lead>('v1/lead'
            + '/usina/' + usina.codigo
            + '/ano/' + ano
            + '/numero/' + numero,
            hideLoading);
    }

    obterOportunidadeDeLead(usina: number, ano: number, numero: number, hideLoading?: boolean) {
        return this.makeGetPrommisse<Oportunidade>(`v1/lead/usina/${usina}/ano/${ano}/numero/${numero}/gerar-oportunidade`, hideLoading);
    }

    ListarLeadLogsPorId(usina: number, ano: number, numero: number, hideLoading?: boolean) {
        return this.makeGetPrommisse<LeadLog[]>('v1/lead'
            + '/usina/' + usina
            + '/ano/' + ano
            + '/numero/' + numero
            + '/logs/',
            hideLoading);
    }

    adicionarAnexo(usina: number, anoLead: number, numeroLead: number, anexo: string, nomeAnexo: string, hideLoading?: boolean) {
        var body = {
            usina: usina,
            ano: anoLead,
            numero: numeroLead,
            arquivo: anexo,
            nome: nomeAnexo
        };
        return this.makePostPrommisse<string>('v1/lead/anexo', JSON.stringify(body), hideLoading);
    }

    listarAnexos(usina: number, anoLead: number, numeroLead: number, hideLoading?: boolean) {
        return this.makeGetPrommisse<LeadAnexo[]>('v1/lead/anexo'
        +'/usina/' + usina
        +'/ano/'+ anoLead
        +'/numero/' + numeroLead
        , hideLoading);
    }

    ObterAnexo(anexoId: string, hideLoading?: boolean){
        return this.makeGetPrommisse<string>('v1/lead/anexo/' + anexoId, hideLoading);
    }

    atualizarDescricaoAnexo(anexo: LeadAnexo) {
        return this.makePatchPrommisse<any>('v1/lead/anexo', JSON.stringify(anexo));
    }

    removerAnexo(id: string, hideLoading?: boolean) {
        return this.makeDeletePrommisse<LeadAnexo>('v1/lead/anexo/' + id, hideLoading);
    }

    adicionarInteracao(leadInteracao: LeadInteracao, hideLoading?: boolean) {
        return this.makePostPrommisse<string>('v1/lead/interacao', JSON.stringify(leadInteracao), hideLoading);
    }

    listarInteracoes(pagina: number, porPagina: number, filtro?: string, hideLoading?: boolean) {
        return this.makePagedGetPrommisse<LeadInteracao>('v1/lead/interacoes?'
            +'pagina='+pagina
            +'&porPagina='+porPagina
            +(filtro ? '&'+filtro : '')
            , hideLoading);
    }
}