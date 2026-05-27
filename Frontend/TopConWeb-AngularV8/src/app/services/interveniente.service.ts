import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';
import { Interveniente } from '../classes/interveniente/interveniente';
import { IntervenienteHistorico } from 'app/classes/interveniente/interveniente-historico';
import { IntervenienteAnexo } from 'app/classes/interveniente/interveniente-anexo';

@Injectable()
export class IntervenienteService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }
    
    obterPermitidoPorCpfCnpj(cpfCnpj: string, inscricaoEstadual: string) {
        return this.makeGetPrommisse<Interveniente>('v1/interveniente-permitido/cpfCnpj/'+cpfCnpj
        +'/inscricao-estadual/'+(inscricaoEstadual.trim() || 'ISENTO'));
    }
    
    obterPorCpfCnpj(cpfCnpj: string, inscricaoEstadual: string) {
        return this.makeGetPrommisse<Interveniente>('v1/interveniente/cpfCnpj/'+cpfCnpj
        +'/inscricao-estadual/'+inscricaoEstadual.trim());
    }

    listarFiltrados(filtro?: string, hideLoading?: boolean) {
        return this.makeGetPrommisse<Interveniente[]>('v1/intervenientes'+(filtro ? '?'+filtro : ''), hideLoading);
    }

    listarFiltradosPermitidos(filtro?: string, hideLoading?: boolean) {
        return this.makeGetPrommisse<Interveniente[]>('v1/intervenientes-permitidos'+(filtro ? '?'+filtro : ''), hideLoading);
    }

    inscricaoEstadualValida(inscricaoEstadual: string, hideLoading?: boolean) {

        return this.makeGetPrommisse<boolean>('v1/intervenientes'
            +`/inscricao-estadual/${inscricaoEstadual.trim() || 'ISENTO'}`
            +`/uf/TODOS`
            +`/valida`,
            hideLoading);
    }

    aprovarIss(interveniente: Interveniente, hideLoading?: boolean) {
        return this.makePostPrommisse<any>('v1/intervenientes/aprovar-iss', JSON.stringify(interveniente.codigo), hideLoading);
    }
    
    alterarLimiteCredito(interveniente: Interveniente, hideLoading?: boolean) {
        return this.makePostPrommisse<any>('v1/intervenientes/alterar-limite-credito', JSON.stringify(interveniente), hideLoading);
    }

    listarHistoricoEmOrdemDescrecente(pagina: number, porPagina: number, filtro?: string, hideLoading?: boolean) {
        return this.makePagedGetPrommisse<IntervenienteHistorico>(`v1/intervenientes/listar-historico/desc?`
            +'pagina='+pagina
            +'&porPagina='+porPagina
            +(filtro ? '&'+filtro : ''),
            hideLoading);
    }

    obterPorCodigoInterveniente(intervenienteCodigo: number) {
        return this.makeGetPrommisse<Interveniente>('v1/interveniente/codigo/'+intervenienteCodigo);
    }

    adicionarHistoricoInterveniente(intervenienteHistorico: IntervenienteHistorico, hideLoading?: boolean) {
        return this.makePostPrommisse<any>('v1/interveniente/historico', JSON.stringify(intervenienteHistorico), hideLoading);
    }

    adicionarAnexo(anexo: string, intervenienteCodigo: number, anoChamada: number, numeroChamada: number, nomeAnexo: string, hideLoading?: boolean) {
        var body = {
            intervenienteCodigo: intervenienteCodigo,
            nome:nomeAnexo,
            arquivo:anexo,
            anoChamada: anoChamada,
            numeroChamada: numeroChamada
        };
        return this.makePostPrommisse<any>('v1/interveniente/anexo', JSON.stringify(body), hideLoading);
    }

    adicionarAnexoPorOportunidade(anexo: string, intervenienteCodigo: number, anoOportunidade: number, numeroOportunidade: number, nomeAnexo: string, hideLoading?: boolean) {
        var body = {
            intervenienteCodigo: intervenienteCodigo,
            nome:nomeAnexo,
            arquivo:anexo,
            anoChamada: 0,
            numeroChamada: 0,
            anoOportunidade: anoOportunidade,
            numeroOportunidade: numeroOportunidade
        };
        return this.makePostPrommisse<any>('v1/interveniente/anexo', JSON.stringify(body), hideLoading);
    }

    ObterAnexo(anexo: IntervenienteAnexo, hideLoading?: boolean){
        anexo.dataHora = new Date(anexo.dataHora);
        var dataHoraFormatada = anexo.dataHora.toISOString();
        var encodedDate = encodeURIComponent(dataHoraFormatada);
        
        return this.makeGetPrommisse<string>(
            'v1/interveniente'
            +'/codigo/' + anexo.intervenienteCodigo
            +'/nome-arquivo/'+anexo.nome
            +'/ano/'+anexo.anoChamada
            +'/numero/'+anexo.numeroChamada
            +'/anexo'
            +'/?data-hora='+encodedDate, hideLoading);
    }

    listarAnexosPorOportunidade(intervenienteCodigo: number, usina: number, anoOportunidade: number, numeroOportunidade: number, hideLoading?: boolean) {
        return this.makeGetPrommisse<IntervenienteAnexo[]>('v1/interveniente/oportunidade/'
        +'/codigo/'+intervenienteCodigo
        +'/usina/'+usina
        +'/ano/'+anoOportunidade
        +'/numero/'+numeroOportunidade
        +'/anexos'
        , hideLoading);
    }

    listarAnexos(intervenienteCodigo: number, anoChamada: number, numeroChamada: number, hideLoading?: boolean) {
        return this.makeGetPrommisse<IntervenienteAnexo[]>('v1/interveniente'
        +'/codigo/'+intervenienteCodigo
        +'/ano/'+anoChamada
        +'/numero/'+numeroChamada
        +'/anexos'
        , hideLoading);
    }

    atualizarDescricaoAnexo(anexo: IntervenienteAnexo) {
        return this.makePatchPrommisse<any>('v1/interveniente/anexo', JSON.stringify(anexo));
    }

    removerAnexo(anexo: IntervenienteAnexo, hideLoading?: boolean) {
        anexo.dataHora = new Date(anexo.dataHora);
        var dataHoraFormatada = anexo.dataHora.toISOString();
        var encodedDate = encodeURIComponent(dataHoraFormatada);
        
        return this.makeDeletePrommisse<IntervenienteAnexo>('v1/interveniente'
        +'/codigo/'+anexo.intervenienteCodigo
        +'/nome-arquivo/'+anexo.nome
        +'/ano/'+anexo.anoChamada
        +'/numero/'+anexo.numeroChamada
        +'/anexo'
        +'/?data-hora='+encodedDate
        , hideLoading);
    }
}