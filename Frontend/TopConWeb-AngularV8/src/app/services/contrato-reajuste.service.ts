import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';
import { ContratoReajuste } from 'app/classes/contrato-reajuste/contrato-reajuste';
import { ContratoReajusteVigencia } from 'app/classes/contrato-reajuste/contrato-reajuste-vigencia';
import { ContratoReajusteLog } from 'app/classes/contrato-reajuste/contrato-reajuste-log';

@Injectable()
export class ContratoReajusteService extends BaseService {
    constructor(injector: Injector) {
        super(injector);
    }

    ListaTodosReajustesTraco(pagina: number, porPagina: number, filtro?: string, hideLoading?: boolean) {
        return this.makePagedGetPrommisse<ContratoReajuste>('v1/contrato-reajuste/traco?'
            +'&pagina='+ pagina
            +'&porPagina='+ porPagina
            +(filtro ? '&'+ filtro : ''),
            hideLoading);
    }

    ListaTodosReajustesBomba(pagina: number, porPagina: number, filtro?: string, hideLoading?: boolean) {
        return this.makePagedGetPrommisse<ContratoReajuste>('v1/contrato-reajuste/bomba?'
            +'&pagina='+ pagina
            +'&porPagina='+ porPagina
            +(filtro ? '&'+ filtro : ''),
            hideLoading);
    }

    ListarReajusteLogs(usina: number, contratoAno: number, contratoNumero: number, dataVigencia: Date, tipo: string, hideLoading?: boolean) {
        return this.makeGetPrommisse<ContratoReajusteLog[]>('v1/contrato-reajuste'
            + '/usina/' + usina
            + '/ano/' + contratoAno
            + '/numero/' + contratoNumero
            + '/dataVigencia/' + dataVigencia.toString().replace("T00:00:00", "")
            + '/tipo/' + tipo
            + '/logs/',
            hideLoading);
    }

    AprovarReajusteTraco(contratoReajuste: ContratoReajuste, hideLoading?: boolean) {
        return this.makePatchPrommisse(
            'v1/contrato-reajuste/aprovar/traco', 
            JSON.stringify(contratoReajuste), 
            hideLoading);
    }

    AprovarReajusteBomba(contratoReajuste: ContratoReajuste, hideLoading?: boolean) {
        return this.makePatchPrommisse(
            'v1/contrato-reajuste/aprovar/bomba', 
            JSON.stringify(contratoReajuste), 
            hideLoading);
    }

    AprovarTodosTraco(contratoReajustes: Array<ContratoReajuste>, hideLoading?: boolean) {
        return this.makePatchPrommisse(
            'v1/contrato-reajuste/aprovar-todos/traco', 
            JSON.stringify(contratoReajustes), 
            hideLoading);
    }

    AprovarTodosBomba(contratoReajustes: Array<ContratoReajuste>, hideLoading?: boolean) {
        return this.makePatchPrommisse(
            'v1/contrato-reajuste/aprovar-todos/bomba', 
            JSON.stringify(contratoReajustes), 
            hideLoading);
    }

    ReprovarReajusteTraco(contratoReajuste: ContratoReajuste, hideLoading?: boolean) {
        return this.makePatchPrommisse(
            'v1/contrato-reajuste/reprovar/traco', 
            JSON.stringify(contratoReajuste), 
            hideLoading);
    }

    ReprovarTodosTraco(contratoReajustes: Array<ContratoReajuste>, hideLoading?: boolean) {
        return this.makePatchPrommisse(
            'v1/contrato-reajuste/reprovar-todos/traco', 
            JSON.stringify(contratoReajustes), 
            hideLoading);
    }

    ReprovarReajusteBomba(contratoReajuste: ContratoReajuste, hideLoading?: boolean) {
        return this.makePatchPrommisse(
            'v1/contrato-reajuste/reprovar/bomba', 
            JSON.stringify(contratoReajuste), 
            hideLoading);
    }

    ReprovarTodosBomba(contratoReajustes: Array<ContratoReajuste>, hideLoading?: boolean) {
        return this.makePatchPrommisse(
            'v1/contrato-reajuste/reprovar-todos/bomba', 
            JSON.stringify(contratoReajustes), 
            hideLoading);
    }

    ObterVigenciasTraco() {
        return this.makeGetPrommisse<ContratoReajusteVigencia[]>('v1/contrato-reajuste/traco/vigencias');
    }

    ObterVigenciasBomba() {
        return this.makeGetPrommisse<ContratoReajusteVigencia[]>('v1/contrato-reajuste/bomba/vigencias');
    }
}