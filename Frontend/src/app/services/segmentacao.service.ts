import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';
import { Segmentacao } from 'app/classes/segmentacao/segmentacao';

@Injectable()
export class SegmentacaoService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }

    listarTodos(hideLoading?: boolean) {
        return this.makeGetPrommisse<Segmentacao[]>('v1/segmentacao', hideLoading);
    }
}

