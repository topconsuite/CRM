import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';
import { Tarefa } from 'app/classes/agenda/tarefa';

@Injectable()
export class TarefaService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }

    ListarUsuarioAgrupamento(idAgrupamento: string, hideLoading?: boolean) {
        return this.makeGetPrommisse<{ [key: string]: string }>(`v1/tarefa/usuarios/` + idAgrupamento, hideLoading);
    }

    Adicionar(tarefa: Tarefa) {
        return this.makePostPrommisse<any>('v1/tarefa', JSON.stringify(tarefa));
    }

    AdicionarAgrupamento(tarefa: Tarefa[]) {
        return this.makePostPrommisse<any>('v1/tarefa/grupo', JSON.stringify(tarefa));
    }

    Atualizar(tarefa: Tarefa) {
        return this.makePatchPrommisse<any>('v1/tarefa', JSON.stringify(tarefa));
    }

    Deletar(tarefaCodigo: number, hideLoading?: boolean) {
        return this.makeDeletePrommisse<Tarefa>(`v1/tarefa/${tarefaCodigo}`, hideLoading);
    }

    ListarEmOrdemDecrescentePorHorario(pagina?: number, porPagina?: number, filtro?: string, filtroTarefasAtrasadas?: boolean, filtroHorarioDe?: string, filtroHorarioAte?: string, filtroUsuarios?: string, hideLoading?: boolean) {
        return this.makePagedGetPrommisse<Tarefa>(`v1/tarefa/por-horario?`
            +'pagina='+(pagina ? pagina : 0)
            +'&porPagina='+(porPagina ? porPagina : 0)
            +(filtro ? '&'+filtro : '')
            +(filtroTarefasAtrasadas ? '&filtroTarefasAtrasadas='+filtroTarefasAtrasadas : '')
            +(filtroHorarioDe ? '&filtroHorarioDe='+filtroHorarioDe : '')
            +(filtroHorarioAte ? '&filtroHorarioAte='+filtroHorarioAte : '')
            +(filtroUsuarios ? '&filtroUsuarios='+filtroUsuarios : ''),
            hideLoading);
    }

    ObterPorId(tarefaCodigo: number, hideLoading?: boolean) {
        return this.makeGetPrommisse<Tarefa>(`v1/tarefa/${tarefaCodigo}`, hideLoading);
    }
    
}