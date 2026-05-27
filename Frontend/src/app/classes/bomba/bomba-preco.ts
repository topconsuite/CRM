import { Usina } from '../usina/usina';
import { CadastroGeral } from '../cadastro-geral/cadastro-geral';

export enum EBombaM3CalculoTipo {
    taxaMinimaOuExcedente = 0,
    taxaMinimaMaisExcedente = 1,
    semCobranca = 9
}

export enum EBombaHoraCalculoTipo {
    taxaMinimaOuExcedente = 0,
    taxaMinimaMaisExcedente = 1,
    semCobranca = 9
}

export class BombaPreco {
    usina: Usina;
    bombaTipo: CadastroGeral;
    dataInicioVigencia: Date;
    m3Ate: number = 0;
    taxaMinimaPreco: number = 0.0;
    m3Preco: number = 0.0;
    m3AteValorMinimo: number = 0;
    taxaMinimaPrecoPercentualDescontoMaximo: number = 0.0;
    taxaMinimaPrecoValorMinimo: number = 0.0;
    m3PrecoPercentualDescontoMaximo: number = 0.0;
    m3PrecoValorMinimo: number = 0.0;
    tipoCalculo: EBombaM3CalculoTipo = EBombaM3CalculoTipo.taxaMinimaOuExcedente;

    horaAte: number = 0;
    horaTaxaMinimaPreco: number = 0.0;
    horaPreco: number = 0.0;
    horaAteValorMinimo: number = 0;
    horaTaxaMinimaPrecoValorMinimo: number = 0.0;
    horaPrecoValorMinimo: number = 0.0;
    horaTipoCalculo: EBombaHoraCalculoTipo = EBombaHoraCalculoTipo.semCobranca;
}