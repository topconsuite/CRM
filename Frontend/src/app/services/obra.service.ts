import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';
import { Obra, ObraLog, ObraTaxa, ObraConsulta, ObraTraco, ObraBomba, ObraProjecao } from '../classes/obra/obra.classes';
import { Usina } from '../classes/usina/usina';
import { ObraSimplesDTO } from '../classes/obra/obra';
import { MovimentoBanco } from '../classes/movimento-banco/movimento-banco';
import { ETipoVinculoMpaConsumo } from 'app/classes/traco/traco.classes';

@Injectable()
export class ObraService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }

    /*ListarLogs(usinaCodigo: number, obraNumero: number, propostaAno: number, propostaNumero: number) {

        return this.makeGetPrommisse<ObraLog[]>('v1/obra'
            +`/usina/${usinaCodigo}`
            +`/numero/${obraNumero}`
            +`/ano-chamada/${propostaAno}`
            +`/numero-chamada/${propostaNumero}`
            +`/logs`);
        
    }*/

    ListarLogs(usinaCodigo: number, obraNumero: number, propostaAno: number, propostaNumero: number, contratoAno: number, contratoNumero: number) {

        return this.makeGetPrommisse<ObraLog[]>('v1/obra'
            +`/usina/${usinaCodigo}`
            +`/numero/${obraNumero}`
            +`/ano-chamada/${propostaAno}`
            +`/numero-chamada/${propostaNumero}`
            +`/ano-contrato/${contratoAno}`
            +`/numero-contrato/${contratoNumero}`
            +`/logs`);
        
    }

    ListarObraTaxasPorId(obra: Obra) {

        return this.makeGetPrommisse<ObraTaxa[]>('v1/obra'
            +`/usina/${obra.usinaEntrega.codigo}`
            +`/numero/${obra.numero}`
            +`/taxas`);

    }

    ListarTaxasPadraoByIdUsina(usina: Usina) {

        return this.makeGetPrommisse<ObraTaxa[]>('v1/obra'
            +`/usina/${usina.codigo}`
            +`/taxas-padrao`);

    }

    ObterValorM3Faltante(obra: Obra, temBomba: boolean, volumeTotal: number, volumePorCarga: number) {

        return this.makeGetPrommisse<number>('v1/obra'
            +`/usina/${obra.usinaEntrega.codigo}`
            +`/numero/${obra.numero}`
            +`/tem-bomba/${temBomba}`
            +`/volume-total/${volumeTotal}`
            +`/volume-por-carga/${volumePorCarga}`
            +`/valor-m3-faltante`);

    }

    ObterValorM3FaltanteUsina(usina: Usina, temBomba: boolean, volumeTotal: number, volumePorCarga: number) {

        return this.makeGetPrommisse<number>('v1/obra'
            +`/usina/${usina.codigo}`
            +`/tem-bomba/${temBomba}`
            +`/volume-total/${volumeTotal}`
            +`/volume-por-carga/${volumePorCarga}`
            +`/valor-m3-faltante`);

    }

    ListaPendentesDeAprovacao(hideLoading?: boolean) {
        return this.makeGetPrommisse<Obra[]>('v1/obra/pendente', hideLoading);
    }

    ObterTempoDescarga(usina: number) {
        return this.makeGetPrommisse<number>(`v1/obra/tempo-descarga/usina/${usina}`);
    }

    TemAprovacaoPendente(obra: Obra, hideLoading?: boolean) {
        return this.makeGetPrommisse<boolean>('v1/obra'
            +`/usina/${obra.usinaCodigo}`
            +`/numero/${obra.numero}`
            +`/ano-chamada/${obra.anoChamada}`
            +`/numero-chamada/${obra.numChamada}`
            +`/tem-aprovacao-pendente`,
            hideLoading);
    }

    async ListarPorNumeroCartaoAutorizacaoDuplicado(obra: Obra, numeroCartao: number, autorizacao: string, hideLoading?: boolean) {
        
        return await this.makeGetPrommisse<ObraSimplesDTO[]>('v1/obras'
            +`/usina/${obra.usinaCodigo}`
            +`/obra-numero/${obra.numero}`
            +`/numero-cartao/${numeroCartao}`
            +`/autorizacao/${autorizacao}`,
            hideLoading);
            
    }

    ConsultarObras(pagina: number, porPagina: number, filtro?: string, ordenacao?: string, hideLoading?: boolean) {

        return this.makePagedGetPrommisse<ObraConsulta>('v1/obras/consultar?'
            +'pagina='+pagina
            +'&porPagina='+porPagina
            +(ordenacao ? '&ordenacao='+ordenacao : '')
            +(filtro ? '&'+filtro : ''),
            hideLoading);

    }

    ObterPendenteAprovacaoComercialPorId(obra: ObraConsulta, hideLoading?: boolean) {

        return this.makeGetPrommisse<Obra>('v1/obra/pendente/'
            +obra.usina+','
            +obra.obraNumero+','
            +obra.propostaAno+','
            +obra.propostaNumero+','
            +obra.contratoNumero+','
            +obra.contratoAno,
            hideLoading);

    }

    ObterPendenteAprovacaoComercialPorIdUsandoAObra(obra: Obra, hideLoading?: boolean) {

        return this.makeGetPrommisse<Obra>('v1/obra/pendente/'
            +obra.usinaCodigo+','
            +obra.numero+','
            +obra.anoChamada+','
            +obra.numChamada+','
            +obra.numContrato+','
            +obra.anoContrato,
            hideLoading);

    }

    AprovarComercialmente(obra, hideLoading?: boolean){
        return this.makePostPrommisse<any>('v1/obra/pendente/aprovar/', JSON.stringify(obra), hideLoading);
    }

    ListarObraPagamentos(obra: ObraConsulta, hideLoading?: boolean) {

        return this.makeGetPrommisse<Obra>('v1/obra'
            +`/usina/${obra.usina}`
            +`/numero/${obra.obraNumero}`
            +`/numeroContrato/${obra.contratoNumero}`
            +`/anoContrato/${obra.contratoAno}`
            +`/pagamentos`,
            hideLoading);

    }

    ListarObraTracos(obra: ObraConsulta, hideLoading?: boolean) {

        return this.makeGetPrommisse<Obra>('v1/obra'
            +`/usina/${obra.usina}`
            +`/numero/${obra.obraNumero}`
            +`/numeroContrato/${obra.contratoNumero}`
            +`/anoContrato/${obra.contratoAno}`
            +`/tracos`,
            hideLoading);

    }

    AprovarObraPagamentos(obra, movimentosBancoAVincular: MovimentoBanco[], hideLoading?: boolean){
        obra['movimentosBancoAVincular'] = movimentosBancoAVincular;
        return this.makePostPrommisse<any>('v1/obra/aprovar-pagamentos', JSON.stringify(obra), hideLoading);
    }

    AprovarEngenharia(obra, hideLoading?: boolean){
        return this.makePostPrommisse<any>('v1/obra/aprovar-engenharia', JSON.stringify(obra), hideLoading);
    }

    ObterObraParaValidacaoCadastro(obra: ObraConsulta, hideLoading?: boolean) {

        return this.makeGetPrommisse<Obra>('v1/obra'
            +`/usina/${obra.usina}`
            +`/numero/${obra.obraNumero}`
            +`/numeroContrato/${obra.contratoNumero}`
            +`/anoContrato/${obra.contratoAno}`
            +`/analise/cadastro`,
            hideLoading);
    }

    aprovarDistanciaUsinaCep(obra, hideLoading?: boolean){
        return this.makePostPrommisse<any>('v1/obra/aprovar-distancia-usina-cep', JSON.stringify(obra), hideLoading);
    }

    aprovarZMRC(obra, hideLoading?: boolean){
        return this.makePostPrommisse<any>('v1/obra/aprovar-zmrc', JSON.stringify(obra), hideLoading);
    }

    reprovarZMRC(obra, hideLoading?: boolean){
        return this.makePostPrommisse<any>('v1/obra/reprovar-zmrc', JSON.stringify(obra), hideLoading);
    }

    AlterarStatusCadastroEAnalista(obra, hideLoading?: boolean){
        return this.makePostPrommisse<any>('v1/obra/alterar-status-cadastro-e-analista', JSON.stringify(obra), hideLoading);
    }

    AlterarDadosFiscais(obra, hideLoading?: boolean){
        return this.makePostPrommisse<any>('v1/obra/alterar-dados-fiscais', JSON.stringify(obra), hideLoading);
    }

    AprovarAutomaticamentePagamentosDaCieloLio(usinaCodigo: number,  propostaAno: number, propostaNumero: number, hideLoading?: boolean) {

        return this.makePostPrommisse<any>('v1/obra/aprovacao-cielo-lio'
            +`/usina/${usinaCodigo}`
            +`/ano-chamada/${propostaAno}`
            +`/numero-chamada/${propostaNumero}`, "", hideLoading);
    }

    CalcularEbitdaObraTraco(item: ObraTraco, obra: Obra, hideLoading?: boolean) {
        item['usinaEntrega'] = obra.usinaEntrega;
        item['usinaCodigo'] = obra.usinaCodigo;
        item['municipioCodigo'] = obra.endereco.municipio.codigo;
        item['obraVolumePorCarga'] = obra.volumePorCarga;
        item['obraTempoAteAObra'] = obra.tempoAteAObra;
        item['ObraTempoBtNaObra'] = obra.tempoBtNaObra;
        return this.makePostPrommisse<any>('v1/obra/calcular-ebitda-traco', JSON.stringify(item), hideLoading);
    }

    CalcularEbitdaObraBomba(item: ObraBomba, obra: Obra, hideLoading?: boolean) {
        item['usinaEntregaCodigo'] = obra.usinaEntrega.codigo;
        item['EnderecoMunicipioCodigo'] = obra.endereco.municipio.codigo;
        item['ObraTracos'] = obra.obraTracos;

        return this.makePostPrommisse<any>('v1/obra/calcular-ebitda-bomba', JSON.stringify(item), hideLoading);
    }

    ObterConsumoPorTraco(obra: Obra, traco: ObraTraco, interv: number, hideLoading?: boolean) {
        let vinculo = traco.resistenciaTipo.vinculo;
        let mpaConsumo = vinculo === ETipoVinculoMpaConsumo.MPA ? traco.mpa : (vinculo === ETipoVinculoMpaConsumo.CONSUMO ? traco.consumo : '');
        return this.makeGetPrommisse<any>(`v1/obra/consumo-por-traco/numeroContrato/${obra.numContrato}/anoContrato/${obra.anoContrato}/traco-resistencia/${mpaConsumo}/traco-pedra/${traco.pedra.codigo}/traco-slump-codigo/${traco.slump.codigo}/uso/${traco.uso.codigo}/traco-slump-variacao/${traco.slump.variacao}/interveniente/${interv}`, hideLoading);
    }

    obterConsumoTracoPorContrato(obra: Obra, traco: ObraTraco, hideLoading?: boolean) {

        return this.makeGetPrommisse<any>(`v1/obra/consumo-traco/usinaContrato/${obra.usinaCodigo}/numeroContrato/${obra.numContrato}/anoContrato/${obra.anoContrato}/sequenciaTraco/${traco.sequencia}`, hideLoading);

    }

    VerificarObraFrentePossuiProgramacao(obra: Obra, obraFrenteSequencia: number) {
        return this.makeGetPrommisse<boolean>(`v1/obra-frente-em-uso/` + obra.usinaCodigo + `/` + obra.numero + `/` + obraFrenteSequencia + ``);
    }

    ListarObraProjecao(usinaCodigo: number, obraNumero: number, propostaAno: number, propostaNumero: number) {

        return this.makeGetPrommisse<ObraProjecao[]>('v1/obra'
            +`/usina/${usinaCodigo}`
            +`/numero/${obraNumero}`
            +`/ano-chamada/${propostaAno}`
            +`/numero-chamada/${propostaNumero}`
            +`/projecao`);
        
    }

    obterConsumoPorContrato(obra: Obra, hideLoading?: boolean) {

        return this.makeGetPrommisse<number>(`v1/obra/consumo-contrato/usinaContrato/${obra.usinaCodigo}/numeroContrato/${obra.numContrato}/anoContrato/${obra.anoContrato}`, hideLoading);

    }

    obterVolumePorContrato(obra: Obra, hideLoading?: boolean) {

        return this.makeGetPrommisse<number>(`v1/obra/volume-contrato/usina/${obra.usinaCodigo}/noObra/${obra.numero}/anoChamada/${obra.anoChamada}/noChamada/${obra.numChamada}`, hideLoading);

    }

    obterConsumoAcumuladoPorContrato(obra: Obra, hideLoading?: boolean) {

        return this.makeGetPrommisse<number>(`v1/obra/consumo-acumulado-contrato/usinaContrato/${obra.usinaCodigo}/numeroContrato/${obra.numContrato}/anoContrato/${obra.anoContrato}`, hideLoading);

    }

    obterConsumoMesAtualPorContrato(obra: Obra, hideLoading?: boolean) {

        return this.makeGetPrommisse<number>(`v1/obra/consumo-mes-contrato/usinaContrato/${obra.usinaCodigo}/numeroContrato/${obra.numContrato}/anoContrato/${obra.anoContrato}`, hideLoading);

    }

}