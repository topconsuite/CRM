import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';
import { LeadFase } from 'app/classes/lead/lead-fase';

@Injectable()
export class LeadFaseService extends BaseService {
    constructor(injector: Injector) {
        super(injector);
    }

    listarFases(){
        return this.makeGetPrommisse<LeadFase[]>('v1/lead/lead-fase');
    }
}