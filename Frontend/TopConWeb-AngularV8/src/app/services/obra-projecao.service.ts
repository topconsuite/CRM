import { Injectable, Injector } from '@angular/core';
import { Obra } from 'app/classes/obra/obra';
import { ObraProjecao } from 'app/classes/obra/obra-projecao';

import { BaseService } from './base.service';

@Injectable()
export class ObraProjecaoService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }

    ListarPorPagina(pagina: number, porPagina: number, filtro?: string, hideLoading?: boolean) {
        return this.makePagedGetPrommisse<Obra>('v1/obras/projecao/listar?'
                    +'&pagina='+ pagina
                    +'&porPagina='+ porPagina
                    +(filtro ? '&'+ filtro : ''),
                    hideLoading);
    }

    Adicionar(obraProjecao: ObraProjecao) {
        return this.makePostPrommisse<any>('v1/obra-projecao', JSON.stringify(obraProjecao));
    }

    Atualizar(obraProjecao: ObraProjecao) {
        console.log(obraProjecao);
        return this.makePatchPrommisse<any>('v1/obra-projecao', JSON.stringify(obraProjecao));
    }

    obterSaldoObraProjecao(obra: Obra, hideLoading?: boolean) {

        return this.makeGetPrommisse<number>(`v1/saldo-obra-projecao/usina/${obra.usinaCodigo}/noObra/${obra.numero}/anoChamada/${obra.anoChamada}/noChamada/${obra.numChamada}`, hideLoading);

    }

    obterPrevisaoSaldoObraProjecao(obra: Obra, hideLoading?: boolean) {

        return this.makeGetPrommisse<number>(`v1/previsao-saldo-obra-projecao/usina/${obra.usinaCodigo}/noObra/${obra.numero}/anoChamada/${obra.anoChamada}/noChamada/${obra.numChamada}`, hideLoading);

    }

    getProximoPeriodoPorContrato(obra: Obra, hideLoading?: boolean) {

        return this.makeGetPrommisse<Date>(`v1/proximo-periodo-obra-projecao/usina/${obra.usinaCodigo}/noObra/${obra.numero}/anoChamada/${obra.anoChamada}/noChamada/${obra.numChamada}`, hideLoading);

    }

    
}