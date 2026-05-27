import { Contrato } from 'app/classes/contrato/contrato';

export class BoletoExterno extends Contrato{
    id: string = '';
    chave: string = '';
    sequencia: number = 0;
    nomeArquivo: string ='';
    dataHora: Date;
}