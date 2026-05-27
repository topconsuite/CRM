export class MovimentoBanco {
    id: number = 0;
    empresaCodigo: number = 0;
    contaCodigo: number = 0;
    dataOperacao: Date;
    documentoTipo: number = 0;
    documentoNumero: string = '';
    entradaSaida: string = '';
    operacaoCodigo: number = 0;
    operacaoDescricao: string = '';
    valor: number = 0.0;
    saldo: number = 0.0;
    origem: string = '';
    centroCustoCodigo: number = 0;
    idCadastro: string = '';
    observacao: string = '';
}