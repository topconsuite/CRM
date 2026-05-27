import { Injectable, Injector } from '@angular/core';

import { BaseService } from './base.service';

import { CadastroDiverso } from '../classes/cadastro-geral/cadastro-diverso';
import { Usina } from '../classes/usina/usina';
import { ObraTaxa, TaxaTipos } from '../classes/obra/obra-taxa';
import { Segmentacao } from 'app/classes/segmentacao/segmentacao';

@Injectable()
export class ObraTaxaService extends BaseService {

    private _volumePorCargaDefault: number = 8.0;

    constructor(injector: Injector) {
        super(injector);
    }

    listarOpcoes(campo: string) {
        return this.makeGetPrommisse<CadastroDiverso[]>(`v1/obra-taxa/opcoes/${campo}`);
    }

    listarTaxaPadraoByIdUsina(usina: Usina) {
        return this.makeGetPrommisse<ObraTaxa[]>(`v1/obra-taxas/usina/${usina.codigo}`);
    }

    listarTaxaPadraoByIdUsinaSegmento(usina: Usina, segmentoId: number) {
        return this.makeGetPrommisse<ObraTaxa[]>(`v1/obra-taxas/usina/${usina.codigo}/segmento/${segmentoId}`);
    }

    obterValorM3Faltante(temBomba: boolean, volumeTotal: number, volumePorCarga: number, obraTaxas: ObraTaxa[]): number {
        var calculoPorM3FaltanteBombeado = obraTaxas.filter(t => t.tipo === TaxaTipos.M3_FALTANTE_BOMBEADO).length > 0 && temBomba;

        var taxaM3Faltante = obraTaxas.find(t => t.tipo === (calculoPorM3FaltanteBombeado ? TaxaTipos.M3_FALTANTE_BOMBEADO : TaxaTipos.M3_FALTANTE));

        if (taxaM3Faltante)
        {
            if (taxaM3Faltante.selecionada === "S" && volumeTotal > 0)
            {
                var volumeMinimo = parseFloat(taxaM3Faltante.volume);

                var menorVolume = (volumePorCarga > 0 ? Math.min(volumePorCarga, volumeTotal) : 0);
                var volumeUltimaCarga = volumeTotal % volumePorCarga;

                if (volumeUltimaCarga > 0 && volumeUltimaCarga < menorVolume)
                    menorVolume = volumeUltimaCarga;

                if (menorVolume === 0) menorVolume = volumePorCarga;

                if (volumePorCarga > 0 && menorVolume < volumeMinimo)
                {
                    var quantidadeCargas = Math.floor(volumeTotal / volumePorCarga);
                    
                    var valorTaxa = (volumeMinimo > volumePorCarga ? (volumeMinimo - volumePorCarga) * taxaM3Faltante.valor * quantidadeCargas : 0);

                    if (volumeUltimaCarga > 0.1)
                    {
                        valorTaxa += (volumeMinimo > volumeUltimaCarga ? (volumeMinimo - volumeUltimaCarga) * taxaM3Faltante.valor : 0);
                    }

                    return valorTaxa;
                }
                else if (volumePorCarga === 0 && volumeTotal < volumeMinimo)
                {
                    return (volumeMinimo - volumeTotal) * taxaM3Faltante.valor;
                }
            }
        }

        return 0;
    }
    
    obterValorAdicionalPorKmRodado(distanciaUsina: number, volumeTotal: number, volumePorCarga: number, obraTaxas: ObraTaxa[], temBomba: boolean): number {
        var taxaAdicionalKmRodado = obraTaxas.find(t => t.tipo === TaxaTipos.ADICIONAL_KM_RODADO);

        if (taxaAdicionalKmRodado)
        {
            if (taxaAdicionalKmRodado.selecionada === "S" && volumeTotal > 0)
            {
                var kmRodadoPorCarga = (distanciaUsina * 2);

                if (kmRodadoPorCarga > taxaAdicionalKmRodado.acimaDe) {
                    var volumeCarga = (volumePorCarga || this._volumePorCargaDefault);
                    var quantidadeCargas = Math.floor(volumeTotal / volumeCarga);

                    if (temBomba) quantidadeCargas++;
                   
                    var volumeUltimaCarga = volumeTotal % volumeCarga;
                    if (volumeUltimaCarga > 0) quantidadeCargas++;

                    return (quantidadeCargas * kmRodadoPorCarga * taxaAdicionalKmRodado.valor);
                }
            }
        }

        return 0;
    }

}