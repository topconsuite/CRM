import { Injectable, Injector } from '@angular/core';
import { ClicksignParametro } from 'app/classes/assinatura-eletronica/clicksing-parametro';
import { ClicksignConfiguracao } from 'app/classes/assinatura-eletronica/clicksign-configuracao';
import { ContratoClicksignEnvio } from 'app/classes/assinatura-eletronica/contrato-clicksing-envio';
import { SolicitacaoAssinaturaClicksign } from 'app/classes/assinatura-eletronica/solicitacao-assinatura-clicksing';
import { Usina } from 'app/classes/usina/usina';

import { BaseService } from './base.service';

@Injectable()
export class AssinaturaEletronicaService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }


    ObterClicksignParametro(hideLoading?: boolean) {
        return this.makeGetPrommisse<ClicksignParametro>('v1/assinatura-eletronica/clicksign',hideLoading);
    }

    AtualizarClicksignParametro(parametro: ClicksignParametro ,hideLoading?: boolean) {
        return this.makePostPrommisse<ClicksignParametro>('v1/assinatura-eletronica/clicksign',JSON.stringify(parametro),hideLoading);
    }
    
    SolicitarAssinaturaClicksign(parametro: SolicitacaoAssinaturaClicksign ,hideLoading?: boolean) {
        return this.makePostPrommisse<ClicksignParametro>('v1/assinatura-eletronica/solicitar/clicksign',JSON.stringify(parametro),hideLoading);
    }

    VerificarIntegracaoConfigurada(hideLoading?: boolean) {
        return this.makeGetPrommisse<boolean>('v1/assinatura-eletronica/possui-integracao',hideLoading);
    }

    ListarContratoClicksignEnvios(usina: number, anoContrato: number, numeroContrato: number,hideLoading?: boolean) {
        return this.makeGetPrommisse<ContratoClicksignEnvio[]>('v1/assinatura-eletronica/clicksign-envios'
        +'/usina/'+usina
        +'/ano-contrato/'+anoContrato
        +'/numero-contrato/'+numeroContrato,
        hideLoading);
    }

    ObterUltimoContratoClicksignEnvio(usina: number, anoContrato: number, numeroContrato: number,hideLoading?: boolean) {
        return this.makeGetPrommisse<ContratoClicksignEnvio>('v1/assinatura-eletronica/clicksign-envio'
        +'/usina/'+usina
        +'/ano-contrato/'+anoContrato
        +'/numero-contrato/'+numeroContrato,
        hideLoading);
    }

    CancelarDocumentoClicksign(idDocumento: string, hideLoading?: boolean) {
        return this.makePatchPrommisse<any>('v1/assinatura-eletronica/cancelar/clicksign'
        +'/documento/'+idDocumento, "", hideLoading);
    }

    ListarClicksignConfiguracoes(hideLoading?: boolean) {
        return this.makeGetPrommisse<ClicksignConfiguracao[]>('v1/clicksign-configuracoes', hideLoading);
    }

    SalvarClicksignConfiguracao(configuracao: ClicksignConfiguracao, hideLoading?: boolean) {
        return this.makePostPrommisse<ClicksignConfiguracao>('v1/clicksign-configuracoes', JSON.stringify(configuracao), hideLoading);
    }

    RemoverClicksignConfiguracao(id: number, hideLoading?: boolean) {
        return this.makeDeletePrommisse<any>('v1/clicksign-configuracoes/' + id, hideLoading);
    }

    ListarUsinasPorConfiguracao(id: number, hideLoading?: boolean) {
        return this.makeGetPrommisse<Usina[]>('v1/clicksign-configuracoes/' + id + '/usinas', hideLoading);
    }

    VincularUsina(configuracaoId: number, usinaId: number, hideLoading?: boolean) {
        return this.makePostPrommisse<any>('v1/clicksign-configuracoes/' + configuracaoId + '/usinas/' + usinaId, '{}', hideLoading);
    }

    DesvincularUsina(configuracaoId: number, usinaId: number, hideLoading?: boolean) {
        return this.makeDeletePrommisse<any>('v1/clicksign-configuracoes/' + configuracaoId + '/usinas/' + usinaId, hideLoading);
    }

}