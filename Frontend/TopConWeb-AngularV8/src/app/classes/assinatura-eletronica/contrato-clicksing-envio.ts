import { Status } from "../proposta/proposta";

export class ContratoClicksignEnvio {
    id: string;
    contratoAno: number;
    contratoNumero: number;
    contratoUsina: number;
    idCLicksign: string;
    DataEnvio: Date;
    DataRetorno: Date;
    DataCancelamento: Date;
    idEnvio: string;
    idCancelamento: string;
    statusClicksignDocumento: EStatusClicksignDocumento;
}

export enum EStatusClicksignDocumento{
    Assinado = 0,
    Processando = 1,
    NaoEnviado = 2,
    Cancelado = 3
}

export const statusClicksignDocumento: Status[] = [
    {codigo: EStatusClicksignDocumento.NaoEnviado, descricao: 'Solicitar Assinatura Eletronica', color: ''},
    {codigo: EStatusClicksignDocumento.Processando, descricao: 'Documento Em Processamento', color: '#ffc800'},
    {codigo: EStatusClicksignDocumento.Assinado, descricao: 'Documento Assinado', color: '#00a500'},
    {codigo: EStatusClicksignDocumento.Cancelado, descricao: 'Documento Cancelado', color: '#FF8072'}
];