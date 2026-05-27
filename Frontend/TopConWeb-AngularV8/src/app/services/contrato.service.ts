import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';
import { Proposta } from 'app/classes/proposta/proposta';
import { Contrato} from 'app/classes/contrato/contrato';
import { ContratoVersao } from 'app/classes/contrato/contrato-versao';
import { VersionamentoContratoParametro } from 'app/classes/versionamento-contrato/versionamento-contrato-parametro'
import { Obra } from 'app/classes/obra/obra';
import { ContratoFinalidade } from 'app/classes/contrato/contrato-finalidade';

@Injectable()
export class ContratoService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }

    ObterUrlContratoReport(usinaCodigo: number, contratoAno: number, contratoNumero: number): string {
        return this.apiBaseUrlService.getUrl()
            +`v1/contrato/usina/${usinaCodigo}/ano/${contratoAno}/numero/${contratoNumero}/report`;
    }

    ObterUrlContratoResidualReport(usinaCodigo: number, contratoAno: number, contratoNumero: number): string {
        return this.apiBaseUrlService.getUrl()
            +`v1/contratoResidual/usina/${usinaCodigo}/ano/${contratoAno}/numero/${contratoNumero}/report`;
    }

    GerarContrato(proposta: Proposta, hideLoading?: boolean) {
        return this.makePostPrommisse<Contrato>(`v1/contrato`
            +`/proposta-usina/${proposta.usina.codigo}`
            +`/proposta-ano/${proposta.ano}`
            +`/proposta-numero/${proposta.numero}`
            +`/gerar`, "", hideLoading);
    }

    GerarContratoPelaObraAposAprovacaoComercial(obra: Obra, hideLoading?: boolean) {
        return this.makePostPrommisse<Contrato>(`v1/contrato`
            +`/proposta-usina/${obra.usinaCodigo}`
            +`/proposta-ano/${obra.anoChamada}`
            +`/proposta-numero/${obra.numChamada}`
            +`/gerar-comercial`, "", hideLoading);
    }

    AprovaCoincidenciasCadastrais(contrato: Contrato, hideLoading?: boolean) {
        return this.makePostPrommisse<any>('v1/contrato/cadastro/coincidencias/aprovar', JSON.stringify(contrato), hideLoading);
    }

    ReprovaCoincidenciasCadastrais(contrato: Contrato, hideLoading?: boolean) {
        return this.makePostPrommisse<any>('v1/contrato/cadastro/coincidencias/reprovar', JSON.stringify(contrato), hideLoading);
    }

    ListarVersoesContrato(usinaCodigo: number, contratoAno: number, contratoNumero: number, hideLoading?: boolean) {
        return this.makeGetPrommisse<ContratoVersao[]>(`v1/contrato/versoes`
            +`/usina/${usinaCodigo}`
            +`/ano/${contratoAno}`
            +`/numero/${contratoNumero}`, hideLoading);
    }

    ObterUrlContratoVersaoReport(versao: number, usinaCodigo: number, contratoAno: number, contratoNumero: number): string {
        return this.apiBaseUrlService.getUrl()
            +`v1/contrato/versao/${versao}/usina/${usinaCodigo}/ano/${contratoAno}/numero/${contratoNumero}/report`;
    }

    ObterUrlAditivoVersaoReport(versao: number, usinaCodigo: number, propostaAno: number, propostaNumero: number, contratoAno: number, contratoNumero: number): string {
        return this.apiBaseUrlService.getUrl()
            +`v1/aditivo/versao/${versao}/usina/${usinaCodigo}/anoprop/${propostaAno}/numeroprop/${propostaNumero}/anocontr/${contratoAno}/numerocontr/${contratoNumero}/report`;
    }

    ObterContratoVersaoParametro(hideLoading?: boolean) {
        return this.makeGetPrommisse<VersionamentoContratoParametro>('v1/contrato/versao',hideLoading);
    }

    ListarPropostasComContratoEmOrdemDecrescente(pagina: number, porPagina: number, filtro?: string, filtroStatusProposta?: number, filtroExibicaoContratos?: number, filtroDivergenciaCarteira?: boolean, statusClicksignDocumento?: number, hideLoading?: boolean) {

        return this.makePagedGetPrommisse<Proposta>('v1/contratos?'
            +'pagina='+pagina
            +'&porPagina='+porPagina
            +(filtro ? '&'+filtro : '')
            +(filtroStatusProposta > 0 ? '&filtroStatusProposta='+filtroStatusProposta : '')
            +(filtroExibicaoContratos > 0 ? '&filtroExibicaoContratos='+filtroExibicaoContratos : '')
            +('&filtroDivergenciaCarteira='+filtroDivergenciaCarteira)
            +('&statusClicksignDocumento='+statusClicksignDocumento),
            hideLoading);

    }

    ListarFinalidades(hideLoading?: boolean) {
        return this.makeGetPrommisse<ContratoFinalidade[]>('v1/contrato-finalidades', hideLoading);
    }

}