import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';

import { Programacao, ProgramacaoLog, ProgramacaoHora } from '../classes/programacao/programacao.classes';
import { Usina } from '../classes/usina/usina';
import { ObraTraco } from '../classes/obra/obra-traco';
import { ObraBomba } from '../classes/obra/obra-bomba';
import { ObraDemaisServicos } from 'app/classes/obra/obra-demais-servicos';


@Injectable()
export class ProgramacaoService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }

    Adicionar(programacao: Programacao) {
        return this.makePostPrommisse<any>('v1/programacao', JSON.stringify(programacao));
    }

    Atualizar(programacao: Programacao) {
        return this.makePatchPrommisse<any>('v1/programacao', JSON.stringify(programacao));
    }

    Cancelar(programacao: Programacao, observacao: string, hideLoading?: boolean) {

        return this.makePatchPrommisse<any>('v1/programacao'
            +'/usina/'+programacao.usina.codigo
            +'/obra-numero/'+programacao.obraNumero
            +'/sequencia/'+programacao.sequencia
            +'/cancelar/observacao/'+observacao
            , JSON.stringify(programacao),
            hideLoading);

    }

    private ListarComPropostaContratoEmOrdem(pagina: number, porPagina: number, decrescente: boolean, filtro?: string, hideLoading?: boolean) {

        return this.makePagedGetPrommisse<Programacao>(`v1/programacoes${decrescente ? '/desc' : ''}?`
            +'pagina='+pagina
            +'&porPagina='+porPagina
            +(filtro ? '&'+filtro : ''),
            hideLoading);

    }

    ListarComPropostaContratoEmOrdemDescrescente(pagina: number, porPagina: number, filtro?: string, hideLoading?: boolean) {
        return this.ListarComPropostaContratoEmOrdem(pagina, porPagina, true, filtro, hideLoading);
    }

    ListarComPropostaContratoEmOrdemCrescente(pagina: number, porPagina: number, filtro?: string, hideLoading?: boolean) {
        return this.ListarComPropostaContratoEmOrdem(pagina, porPagina, false, filtro, hideLoading);
    }

    ListarPorPropostaTraco(obraTraco: ObraTraco, hideLoading?: boolean) {

        return this.makeGetPrommisse<Programacao[]>(`v1/programacoes`
            + `/usina/${obraTraco.usina ? obraTraco.usina.codigo : 0}`
            + `/proposta-ano/${obraTraco.propostaAno}`
            + `/proposta-numero/${obraTraco.propostaNumero}`
            + `/obra-numero/${obraTraco.obraCodigo}`
            + `/obra-traco-sequencia/${obraTraco.sequencia}`,
            hideLoading);

    }

    ListarPorPropostaBomba(obraBomba: ObraBomba, hideLoading?: boolean) {

        return this.makeGetPrommisse<Programacao[]>(`v1/programacoes`
            + `/usina/${obraBomba.usinaCodigo}`
            + `/proposta-ano/${obraBomba.propostaAno}`
            + `/proposta-numero/${obraBomba.propostaNumero}`
            + `/obra-numero/${obraBomba.obraCodigo}`
            + `/obra-bomba-sequencia/${obraBomba.sequencia}`,
            hideLoading);

    }

    ListarPorItemDemaisServicos(itemDemaisServicos: ObraDemaisServicos, hideLoading?: boolean) {

        return this.makeGetPrommisse<Programacao[]>(`v1/programacoes`
            + `/usina/${itemDemaisServicos.usinaCodigo}`
            + `/obra-numero/${itemDemaisServicos.obraNumero}`
            + `/demais-servicos-sequencia/${itemDemaisServicos.sequencia}`,
            hideLoading);

    }

    ListarHorarios(programacao: Programacao, hideLoading?: boolean) {

        return this.makeGetPrommisse<ProgramacaoHora[]>(`v1/programacao`
            + `/usina/${programacao.usina.codigo}`
            + `/contrato-ano/${programacao.contratoAno}`
            + `/contrato-numero/${programacao.contratoNumero}`
            + `/sequencia/${programacao.sequencia}`
            + `/horarios`,
            hideLoading);

    }

    ObterPorId(usina: Usina, obraNumero: number, sequencia: number, hideLoading?: boolean) {

        return this.makeGetPrommisse<Programacao>('v1/programacao'
            +'/usina/'+usina.codigo
            +'/obra-numero/'+obraNumero
            +'/sequencia/'+sequencia,
            hideLoading);

    }

    TemNotaFiscalEmitida(usina: Usina, obraNumero: number, sequencia: number, hideLoading?: boolean) {

        return this.makeGetPrommisse<boolean>('v1/programacao'
            +'/usina/'+usina.codigo
            +'/obra-numero/'+obraNumero
            +'/sequencia/'+sequencia
            +'/tem-nf-emitida',
            hideLoading);

    }

    ObterVolumeTotalProgramado(usina: Usina, obraNumero: number, hideLoading?: boolean) {

        return this.makeGetPrommisse<number>('v1/programacao'
            +'/usina/'+usina.codigo
            +'/obra-numero/'+obraNumero
            +'/voume-total',
            hideLoading);

    }

    ObterVolumeTotalProgramadoProposta(usina: number, obraNumero: number, hideLoading?: boolean) {
        return this.makeGetPrommisse<number>('v1/programacao'
            +'/usina/'+usina
            +'/obra-numero/'+obraNumero
            +'/voume-total',
            hideLoading);

    }

    ListarLogs(programacao: Programacao) {

        return this.makeGetPrommisse<ProgramacaoLog[]>('v1/programacao'
            +'/usina/'+programacao.usina.codigo
            +'/obra-numero/'+programacao.obraNumero
            +'/sequencia/'+programacao.sequencia
            +'/logs');

    }

    ObterBomba(programacao: Programacao) {

        return this.makeGetPrommisse<ObraBomba>('v1/programacao'
            +'/usina/'+programacao.usina.codigo
            +'/obra-numero/'+programacao.obraNumero
            +'/sequencia/'+programacao.sequencia
            +'/bomba');

    }

    ObterUrlProgramacaoReport(programacao: Programacao): string {
        return this.apiBaseUrlService.getUrl()
            +`v1/programacao/usina/${programacao.usina.codigo}/obra-numero/${programacao.obraNumero}/sequencia/${programacao.sequencia}/report`;
    }

    GeraProgramacao(programacao: Programacao , atualizaComplexidadeBombeado: boolean, gravaContinuidadeProgramacao: boolean, hideLoading?: boolean){
        return this.makePatchPrommisse<any>(
            'v1/programacao/usina/'+programacao.usina.codigo
            +'/obra-numero/'+programacao.obraNumero
            +'/sequencia/'+programacao.sequencia
            +'/gerar'
            , JSON.stringify({
                atualizaComplexidadeBombeado : atualizaComplexidadeBombeado,
                gravaContinuidadeProgramacao : gravaContinuidadeProgramacao
            }),
            hideLoading);
    }

    TemComplexidadeBombeado(usina: Usina, obraNumero: number, sequencia: number, hideLoading?: boolean) {

        return this.makeGetPrommisse<boolean>('v1/programacao'
            +'/usina/'+usina.codigo
            +'/obra-numero/'+obraNumero
            +'/sequencia/'+sequencia
            +'/complexidade-bombeado',
            hideLoading);

    }

    VerificaContinuidade(usina: Usina, obraNumero: number, sequencia: number, hideLoading?: boolean) {

        return this.makeGetPrommisse<boolean>('v1/programacao'
            +'/usina/'+usina.codigo
            +'/obra-numero/'+obraNumero
            +'/sequencia/'+sequencia
            +'/verifica-continuidade',
            hideLoading);

    }

    RejeitaProgramacao(programacao: Programacao , observacaoRejeitada: string, hideLoading?: boolean){
        return this.makePatchPrommisse<any>(
            'v1/programacao/usina/'+programacao.usina.codigo
            +'/obra-numero/'+programacao.obraNumero
            +'/sequencia/'+programacao.sequencia
            +'/rejeitar'
            , JSON.stringify({
                observacaoRejeitada : observacaoRejeitada
            }),
            hideLoading);
    }

    AprovaFinanceiro(programacao: Programacao){
        return this.makePatchPrommisse<any>(
            'v1/programacao/usina/'+programacao.usina.codigo
            +'/obra-numero/'+programacao.obraNumero
            +'/sequencia/'+programacao.sequencia
            +'/aprovacao-financeira'
            , JSON.stringify(programacao));
    }  
    
}