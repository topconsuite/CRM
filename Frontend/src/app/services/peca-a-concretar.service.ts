import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';

@Injectable()
export class PecaAConcretarService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }

    listarTodos() {
        return this.makeGetPrommisse<{descricao: string}[]>('v1/pecasAConcretar')
            .then(res => res.map(t => t.descricao), err => err);
    }

}