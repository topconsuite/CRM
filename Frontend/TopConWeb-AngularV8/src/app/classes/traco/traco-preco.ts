import { Usina, Uso, Pedra, Slump, ResistenciaTipo } from './traco.classes';
import { Vendedor } from '../vendedor/vendedor';

export class TracoPreco {
    numeroTabela: number = 0;
    usinaBase: Usina;
    vendedorRepresentante: Vendedor;
    uso: Uso;
    resistenciaTipo: ResistenciaTipo;
    mpa: number = 0.0;
    consumo: number = 0;
    pedra: Pedra;
    slump: Slump;
    m3Preco: number = 0.0;
    dataInicioVigencia: Date;
    dataFinalVigencia: Date;
    m3PrecoRecalculo: number = 0.0;
    percentualVariacao: number = 0.0;
    dataRecalculo: Date;
    custoMaterial: number = 0.0;
    markup: number = 0.0
    tracoEspecificacao: string = '';
    usinaReferencia: Usina;
    numeracaoProduto: number = 0;
}