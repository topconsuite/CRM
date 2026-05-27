import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';
import { Compromisso } from 'app/classes/agenda/compromisso';


@Injectable()
export class CompromissoService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }

    AdicionarAgrupamento(compromissos: Compromisso[]) {
        return this.makePostPrommisse<any>('v1/compromisso/grupo', JSON.stringify(compromissos));
    }

    Adicionar(compromisso: Compromisso) {
        return this.makePostPrommisse<any>('v1/compromisso', JSON.stringify(compromisso));
    }

    Atualizar(compromisso: Compromisso) {
        return this.makePatchPrommisse<any>('v1/compromisso', JSON.stringify(compromisso));
    }

    Deletar(compromissoCodigo: number, hideLoading?: boolean) {
        return this.makeDeletePrommisse<Compromisso>(`v1/compromisso/${compromissoCodigo}`, hideLoading);
    }

    ListarEmOrdemDecrescentePorHorario(pagina?: number, porPagina?: number, filtro?: string, filtroHoraInicioDe?: string, filtroHoraInicioAte?: string, filtroHoraFinalDe?: string, filtroHoraFinalAte?: string, filtroUsuarios?: string, hideLoading?: boolean) {
        return this.makePagedGetPrommisse<Compromisso>(`v1/compromisso/por-horario?`
            +'pagina='+(pagina ? pagina : 0)
            +'&porPagina='+(porPagina ? porPagina : 0)
            +(filtro ? '&'+filtro : '')
            +(filtroHoraInicioDe ? '&filtroHoraInicioDe='+filtroHoraInicioDe : '')
            +(filtroHoraInicioAte ? '&filtroHoraInicioAte='+filtroHoraInicioAte : '')
            +(filtroHoraFinalDe ? '&filtroHoraFinalDe='+filtroHoraFinalDe : '')
            +(filtroHoraFinalAte ? '&filtroHoraFinalAte='+filtroHoraFinalAte : '')
            +(filtroUsuarios ? '&filtroUsuarios='+filtroUsuarios : ''),
            hideLoading);
    }

    ObterPorId(compromissoCodigo: number, hideLoading?: boolean) {
        return this.makeGetPrommisse<Compromisso>(`v1/compromisso/${compromissoCodigo}`, hideLoading);
    }

    ListarGruposDeUsuarioRespeitandoDireito(hideLoading?: boolean) {
        return this.makeGetPrommisse<{ [key: string]: string }>(`v1/compromisso/usuarios`, hideLoading);
    }

    ListarUsuarioAgrupamento(idAgrupamento: string, hideLoading?: boolean) {
        return this.makeGetPrommisse<{ [key: string]: string }>(`v1/compromisso/usuarios/` + idAgrupamento, hideLoading);
    }
    
}