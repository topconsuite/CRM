import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';
import { Slump } from '../classes/traco/slump';

@Injectable()
export class SlumpService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }

    listarPorSlumpReal(slumpReal: Slump, hideLoading?: boolean) {
        return this.makeGetPrommisse<Slump[]>('v1/slumps/slumpReal/'+slumpReal.codigo, hideLoading);
    }

    listarTodos(hideLoading?: boolean) {
        return this.makeGetPrommisse<Slump[]>('v1/slumps', hideLoading);
    }

}