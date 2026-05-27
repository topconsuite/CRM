import { Injectable, Injector } from '@angular/core';
import { GrupoAcesso} from 'app/classes/acesso-aprovacoes/grupo-acesso';
import { LiberacaoAcesso} from 'app/classes/acesso-aprovacoes/liberacao-acesso';
import { PeriodoAusenciaUsuario } from 'app/classes/acesso-aprovacoes/periodo-ausencia-usuario';
import { Usuario } from 'app/classes/aprovacao-comercial/aprovacao-comercial-hierarquia.classes';
import { Router } from '@angular/router';

import { BaseService } from './base.service';

@Injectable()
export class AcessoAprovacoesConfigService extends BaseService {

    constructor(injector: Injector, private _router: Router,) {
        super(injector);
    }

    Adicionar(grupoAcesso: GrupoAcesso) {
        const grupoAcessoPayload = {
            usina: grupoAcesso.usinaInf.codigo,
            descricao: grupoAcesso.descricao,
            liberacoesAcessos: grupoAcesso.liberacoesAcessos
        };
        return this.makePostPrommisse<any>('v1/liberacoes-acessos/grupo', JSON.stringify(grupoAcessoPayload));
    }

    Atualizar(grupoAcesso: GrupoAcesso, alteraUsuarios: boolean) {
        const grupoAcessoPayload = {
            codigo: grupoAcesso.codigo,
            usina: grupoAcesso.usinaInf.codigo,
            descricao: grupoAcesso.descricao,
            criadoEm: grupoAcesso.criadoEm,
            atualizadoEm: grupoAcesso.atualizadoEm,
            liberacoesAcessos: grupoAcesso.liberacoesAcessos
        };
        return this.makePatchPrommisse<any>('v1/liberacoes-acessos/grupo/'+alteraUsuarios, JSON.stringify(grupoAcessoPayload));
    }

    Deletar(grupoAcessoCodigo: number, hideLoading?: boolean) {
        return this.makeDeletePrommisse<GrupoAcesso>(`v1/liberacoes-acessos/grupo/${grupoAcessoCodigo}`, hideLoading);
    }

    Listar(pagina: number, porPagina: number, filtro?: string, hideLoading?: boolean) {
        return this.makePagedGetPrommisse<GrupoAcesso>('v1/liberacoes-acessos/grupos?'
            +'pagina='+pagina
            +'&porPagina='+porPagina
            +(filtro ? '&'+filtro : ''),
            hideLoading);
    }

    ObterPorId(grupoAcessoCodigo: number, hideLoading?: boolean) {
        return this.makeGetPrommisse<GrupoAcesso>('v1/liberacoes-acessos/grupo/'
            +grupoAcessoCodigo,
            hideLoading);
    }

    // --------------------- Usuários ------------------------------------

    ListarUsuariosPorGrupo(grupoAcessoCodigo: number, hideLoading?: boolean) {
        return this.makePostPrommisse<LiberacaoAcesso[]>('v1/liberacoes-acessos/usuarios/' + grupoAcessoCodigo, '', hideLoading);
    }

    ListarPeriodosAusenciaPorUsuario(usuario: string, hideLoading?: boolean) {
        return this.makePostPrommisse<PeriodoAusenciaUsuario[]>('v1/liberacoes-acessos/usuario/ausencia/' + usuario, '', hideLoading);
    }

    ListarUsuariosDisponiveis(hideLoading?: boolean) {
        return this.makePostPrommisse<Usuario[]>('v1/liberacoes-acessos/usuarios-disponiveis', '', hideLoading);
    }

    AdicionarUsuario(usuario: LiberacaoAcesso[], hideLoading?: boolean) {
        return this.makePostPrommisse<LiberacaoAcesso[]>('v1/liberacoes-acessos/usuario', JSON.stringify(usuario), hideLoading);
    }

    AtualizarUsuario(liberacoesAcessoUsuario: LiberacaoAcesso[]) {
        return this.makePatchPrommisse<any>('v1/liberacoes-acessos/usuario', JSON.stringify(liberacoesAcessoUsuario));
    }

    AtualizarPeriodoAusenciaUsuario(periodoAusenciaUsuario: PeriodoAusenciaUsuario[]) {
        return this.makePostPrommisse<any>('v1/liberacoes-acessos/usuario/ausencia', JSON.stringify(periodoAusenciaUsuario));
    }

    RemoverUsuario(usuario: string, hideLoading?: boolean) {
        return this.makeDeletePrommisse<any>('v1/liberacoes-acessos/usuario/remover/' + usuario, hideLoading)
    }

    ObterLiberacaoAcessoUsuario(hideLoading?: boolean) {
        return this.makeGetPrommisse<boolean>('v1/liberacoes-acessos/usuario',hideLoading);
    }

    async TemLiberacaoAcesso(redirecionaSemPermissaoAfterMiliseconds: number = 0) {
        var temLiberacaoAcesso = await this.ObterLiberacaoAcessoUsuario();

        var self = this;
        
        if (redirecionaSemPermissaoAfterMiliseconds && !temLiberacaoAcesso) {
            setTimeout(() => {
                self._router.navigate(['pages/sem-permissao']);
            }, redirecionaSemPermissaoAfterMiliseconds);
        }
        
        return temLiberacaoAcesso;
    }
}