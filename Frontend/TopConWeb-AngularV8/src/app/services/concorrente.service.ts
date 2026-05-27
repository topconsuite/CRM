import { Injectable, Injector } from '@angular/core';
import { Concorrente } from 'app/classes/oportunidade/concorrente';
import { BaseService } from './base.service';

@Injectable()
export class ConcorrenteService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }

    Adicionar(concorrente: Concorrente) {
        return this.makePostPrommisse<any>('v1/oportunidade/concorrente', JSON.stringify(concorrente));
    }

    Atualizar(concorrente: Concorrente) {
        return this.makePatchPrommisse<any>('v1/oportunidade/concorrente', JSON.stringify(concorrente));
    }

    Deletar(concorrenteCodigo: number, hideLoading?: boolean) {
        return this.makeDeletePrommisse<Concorrente>(`v1/oportunidade/concorrente/${concorrenteCodigo}`, hideLoading);
    }

    Listar(pagina?: number, porPagina?: number, filtro?: string, hideLoading?: boolean) {
        return this.makePagedGetPrommisse<Concorrente>(`v1/oportunidade/concorrentes?`
            +'pagina='+(pagina ? pagina : 0)
            +'&porPagina='+(porPagina ? porPagina : 0)
            +(filtro ? '&'+filtro : ''),
            hideLoading);
    }

    ListarAtivos( hideLoading?: boolean) {
        return this.makeGetPrommisse<Concorrente[]>(`v1/oportunidade/concorrentes-ativos`, hideLoading);
    }

    ObterPorId(concorrenteCodigo: number, hideLoading?: boolean) {
        return this.makeGetPrommisse<Concorrente>(`v1/oportunidade/concorrente/${concorrenteCodigo}`, hideLoading);
    }
    
}