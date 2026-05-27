import { Usina } from '../usina/usina';
import { Mercadoria, Unidade } from '../mercadoria/mercadoria';

export enum EFrequenciaDeCobranca {
	Contrato = 1,
	Programacao,
	Remessa, 
	M3,
	M3Bombeado,
	Bombeamento
}

export enum EFormaDeCobrancaDemaisServicos {
	NaRemessa = 1,
	FinalConcretagem,
}

export class DemaisServicos {
    codigo: number = 0;
    usina: Usina;
    mercadoria: Mercadoria;
    unidade: Unidade;
    numeroDeCasasDecimais: number = 0;
    precoSugerido: number = 0;
    precoMinimo: number = 0;
    frequenciaDeCobranca: EFrequenciaDeCobranca;
    formaDeCobranca: EFormaDeCobrancaDemaisServicos = EFormaDeCobrancaDemaisServicos.NaRemessa;
    atualizaEstoque: string = '';
}