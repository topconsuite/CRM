import { Injectable, Injector } from "@angular/core";
import { BaseService } from "./base.service";
import { Visita } from "app/classes/visita/visita";
import { VisitaAnexo } from "app/classes/visita/visita-anexo";
import { VisitaHistorico } from "app/classes/visita/visita-historico";
import { Lead } from "app/classes/lead/lead";

@Injectable()
export class VisitaService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }

    Adicionar(visita: Visita) {
        return this.makePostPrommisse<any>('v1/visita', JSON.stringify(visita));
    }

    Atualizar(visita: Visita) {
        return this.makePatchPrommisse<any>('v1/visita', JSON.stringify(visita));
    }

    Listar(pagina?: number, porPagina?: number, filtro?: string, hideLoading?: boolean) {
        return this.makePagedGetPrommisse<Visita>(`v1/visita?`
            +'pagina='+(pagina ? pagina : 0)
            +'&porPagina='+(porPagina ? porPagina : 0)
            +(filtro ? '&'+filtro : ''),
            hideLoading);
    }

    ObterPorId(usina: number, ano: number, numero: number, hideLoading?: boolean) {
        return this.makeGetPrommisse<Visita>(`v1/visita/usina/${usina}/ano/${ano}/numero/${numero}`, hideLoading);
    }

    obterLeadDeVisita(usina: number, ano: number, numero: number, hideLoading?: boolean) {
        return this.makeGetPrommisse<Lead>(`v1/visita/usina/${usina}/ano/${ano}/numero/${numero}/gerar-lead`, hideLoading);
    }

    adicionarAnexo(usina: number, anoVisita: number, numeroVisita: number, anexo: string, nomeAnexo: string, hideLoading?: boolean) {
        var body = {
            usina: usina,
            ano: anoVisita,
            numero: numeroVisita,
            arquivo: anexo,
            nome: nomeAnexo
        };
        return this.makePostPrommisse<string>('v1/visita/anexo', JSON.stringify(body), hideLoading);
    }

    ObterAnexo(anexoId: string, hideLoading?: boolean){
        return this.makeGetPrommisse<string>('v1/visita/anexo/' + anexoId, hideLoading);
    }

    listarAnexos(usina: number, anoVisita: number, numeroVisita: number, hideLoading?: boolean) {
        return this.makeGetPrommisse<VisitaAnexo[]>('v1/visita/anexo'
        +'/usina/' + usina
        +'/ano/'+ anoVisita
        +'/numero/' + numeroVisita
        , hideLoading);
    }

    atualizarDescricaoAnexo(anexo: VisitaAnexo) {
        return this.makePatchPrommisse<any>('v1/visita/anexo', JSON.stringify(anexo));
    }

    removerAnexo(id: string, hideLoading?: boolean) {
        return this.makeDeletePrommisse<VisitaAnexo>('v1/visita/anexo/' + id, hideLoading);
    }

    adicionarHistorico(historico: VisitaHistorico, hideLoading?: boolean) {
        return this.makePostPrommisse<any>('v1/visita/historico', JSON.stringify(historico), hideLoading);
    }

    ListarHistorico(pagina?: number, porPagina?: number, filtro?: string, hideLoading?: boolean) {
        return this.makePagedGetPrommisse<VisitaHistorico>(`v1/visita/historico?`
            +'pagina='+(pagina ? pagina : 0)
            +'&porPagina='+(porPagina ? porPagina : 0)
            +(filtro ? '&'+filtro : ''),
            hideLoading);
    }
    
}