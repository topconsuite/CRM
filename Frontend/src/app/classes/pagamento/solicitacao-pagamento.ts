import { CartaoBandeira } from './cartao-bandeira';
import { Obra } from '../obra/obra';

export class SolicitacaoPagamento {
    cartaoBandeira: CartaoBandeira;
    tipoCobranca: string = "";
    quantidadeParcelas: number = 1;
    valorTotal: number = 0;
    cpfCnpj: string = "";
    intervenienteRazao: string = "";
    obra: Obra;
    observacao: string = "";
}