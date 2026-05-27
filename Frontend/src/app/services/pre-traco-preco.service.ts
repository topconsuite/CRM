import { Injectable, Injector } from "@angular/core";
import { BaseService } from "./base.service";

import { PreTracoPreco } from "app/classes/pre-traco-preco/pre-traco-preco";
import { Usina } from "app/classes/usina/usina";
import { Uso } from "app/classes/traco/uso";
import { Pedra } from "app/classes/traco/pedra";
import { Slump } from "app/classes/traco/slump";
import { ResistenciaTipo } from "app/classes/traco/resistencia-tipo";
import { ETipoAlteracaoLoteTabelaVenda } from "app/classes/tabela-venda/tabela-venda";

@Injectable()
export class PreTracoPrecoService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }

    ListarAguardandoCienciaPorPagina(pagina: number, porPagina: number, segmentacao: number, filtro?: string, hideLoading?: boolean) {
        return this.makePagedGetPrommisse<PreTracoPreco>('v1/preTracoPreco?'
            +'&pagina='+ pagina
            +'&porPagina='+ porPagina
            +'&segmentacao='+ segmentacao
            +(filtro ? '&'+ filtro : ''),
            hideLoading);
    }

    ObterUltimoAguardandoCienciaPorTraco(
        usina: Usina, 
        uso: Uso, 
        pedra: Pedra, 
        slump: Slump,
        resistenciaTipo: ResistenciaTipo,
        mpa: number,
        consumo: number, hideLoading?: boolean) {

            return this.makeGetPrommisse<PreTracoPreco>('v1/preTracoPreco'
                + '/usina/' + usina.codigo
                + '/uso/' + uso.codigo
                + '/pedra/' + pedra.codigo
                + '/slump/' + slump.codigo
                + '/resistenciaTipo/' + resistenciaTipo.codigo
                + '/mpa/' + mpa
                + '/consumo/' + consumo,
                hideLoading);

        }

    AtualizarConfirmarCiencia(preTracoPreco: PreTracoPreco, hideLoading?: boolean) {

        return this.makePatchPrommisse(
            'v1/preTracoPreco/ciente', 
            JSON.stringify(preTracoPreco), 
            hideLoading);

    }

    AprovarTodos(preTracosPrecos: Array<PreTracoPreco>, hideLoading?: boolean) {

        return this.makePatchPrommisse(
            'v1/preTracoPreco/aprovar-todos', 
            JSON.stringify(preTracosPrecos), 
            hideLoading);
    }

    ReprovarAlteracao(preTracoPreco: PreTracoPreco, hideLoading?: boolean) {

        return this.makePatchPrommisse(
            'v1/preTracoPreco/reprovar', 
            JSON.stringify(preTracoPreco), 
            hideLoading);

    }

    ReprovarTodos(preTracosPrecos: Array<PreTracoPreco>, hideLoading?: boolean) {

        return this.makePatchPrommisse(
            'v1/preTracoPreco/reprovar-todos', 
            JSON.stringify(preTracosPrecos), 
            hideLoading);
    }

    AtualizarLote(preTracosPrecos: Array<PreTracoPreco>,tipo: ETipoAlteracaoLoteTabelaVenda, valor:number, hideLoading?: boolean) {
        var body = {
            tipo: tipo,
            valor: valor,
            tracos: preTracosPrecos
        }
        return this.makePatchPrommisse(
            'v1/preTracoPreco/atualizarLote',
            JSON.stringify(body), 
        hideLoading);
    }

}
