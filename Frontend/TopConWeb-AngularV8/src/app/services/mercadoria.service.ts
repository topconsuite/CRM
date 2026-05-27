import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';
import { Mercadoria } from '../classes/mercadoria/mercadoria';
import { Usina } from 'app/classes/usina/usina';
import { Uso } from 'app/classes/traco/uso';
import { Pedra } from 'app/classes/traco/pedra';
import { Slump } from 'app/classes/traco/slump';
import { ResistenciaTipo } from 'app/classes/traco/traco.classes';
import { TracoParticularidades } from 'app/classes/traco/traco-particularidades';

@Injectable()
export class MercadoriaService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }

    listarProdutosEServicosFiltrados(filtro?: string, hideLoading?: boolean) {
        return this.makeGetPrommisse<Mercadoria[]>('v1/mercadorias/produtos-e-servicos'
            +(filtro ? '?'+filtro : ''), hideLoading);
    }

    obterTracoMercadoriaComDescricaoPersonalizada
        (uso: Uso, pedra: Pedra, slump: Slump, resistenciaTipo: ResistenciaTipo, mpa: number, consumo: number, hideLoading?: boolean) {
            return this.makeGetPrommisse<Mercadoria>('v1/mercadorias/traco-desc-personalizado'
                +'/uso/'+uso.codigo
                +'/pedra/'+pedra.codigo
                +'/slump/'+slump.codigo
                +'/resistenciaTipo/'+resistenciaTipo.codigo
                +'/mpa/'+mpa
                +'/consumo/'+consumo,
                hideLoading);
    }

}