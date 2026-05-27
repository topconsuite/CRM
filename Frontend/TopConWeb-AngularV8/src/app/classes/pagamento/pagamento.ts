import { CondicaoPagamento } from './condicao-pagamento';
import { TipoCobranca } from './tipo-cobranca';
import { CartaoBandeira } from './cartao-bandeira';
import { Portador } from './../banco/portador';

export class Pagamento {
    sequencia: number = 0;
    condicaoPagamento: CondicaoPagamento;
    tipoCobranca: TipoCobranca;
    valor: number = 0;
    detalhes: IPagamentoDetalhe[] = [];
    necessitaAprovacao: boolean = false;
    idAprovacao: string = '';
    valorApropriado: number = 0;
    ativoSimNao: string = 'S';
}

export interface IPagamentoDetalhe {
    detalheSequencia: number;
    valor: number;
}
export class PagamentoDetalheCartao implements IPagamentoDetalhe {
    detalheSequencia: number = 0;
    bandeira: CartaoBandeira;
    numeroCartao: number = 0;
    dataTransacao: Date;
    valor: number = 0;
    quantidadeParcelas: number = 1;
    numeroAutorizacao: string = '';
}
export class PagamentoDetalheDeposito implements IPagamentoDetalhe {
    detalheSequencia: number = 0;
    dataDeposito: Date;
    portador: Portador;
    numeroTerminal: number = 0
    valor: number = 0;
    idAprovacao: string = '';
}
export class PagamentoDetalheBoleto implements IPagamentoDetalhe {
    detalheSequencia: number = 0;
    dataVencimento: Date;
    dataHoraImpressao: Date;
    nossoNumero: string;
    linhaDigitavel: string;
    codigoDeBarras: string;
    dataRemessa: Date;
    dataLiquidacao: Date;
    valor: number;
    valorLiquidacao: number;
    idLiquidacao: string;
}
export class PagamentoDetalheDinheiro implements IPagamentoDetalhe {
    detalheSequencia: number = 0;
    numeroRecibo: string = '';
    dataPagamento: Date;
    valor: number = 0;
}
export class PagamentoDetalheCheque implements IPagamentoDetalhe {
    detalheSequencia: number = 0;
    bancoCodigoOficial: number = 0;
    bancoAgencia: number = 0;
    bancoContaNumero: number = 0;
    bancoContaDV: number = 0;
    numeroCheque: number = 0;
    dataRecebimento: Date;
    valor: number = 0;
    observacao: string = '';
    dataBomPara: Date;
}