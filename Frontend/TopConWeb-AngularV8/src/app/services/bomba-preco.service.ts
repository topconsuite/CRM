import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';
import { Usina, BombaPreco, BombaPrecoTerceiro, EBombaM3CalculoTipo,
        Interveniente, CadastroGeral } from '../classes/bomba/bomba.classes';

@Injectable()
export class BombaPrecoService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }

    ListarBombaTiposPorUsina(usina: Usina, hideLoading?: boolean) {
        
        return this.makeGetPrommisse<CadastroGeral[]>('v1/bombaPreco/bombaTipos'
            +'/usina/'+usina.codigo,
            hideLoading);
        
    }

    ListarTerceirosPorBombaTipo(bombaTipo: CadastroGeral, hideLoading?: boolean) {

        return this.makeGetPrommisse<Interveniente[]>('v1/bombaPreco/terceiros'
            +'/bombaTipo/'+bombaTipo.codigo,
            hideLoading);

    }

    ObterPorUsinaBombaTipoData(usina: Usina, bombaTipo: CadastroGeral, data: Date, hideLoading?: boolean) {

        return this.makeGetPrommisse<BombaPreco>('v1/bombaPreco'
            +'/usina/'+usina.codigo
            +'/bombaTipo/'+bombaTipo.codigo
            +'/data/'+data.toISOString().substr(0, 10),
            hideLoading);
        
    }

    ObterPorBombistaBombaTipoData(terceiro: Interveniente, bombaTipo: CadastroGeral, data: Date, hideLoading?: boolean) {

        return this.makeGetPrommisse<BombaPrecoTerceiro>('v1/bombaPreco'
            +'/terceiro/'+terceiro.codigo
            +'/bombaTipo/'+bombaTipo.codigo
            +'/data/'+data.toISOString().substr(0, 10),
            hideLoading);
        
    }

    ObterValorAdicional(usina: Usina, bombaTipo: CadastroGeral, distanciaTubulacao: number, hideLoading?: boolean) {

        return this.makeGetPrommisse<number>('v1/bombaPreco'
            +'/usina/'+usina.codigo
            +'/bombaTipo/'+bombaTipo.codigo
            +'/distancia-tubulacao/'+distanciaTubulacao
            +'/valor-adicional',
            hideLoading);
        
    }

}