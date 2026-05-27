import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';
import { Proposta } from '../classes/proposta/proposta.classes';
import { Usina } from '../classes/usina/usina';
import { PropostaVersao } from 'app/classes/proposta/proposta-versao';
import { PropostaReportPDF } from 'app/classes/proposta/proposta-report';
import { PropostaPropaganda } from 'app/classes/proposta/proposta-propaganda';

@Injectable()
export class PropostaService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }

    Adicionar(proposta: Proposta) {
        return this.makePostPrommisse<Proposta>('v1/proposta', JSON.stringify(proposta));
    }

    Atualizar(proposta: Proposta) {
        return this.makePatchPrommisse<any>('v1/proposta', JSON.stringify(proposta));
    }

    ListarEmOrdemDecrescente(pagina: number, porPagina: number, filtro?: string, filtroStatusProposta?: number, filtroExibicaoContratos?: number, filtroDivergenciaCarteira?: boolean,hideLoading?: boolean) {

        return this.makePagedGetPrommisse<Proposta>('v1/propostas?'
            +'pagina='+pagina
            +'&porPagina='+porPagina
            +(filtro ? '&'+filtro : '')
            +(filtroStatusProposta > 0 ? '&filtroStatusProposta='+filtroStatusProposta : '')
            +(filtroExibicaoContratos > 0 ? '&filtroExibicaoContratos='+filtroExibicaoContratos : '')
            +('&filtroDivergenciaCarteira='+filtroDivergenciaCarteira),
            hideLoading);

    }

    ListarPorCpfCnpj(cpfCnpj: string, pagina: number, porPagina: number, hideLoading?: boolean) {

        return this.makePagedGetPrommisse<Proposta>('v1/propostas'
            +'/cpf-cnpj/'+cpfCnpj
            +'/pagina/'+pagina
            +'/por-pagina/'+porPagina,
            hideLoading);

    }

    ObterPorUsinaAnoNumero(usina: Usina, ano: number, numero: number, hideLoading?: boolean) {

        return this.makeGetPrommisse<Proposta>('v1/proposta'
            +'/usina/'+usina.codigo
            +'/ano/'+ano
            +'/numero/'+numero,
            hideLoading);

    }

    ObterVolumeTotalProposto(usina: Usina, ano: number, numero: number, hideLoading?: boolean) {

        return this.makeGetPrommisse<number>('v1/proposta'
            +'/usina/'+usina.codigo
            +'/ano/'+ano
            +'/numero/'+numero
            +'/volume-total',
            hideLoading);

    }

    ObterUrlPropostaReport(proposta: Proposta): string {
        return this.apiBaseUrlService.getUrl()
            +`v1/proposta/usina/${proposta.usina.codigo}/ano/${proposta.ano}/numero/${proposta.numero}/report`;
    }

    ObterUrlPropostaProgramacaoReport(proposta: Proposta, sequenciaProgramacao: number): string {
        return this.apiBaseUrlService.getUrl()
            +`v1/proposta/usina/${proposta.usina.codigo}/ano/${proposta.ano}/numero/${proposta.numero}/sequencia-programacao/${sequenciaProgramacao}/report`;
    }
    
    ListarPropostaReport(usinaCodigo: number, anoChamada: number, numeroChamada: number, hideLoading?: boolean) {
        return this.makeGetPrommisse<PropostaReportPDF[]>(`v1/proposta/report`
            +`/usina/${usinaCodigo}`
            +`/ano/${anoChamada}`
            +`/numero/${numeroChamada}`, hideLoading);
    }

    ObterUrlPropostaReportSequencia(usinaCodigo: number, anoChamada: number, numeroChamada: number, sequencia: number, hideLoading?: boolean) {
        return this.apiBaseUrlService.getUrl()
            +`v1/proposta/report/usina/${usinaCodigo}/ano/${anoChamada}/numero/${numeroChamada}/sequencia/${sequencia}`;
    }

    CriarNovaPropostaReport(usinaCodigo: number, anoChamada: number, numeroChamada: number, propagandaId?: string, hideLoading?: boolean) {
        return this.makePostPrommisse<number>(`v1/proposta/report`
            +`/usina/${usinaCodigo}`
            +`/ano/${anoChamada}`
            +`/numero/${numeroChamada}`, JSON.stringify({ propagandaId: propagandaId }), hideLoading);
    }

    ListarPropagandaTodos(hideLoading?: boolean) {
        return this.makeGetPrommisse<PropostaPropaganda[]>(`v1/proposta/propaganda`, hideLoading);
    }

    AdicionarPropaganda(file64String: string, fileName: string, hideLoading?: boolean) {

        var parameters = {
            nome: fileName,
            arquivo: file64String
        }

        return this.makePostPrommisse<string>(`v1/proposta/propaganda`, JSON.stringify(parameters), hideLoading);

    }

    AtualizarPropaganda(propaganda: PropostaPropaganda, hideLoading?: boolean) {
        return this.makePatchPrommisse<string>(`v1/proposta/propaganda`, JSON.stringify(propaganda), hideLoading);
    }

    ObterPropagandaAnexo(propaganda: PropostaPropaganda, hideLoading?: boolean) {
        return this.makeGetPrommisse<string>(`v1/proposta/propaganda/anexo/${propaganda.id}`)
    }

    ObterPropaganda(id: string, hideLoading?: boolean) {
        return this.makeGetPrommisse<PropostaPropaganda>(`v1/proposta/propaganda/${id}`)
    }

    RemoverPropaganda(propaganda: PropostaPropaganda, hideLoading?: boolean) {
        return this.makeDeletePrommisse<string>(`v1/proposta/propaganda/${propaganda.id}`)
    }

}