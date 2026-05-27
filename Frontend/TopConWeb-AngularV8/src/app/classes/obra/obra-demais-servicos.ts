import { Usina } from '../usina/usina';
import { Mercadoria, Unidade } from '../mercadoria/mercadoria';
import { EFrequenciaDeCobranca, EFormaDeCobrancaDemaisServicos } from '../demais-servicos/demais-servicos';

export class ObraDemaisServicos {
    usinaCodigo: number = 0;
    obraNumero: number = 0;
    sequencia: number = 0;
    codigo: number = 0;
    usinaEntrega: Usina;
    mercadoria: Mercadoria;
    unidade: Unidade;
    numeroDeCasasDecimais: number = 0;
    precoSugerido: number = 0;
    precoMinimo: number = 0;
    frequenciaDeCobranca: EFrequenciaDeCobranca;
    formaDeCobranca: EFormaDeCobrancaDemaisServicos;
    atualizaEstoque: string = '';
    precoProposto: number = 0;
    quantidade: number = 0;
}