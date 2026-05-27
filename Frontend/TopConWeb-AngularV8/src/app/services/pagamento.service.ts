import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';
import { Usina } from '../classes/usina/usina';
import { CondicaoPagamento, TipoCobranca, CartaoBandeira, Portador, Conta, Pagamento } from '../classes/pagamento/pagamento.classes';
import { SolicitacaoPagamento } from 'app/classes/pagamento/solicitacao-pagamento';
import { Obra } from 'app/classes/obra/obra';
import { MovimentoBanco } from 'app/classes/movimento-banco/movimento-banco';

@Injectable()
export class PagamentoService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }

    ListarCartaoBandeiras(hideLoading?: boolean) {
        return this.makeGetPrommisse<CartaoBandeira[]>('v1/cartaoBandeiras', hideLoading);
    }

    ListarCartaoBandeirasComIntegracao(hideLoading?: boolean) {
        return this.makeGetPrommisse<CartaoBandeira[]>('v1/cartaoBandeiras/com-integracao', hideLoading);
    }

    ListarCondicoesPagamentoPorUsinaDataIntervenienteTipo(usina: Usina, data: Date, intervenienteTipo: string, segmentacao: number, hideLoading?: boolean) {

        return this.makeGetPrommisse<CondicaoPagamento[]>('v1/condicoesPagamento'
            +'/usina/'+usina.codigo
            +'/data/'+data.toISOString().substr(0, 10)
            +'/intervenienteTipo/'+intervenienteTipo
            +'/segmentacao/'+segmentacao,
            hideLoading);

    }

    ObterValorAdicionalM3PorCondicaoPagamentoUsinaPrecoUnitarioTabela(condicaoPagamento: CondicaoPagamento, usina: Usina, precoUnitarioTabela: number, hideLoading?: boolean) {

        return this.makeGetPrommisse<number>(`v1/condicaoPagamento/${condicaoPagamento.codigo}`
            +`/usina/${usina.codigo}`
            +`/preco-unitario-tabela/${precoUnitarioTabela}`
            +`/valor-adicional`, hideLoading);
        
    }

    ListarTiposCobrancaPorCondicaoPagamento(condicaoPagamento: CondicaoPagamento, hideLoading?: boolean) {

        return this.makeGetPrommisse<TipoCobranca[]>('v1/tiposCobranca'
            +'/condicaoPagamento/'+condicaoPagamento.codigo,
            hideLoading);

    }

    ListarTiposCobranca(hideLoading?: boolean) {
        return this.makeGetPrommisse<TipoCobranca[]>('v1/tiposCobranca', hideLoading);
    }

    ListarPortadoresVinculadosComContas(hideLoading?: boolean) {
        return this.makeGetPrommisse<Portador[]>('v1/portadores/vinculadosComContas', hideLoading);
    }

    SolicitarPagamento(solicitacaoPagamento: SolicitacaoPagamento,hideLoading?: boolean) {
        return this.makePostPrommisse<any>('v1/solicitacao-pagamento', JSON.stringify(solicitacaoPagamento), hideLoading);
    }

    DesaprovarPagamento(obra: Obra, pagamentoSelecionado: Pagamento ,verificaMovimentoBancarioConciliado?: boolean ,hideLoading?: boolean){
        return this.makePostPrommisse<any>(`v1/financeiro/desaprovar-condicao-pagamento/usina/${obra.usinaCodigo}`
        +`/contrato-ano/${obra.anoContrato}`
        +`/contrato-numero/${obra.numContrato}`
        +`/sequencia-pagamento/${pagamentoSelecionado.sequencia}`
        +`/verifica-movimento-bancario/${verificaMovimentoBancarioConciliado}`, 
        "", hideLoading);
    }

    ListarMovimentosBancoNaoVinculadosComContasAReceber(empresaCodigo: number, contaCodigo: number, dataOperacao: Date, hideLoading?: boolean) {
        var url = `v1/financeiro/movimento-banco/empresa/${empresaCodigo}/conta/${contaCodigo}/nao-vinculados-car`

        if (dataOperacao)
            url += `?dataOperacao=${dataOperacao.toISOString().substr(0, 10)}`;
        
        return this.makeGetPrommisse<MovimentoBanco[]>(url, hideLoading);
    }
}