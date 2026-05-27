import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';
import { TracoPreco, Usina, Uso, Pedra, Slump, ResistenciaTipo } from '../classes/traco/traco.classes';
import { TracoParticularidades } from 'app/classes/traco/traco-particularidades';
import { Obra } from 'app/classes/obra/obra';
import { ETipoAlteracaoLoteTabelaVenda } from 'app/classes/tabela-venda/tabela-venda';
import { TracoPrecoNumeracaoProduto } from 'app/classes/traco/traco-preco-numeracao-produto';
import { ObraTracoStatusResponse } from 'app/classes/traco/traco-status';

@Injectable()
export class TracoPrecoService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }

    ObterPorDataUsinaUsoPedraSlumpResistenciaTipoMpaConsumo
            (usina: Usina, uso: Uso, pedra: Pedra, slump: Slump,
            resistenciaTipo: ResistenciaTipo, mpa: number, consumo: number, obra: Obra, hideLoading?: boolean) {
                
        return this.makePostPrommisse<TracoPreco>('v1/tracoPreco'
            + `/usina/${usina.codigo}`
            + `/uso/${uso.codigo}`
            + `/pedra/${pedra.codigo}`
            + `/slump/${slump.codigo}`
            + `/resistenciaTipo/${resistenciaTipo.codigo}`
            + `/mpa/${mpa}`
            + `/consumo/${consumo}`,
            JSON.stringify(obra), hideLoading);
    }    

    ObterStatusPorNumeracaoProduto(idUsina: number, numeracaoProduto: number, obra: Obra, hideLoading?: boolean) {
        return this.makePostPrommisse<number>('v1/tracoPreco/status'
             + `/usina/${idUsina}`
             + `/numeracaoProduto/${numeracaoProduto}`,
             JSON.stringify(obra), hideLoading);
             
    }

    VerificarObraPossuiTracoStatusCustoVirtual(data: Date, usina: Usina, obra: Obra, hideLoading?: boolean) {

        return this.makePostPrommisse<ObraTracoStatusResponse>('v1/tracoPreco/status'
             + `/data/${data.toISOString().substr(0, 10)}`
             + `/usina/${usina.codigo}`,
             JSON.stringify(obra), hideLoading);

    }

    ObterValorAdicionalM3PorUsinaVolumePrecoUnitarioTabela
            (usina: Usina, volume: number, precoUnitarioTabela: number, hideLoading?: boolean) {
        
        return this.makeGetPrommisse<number>('v1/tracoPreco'
            +`/usina/${usina.codigo}`
            +`/volume/${volume}`
            +`/preco-unitario-tabela/${precoUnitarioTabela}`
            +'/valor-adicional', hideLoading);
        
    }

    ListarPorDataUsina(data: Date, usina: Usina) {

        return this.makeGetPrommisse<TracoPreco[]>('v1/tracoPrecos'
            +'/usina/'+usina.codigo);

    }

    ListarPorDataUsinaPagina(data: Date, usina: Usina, pagina: number, porPagina: number, segmentacao: number, filtro?: string, hideLoading?: boolean) {
        return this.makePagedGetPrommisse<TracoPreco>('v1/tracoPrecos?'
            +'data=' + data.toISOString().split('T')[0]
            +'&usina='+ usina.codigo
            +'&pagina='+ pagina
            +'&porPagina='+ porPagina
            +'&segmentacao='+ segmentacao
            +(filtro ? '&'+ filtro : ''),
            hideLoading);
    }

    
    ListarNumeracoesProdutoPorDataUsina(usina: Usina, idSegmentacao: number, hideLoading?: boolean) {

        return this.makeGetPrommisse<TracoPrecoNumeracaoProduto[]>('v1/tracoPreco/numeracoesProduto'
            +'/usina/'+usina.codigo+'/segmentacao/'+idSegmentacao,
            hideLoading);
    }

    ListarNumeracoesProduto(hideLoading?: boolean) {
        
        return this.makeGetPrommisse<TracoPrecoNumeracaoProduto[]>('v1/tracoPreco/numeracoesProduto', hideLoading);
    }

    ObterPorNumeracaoProduto(usina: Usina, numeracaoProduto: number, obra: Obra, hideLoading?: boolean) {

        return this.makePostPrommisse<TracoPreco>('v1/tracoPreco'
            +`/usina/${usina.codigo}`
            +`/numeracaoProduto/${numeracaoProduto}`,
            JSON.stringify(obra),
            hideLoading);   
    }

    ListarUsosPorDataUsina(usina: Usina, idSegmentacao: number, hideLoading?: boolean) {

        return this.makeGetPrommisse<Uso[]>('v1/tracoPreco/usos'
            +'/usina/'+usina.codigo+'/segmentacao/'+idSegmentacao,
            hideLoading);

    }

    ListarUsos(hideLoading?: boolean) {

        return this.makeGetPrommisse<Uso[]>('v1/usos' ,hideLoading);

    }

    ListarPedrasPorDataUsinaUso(usina: Usina, uso: Uso, hideLoading?: boolean) {

        return this.makeGetPrommisse<Pedra[]>('v1/tracoPreco/pedras'
            +'/usina/'+usina.codigo
            +'/uso/'+uso.codigo,
            hideLoading);

    }

    ListarSlumpsPorDataUsinaUsoPedra(usina: Usina, uso: Uso, pedra: Pedra, hideLoading?: boolean) {

        return this.makeGetPrommisse<Slump[]>('v1/tracoPreco/slumps'
            +'/usina/'+usina.codigo
            +'/uso/'+uso.codigo
            +'/pedra/'+pedra.codigo,
            hideLoading);

    }

    ListarSlumpsNominaisPorDataUsinaUsoPedra(usina: Usina, uso: Uso, pedra: Pedra, hideLoading?: boolean) {

        return this.makeGetPrommisse<Slump[]>('v1/tracoPreco/slumpsNominais'
            +'/usina/'+usina.codigo
            +'/uso/'+uso.codigo
            +'/pedra/'+pedra.codigo,
            hideLoading);

    }

    ListarResistenciaTiposPorDataUsinaUsoPedraSlump
    (usina: Usina, uso: Uso, pedra: Pedra, slump: Slump, hideLoading?: boolean) {

        return this.makeGetPrommisse<ResistenciaTipo[]>('v1/tracoPreco/resistenciaTipos'
            +'/usina/'+usina.codigo
            +'/uso/'+uso.codigo
            +'/pedra/'+pedra.codigo
            +'/slump/'+slump.codigo,
            hideLoading);

    }

    ListarResistenciaTipos(hideLoading?: boolean) {

        return this.makeGetPrommisse<ResistenciaTipo[]>('v1/resitenciaTipos', hideLoading);

    }

    ListarMpasPorDataUsinaUsoPedraSlumpResistenciaTipo
    (usina: Usina, uso: Uso, pedra: Pedra, slump: Slump, resistenciaTipo: ResistenciaTipo, hideLoading?: boolean) {

        return this.makeGetPrommisse<number[]>('v1/tracoPreco/mpas'
            +'/usina/'+usina.codigo
            +'/uso/'+uso.codigo
            +'/pedra/'+pedra.codigo
            +'/slump/'+slump.codigo
            +'/resistenciaTipo/'+resistenciaTipo.codigo,
            hideLoading);

    }

    ListarConsumosPorDataUsinaUsoPedraSlumpResistenciaTipo
    (usina: Usina, uso: Uso, pedra: Pedra, slump: Slump, resistenciaTipo: ResistenciaTipo, hideLoading?: boolean) {

        return this.makeGetPrommisse<number[]>('v1/tracoPreco/consumos'
            +'/usina/'+usina.codigo
            +'/uso/'+uso.codigo
            +'/pedra/'+pedra.codigo
            +'/slump/'+slump.codigo
            +'/resistenciaTipo/'+resistenciaTipo.codigo,
            hideLoading);

    }

    ObterParticularidadesPorUsinaUsoPedraSlumpResistenciaTipoVersaoMpaConsumo
    (usina: Usina, uso: Uso, pedra: Pedra, slump: Slump, resistenciaTipo: ResistenciaTipo, mpa: number, consumo: number, hideLoading?: boolean) {
        return this.makeGetPrommisse<TracoParticularidades>('v1/tracoPreco/familia/homologados'
            +'/usina/'+usina.codigo
            +'/uso/'+uso.codigo
            +'/pedra/'+pedra.codigo
            +'/slump/'+slump.codigo
            +'/resistenciaTipo/'+resistenciaTipo.codigo
            +'/mpa/'+mpa
            +'/consumo/'+consumo,
            hideLoading);

    }

    VerificaTracoPendenteAprovacaoTabelaDeVenda
    (usina: Usina, uso: Uso, pedra: Pedra, slump: Slump, resistenciaTipo: ResistenciaTipo, mpa: number, consumo: number, hideLoading?: boolean) {
        return this.makeGetPrommisse<boolean>('v1/tracoPreco/pendente-aprovacao'
            +'/usina/'+usina.codigo
            +'/uso/'+uso.codigo
            +'/pedra/'+pedra.codigo
            +'/slump/'+slump.codigo
            +'/resistenciaTipo/'+resistenciaTipo.codigo
            +'/mpa/'+mpa
            +'/consumo/'+consumo,
            hideLoading);

    }

    ListarPrecosAtuaisPorObra(usinaCodigo: number, obraNumero: number, contratoNumero: number, contratoAno: number, hideLoading?: boolean) {

        return this.makeGetPrommisse<TracoPreco[]>('v1/tracoPrecos'
            +'/usina/'+usinaCodigo
            +'/obra/'+obraNumero
            +'/contratoNumero/'+contratoNumero
            +'/contratoAno/'+contratoAno,
            hideLoading);

    }

    atualizar(item: TracoPreco, hideLoading?: boolean) {
        return this.makePatchPrommisse<any>('v1/tracoPreco', JSON.stringify(item), hideLoading);
    }

    AtualizarLote(tracosPrecos: Array<TracoPreco>, tipo:ETipoAlteracaoLoteTabelaVenda, valor:number, hideLoading?: boolean) {
        var body = {
            tipo: tipo,
            valor: valor,
            tracos: tracosPrecos
        }
        return this.makePatchPrommisse(
            'v1/tracoPreco/atualizarLote',
            JSON.stringify(body), 
        hideLoading);
    }

    ListarUsosPorSegmentacao(idSegmentacao: number, hideLoading?: boolean) {
        return this.makeGetPrommisse<Uso[]>('v1/tracoPreco/usos-segmentacao'
            +'/segmentacao/'+idSegmentacao,
            hideLoading);

    }

}