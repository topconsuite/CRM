export class ModeloDocumentoRemessaConcreto { codigo: number; descricao: string; }

export class Filial {
    codigo: number = 0;
    nome: string = '';
    RazaoSocial: string = '';
    permiteDocumentoDiferentePadraoRemessa: string = 'N';
    permiteDocumentoDiferentePadraoBomba: string = 'N';
}