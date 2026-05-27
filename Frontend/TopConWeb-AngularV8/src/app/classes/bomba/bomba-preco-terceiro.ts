import { Interveniente } from '../interveniente/interveniente';
import { CadastroGeral } from '../cadastro-geral/cadastro-geral';
import { EBombaM3CalculoTipo } from './bomba-preco';

export class BombaPrecoTerceiro {
    bombista: Interveniente;
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
}