import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';
import { Usina, ParametroProgramacao } from '../classes/usina/usina';

@Injectable()
export class UsinaService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }

    listarTodos() {
        return this.makeGetPrommisse<Usina[]>('v1/usinas');
    }

    listarPorEmpresa(empresa: number){
        return this.makeGetPrommisse<Usina[]>(`v1/usinas-por-empresa/${empresa}`)
    }

    obterParametroProgramacao(usina: Usina) {
        return this.makeGetPrommisse<ParametroProgramacao>(`v1/usina/${usina.codigo}/parametro-programacao`);
    }

    usinaAtendeKm(usina: Usina, km: number) {

        return this.makeGetPrommisse<boolean>('v1/usina'
            +`/${usina.codigo}`
            +`/distancia-km/${km}`
            +'/atende');

    }

    obterValorAdicionalM3PorUsinaKm(usina: Usina, km: number) {

        return this.makeGetPrommisse<number>('v1/usina'
            +`/${usina.codigo}`
            +`/distancia-km/${km}`
            +'/valor-adicional');

    }

    listarListarUsinasPermitidasUsuario() {
        return this.makeGetPrommisse<Usina[]>('v1/usinas-permitidas');
    }

}