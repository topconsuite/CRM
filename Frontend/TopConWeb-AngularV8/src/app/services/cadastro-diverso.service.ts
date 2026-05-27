import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';
import { CadastroDiverso } from '../classes/cadastro-geral/cadastro-diverso';

@Injectable()
export class CadastroDiversoService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }

    listarAndares() {
        return this.makeGetPrommisse<CadastroDiverso[]>('v1/cadastro-diverso/andares');
    }

    listarCondicoesPagamentos() {
        return this.makeGetPrommisse<CadastroDiverso[]>('v1/cadastro-diverso/condicoes-pagamento');
    }

    listarDiasDaSemanaFixo() {
        return this.makeGetPrommisse<CadastroDiverso[]>('v1/cadastro-diverso/dias-semana-fixo');
    }

    listarOpcoesDeVencimentoEmDiaNaoUtil() {
        return this.makeGetPrommisse<CadastroDiverso[]>('v1/cadastro-diverso/opcoes-vencimento-dia-nao-util');
    }

    listarQuantidadeDeCorposDeProva() {
        return this.makeGetPrommisse<CadastroDiverso[]>('v1/cadastro-diverso/quantidade-de-corpos-de-prova');
    }

    listarModeloDocumentoRemessaConcreto() {
        return this.makeGetPrommisse<CadastroDiverso[]>('v1/cadastro-diverso/modelo-documento-remessa-concreto');
    }

}