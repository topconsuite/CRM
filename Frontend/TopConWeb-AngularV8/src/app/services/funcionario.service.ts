import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';
import { Funcionario } from '../classes/funcionario/funcionario';

@Injectable()
export class FuncionarioService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }

    ListarAnalistas(hideLoading?: boolean) {
        return this.makeGetPrommisse<Funcionario[]>('v1/funcionarios/analistas', hideLoading);
    }
}