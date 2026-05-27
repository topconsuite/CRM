import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';
import { Endereco, Municipio, DistanciaViaGoogleApi } from '../classes/endereco/endereco';
import { Usina } from '../classes/usina/usina';

@Injectable()
export class EnderecoService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }

    obterPorCep(cep: string, hideLoading?: boolean) {
        return this.makeGetPrommisse<Endereco>('v1/endereco/cep/'+cep, hideLoading);
    }

    listarMunicipios() {
        return this.makeGetPrommisse<Municipio[]>('v1/endereco/municipios');
    }

    usinaAtendeCep(usina: Usina, cep: string) {

        return this.makeGetPrommisse<boolean>('v1/endereco'
            +`/cep/${cep}`
            +`/usina/${usina.codigo}`
            +'/atende');

    }

    obterValorAdicionalM3PorUsinaCep(usina: Usina, cep: string) {

        return this.makeGetPrommisse<number>('v1/endereco'
            +`/cep/${cep}`
            +`/usina/${usina.codigo}`
            +'/valor-adicional');

    }

    obterDistanciaKmPorUsinaCep(usina: Usina, cep: string) {

        return this.makeGetPrommisse<number>('v1/endereco'
            +`/cep/${cep || '0'}`
            +`/usina/${usina.codigo}`
            +'/distancia-km');

    }

    obterDistanciaKmEntreUsinaEObraViaGoogleApi(usina: Usina, filtro: string, hideLoading?: boolean) {

        return this.makeGetPrommisse<DistanciaViaGoogleApi>('v1/endereco'
            +`/usina/${usina.codigo}`
            +'/distancia-km-sugerida-google'
            +`?destino=${filtro}`, hideLoading);

    }

    distanciaKmUsinaCepAprovada(usina: Usina, cep: string) {

        return this.makeGetPrommisse<boolean>('v1/endereco'
            +`/cep/${cep || '0'}`
            +`/usina/${usina.codigo}`
            +'/distancia-km-aprovada');

    }

}