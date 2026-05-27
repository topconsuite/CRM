import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';

@Injectable()
export class RelatorioService extends BaseService {

    constructor(injector: Injector) {
        super(injector);
    }

    ObterUrlRelatorioProducaoAnalitico(filtro: string, detalharAdicionais: boolean, detalharViaCaptacaoIndicadores: boolean): string {
        return this.apiBaseUrlService.getUrl()
            +`v1/relatorio/producao-analitico?${filtro}&detalharAdicionais=${detalharAdicionais}&detalharViaCaptacao=${detalharViaCaptacaoIndicadores}`;
    }

    ObterUrlRelatorioProducaoSintetico(filtro: string): string {
        return this.apiBaseUrlService.getUrl()
            +`v1/relatorio/producao-sintetico?${filtro}`;
    }

    ObterUrlRelatorioProducaoPorProgramacao(filtro: string, detalharViaCaptacaoIndicadores: boolean): string {
        return this.apiBaseUrlService.getUrl()
            +`v1/relatorio/producao-por-programacao?${filtro}&detalharViaCaptacao=${detalharViaCaptacaoIndicadores}`;
    }

    ObterUrlRelatorioProducaoVolume(filtro: string): string {
        return this.apiBaseUrlService.getUrl()
            +`v1/relatorio/producao-volume?${filtro}`;
    }

}