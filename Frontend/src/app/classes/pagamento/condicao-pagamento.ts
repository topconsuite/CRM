import { CadastroDiverso } from '../cadastro-geral/cadastro-diverso';

export class CondicaoPagamento {
    codigo: number = 0;
    descricao: string = '';
    quantidadeParcelas: number = 0;
    vencimento1Parcela: number = 0;
    vencimento2Parcela: number = 0;
    vencimento3Parcela: number = 0;
    vencimento4Parcela: number = 0;
    vencimento5Parcela: number = 0;
    vencimento6Parcela: number = 0;
    vencimento7Parcela: number = 0;
    vencimento8Parcela: number = 0;
    vencimento9Parcela: number = 0;
    vencimento10Parcela: number = 0;
    vencimento11Parcela: number = 0;
    vencimento12Parcela: number = 0;
    idCadastro: string = '';
    idAtualizacao: string = ''; 
    condicaoDaCobrancaCod: string = ''; 
    vencimentoFixoSemana: string = 'N'; 
    diaVencimentoFixoSemana: number = 0; 
    diaUltimoVencimento: number = 0; 
    analisaFraude: string = 'N'; 
    ativo: string = 'N'; 
    mesFixo30Dias: string = 'N'; 
    retencaoPrimeiraParcela: string = 'N'; 
    tiposDeCobrancaCodigos: string = ''; 
    condicaoDaCobranca: CadastroDiverso;
    parcelas: CondicaoPagamentoParcelas[] = [];
    mediaDias: number = 0.0;
}


export class CondicaoPagamentoParcelas {
    condicaoPagamentoCodigo: number = 0;
    dias: number = 0;
    percentual: number = 0.00;
}
