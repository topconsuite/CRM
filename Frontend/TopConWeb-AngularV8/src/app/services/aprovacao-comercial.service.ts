import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';
import { AprovacaoComercialUsina } from 'app/classes/aprovacao-comercial/aprovacao-comercial-usina.classes';
import { AprovacaoComercialHierarquia, AprovacaoComercialHierarquiaCondicao, AprovacaoComercialHierarquiaCondicaoPagamentoItem, AprovacaoComercialHierarquiaUsuario, Usuario } from 'app/classes/aprovacao-comercial/aprovacao-comercial-hierarquia.classes';
import { AprovacaoComercialTipoPessoa } from 'app/classes/aprovacao-comercial/aprovacao-comercial-tipo-pessoa.classes';
import { ObraConsulta } from 'app/classes/obra/obra.classes';
import { AprovacaoComercialAprovacoesRestantes } from 'app/classes/aprovacao-comercial/aprovacao-comercial-aprovacoes-restantes';
import { AprovacaoComercialHierarquiaDireito } from 'app/classes/aprovacao-comercial/aprovacao-comercial-hierarquia-direito.classes';
import { AprovacaoComercialDados } from 'app/classes/aprovacao-comercial/aprovacao-comercial-dados.classes';
import { CondicaoPagamento } from 'app/classes/pagamento/condicao-pagamento';
import { Segmentacao } from 'app/classes/segmentacao/segmentacao';

@Injectable()
export class AprovacaoComercialService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }

    ListarAguardandoCienciaPorPagina(pagina: number, porPagina: number, filtro?: string, hideLoading?: boolean) {
        return this.makePagedGetPrommisse<AprovacaoComercialUsina>('v1/aprovacao-comercial?'
            +'&pagina='+ pagina
            +'&porPagina='+ porPagina
            +(filtro ? '&'+ filtro : ''),
            hideLoading);
    }

    AdicionarUsina(usina: AprovacaoComercialUsina, hideLoading?: boolean) {
        return this.makePostPrommisse<any>('v1/aprovacao-comercial/adicionar', JSON.stringify(usina), hideLoading);
    }

    AtualizarUsina(usina: AprovacaoComercialUsina, hideLoading?: boolean) {
        return this.makePatchPrommisse<any>('v1/aprovacao-comercial/atualizar', JSON.stringify(usina), hideLoading);
    }

    ListarHierarquiaPorUsina(usina: AprovacaoComercialUsina, hideLoading?: boolean) {
        return this.makePostPrommisse<AprovacaoComercialHierarquia[]>('v1/aprovacao-comercial-hierarquia/listar-por-usina/' + usina.id, '', hideLoading)
    }

    ObterProximoNivelAutoridadePorAprovacaoComercial(usina: AprovacaoComercialUsina, hideLoading?: boolean) {
        return this.makePostPrommisse<number>('v1/aprovacao-comercial-hierarquia/proximo-nivel-autoridade/' + usina.id, '', hideLoading)
    }

    AdicionarHierarquia(hierarquia: AprovacaoComercialHierarquia, hideLoading?: boolean) {
        return this.makePostPrommisse<AprovacaoComercialHierarquia>('v1/aprovacao-comercial-hierarquia/adicionar', JSON.stringify(hierarquia), hideLoading);
    }

    AtualizarHierarquia(hierarquia: AprovacaoComercialHierarquia, hideLoading?: boolean) {
        return this.makePatchPrommisse<any>('v1/aprovacao-comercial-hierarquia/atualizar', JSON.stringify(hierarquia), hideLoading);
    }

    // --------------------- Usuários ------------------------------------

    ListarUsuariosPorHierarquia(hierarquia: AprovacaoComercialHierarquia, hideLoading?: boolean) {
        return this.makePostPrommisse<AprovacaoComercialHierarquiaUsuario[]>('v1/aprovacao-comercial-hierarquia/usuarios/' + hierarquia.id, '', hideLoading);
    }

    ListarUsuariosDisponiveis(hideLoading?: boolean) {
        return this.makePostPrommisse<Usuario[]>('v1/aprovacao-comercial-hierarquia/usuarios-disponiveis', '', hideLoading);
    }

    AdicionarUsuario(usuario: AprovacaoComercialHierarquiaUsuario, hideLoading?: boolean) {
        return this.makePostPrommisse<AprovacaoComercialHierarquiaUsuario>('v1/aprovacao-comercial-hierarquia/usuario', JSON.stringify(usuario), hideLoading);
    }

    RemoverUsuario(usuario: AprovacaoComercialHierarquiaUsuario, hideLoading?: boolean) {
        return this.makeDeletePrommisse<any>('v1/aprovacao-comercial-hierarquia/usuario/remover/' + usuario.id, hideLoading)
    }

    ListarDireitosPorUsuario(obraUsina: number, obraNumero: number, hideLoading?: boolean) {
        return this.makeGetPrommisse<AprovacaoComercialHierarquiaDireito>('v1/aprovacao-comercial/direitos-usuario/' + obraUsina + '/' + obraNumero + '/0', hideLoading)
    }

    // --------------------- Condições ------------------------------------

    ListarTipoPessoa(hideLoading?: boolean) {
        return this.makeGetPrommisse<AprovacaoComercialTipoPessoa[]>('v1/aprovacao-comercial-hierarquia/tipo-pessoa', hideLoading);
    }

    ListarCondicaoPorHierarquaTipoPessoa(hierarquia: AprovacaoComercialHierarquia, tipoPessoa: AprovacaoComercialTipoPessoa, hideLoading?: boolean) {
        return this.makeGetPrommisse<AprovacaoComercialHierarquiaCondicao[]>('v1/aprovacao-comercial-hierarquia/condicao/nivel-hierarquia/' + hierarquia.id + '/tipo-pessoa/' + tipoPessoa.id, hideLoading)
    }

    SalvarCondicoes(condicoes: AprovacaoComercialHierarquiaCondicao[], hideLoading?: boolean) {
        return this.makePostPrommisse<any>('v1/aprovacao-comercial-hierarquia/condicao/adicionar-lote', JSON.stringify(condicoes), hideLoading);
    }

    ListarAprovacoes(obra: ObraConsulta) {
        return this.makeGetPrommisse<AprovacaoComercialDados>(`v1/aprovacao-comercial/listar-aprovacoes/${obra.usina}/${obra.obraNumero}/0`, true);
    }

    ObterCondicaoPagamento(hierarquia: AprovacaoComercialHierarquia, tipoPessoa: AprovacaoComercialTipoPessoa, segmentacao: Segmentacao, hideLoading?: boolean) {
        var url = 'v1/aprovacao-comercial-hierarquia/demais-condicao/nivel-hierarquia/' + hierarquia.id + '/tipo-pessoa/' + tipoPessoa.id + '/segmentacao/' + segmentacao.id;
        return this.makeGetPrommisse<AprovacaoComercialHierarquiaCondicaoPagamentoItem>(url, hideLoading)
    }

    AtualizarCondicaoPagamento(condicaoPagamento: AprovacaoComercialHierarquiaCondicaoPagamentoItem[], hideLoading) {
        return this.makePostPrommisse<string>('v1/aprovacao-comercial-hierarquia/demais-condicao', JSON.stringify(condicaoPagamento), hideLoading);
    }

}
