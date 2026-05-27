import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';
import { BoletoExterno } from 'app/classes/contrato/contrato-boleto-externo';

@Injectable()
export class BoletoExternoService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }

    ListarBoletosExternos(usinaCodigo: number, contratoAno: number, contratoNumero: number, hideLoading?: boolean) {
        return this.makeGetPrommisse<BoletoExterno[]>(`v1/boletosExternos`
            +`/usina/${usinaCodigo}`
            +`/ano/${contratoAno}`
            +`/numero/${contratoNumero}`, hideLoading);
    }

    ObterArquivoBoletoExterno(arquivo: BoletoExterno, hideLoading?: boolean){
        return this.makeGetPrommisse<string>(
            'v1/boletosExternos/arquivo'
            +'/id/' + arquivo.id
            +'/chave/'+ arquivo.chave
            +'/sequencia/'+ arquivo.sequencia);
    }
}