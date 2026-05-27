import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';
import { CadastroGeral } from '../classes/cadastro-geral/cadastro-geral';

@Injectable()
export class CadastroGeralService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }

    listarFuncoes() {
        return this.makeGetPrommisse<CadastroGeral[]>('v1/cadastroGeral/funcoes');
    }

    listarViasCaptacao() {
        return this.makeGetPrommisse<CadastroGeral[]>('v1/cadastroGeral/viasCaptacao');
    }

    listarmotivosBloqueioInterveniente(){
        return this.makeGetPrommisse<CadastroGeral[]>('v1/cadastroGeral/motivosBloqueioInterveniente');
    }

    listarTipoObra() {
        return this.makeGetPrommisse<CadastroGeral[]>('v1/cadastroGeral/tipo-obra');
    }

    listarPorteObra() {
        return this.makeGetPrommisse<CadastroGeral[]>('v1/cadastroGeral/porte-obra');
    }

    listarTemposAprovacaoMedicao() {
        return this.makeGetPrommisse<CadastroGeral[]>('v1/cadastroGeral/temposAprovacaoMedicao');
    }

}