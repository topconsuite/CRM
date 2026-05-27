import { Contrato } from "../contrato/contrato";
import { Obra } from "../obra/obra";
import { Usina } from "../usina/usina";
import { ObraBombaReajuste } from "./obra-bomba-reajuste";
import { ObraTracoReajuste } from "./obra-traco-reajuste";

export class StatusReajusteContrato { codigo: number; descricao: string }

export enum EStatusReajusteContrato {
    Aprovado = 1,
    Pendente = 2,
    Reprovado = 3
}

export const statusReajusteContrato: StatusReajusteContrato[] = [
    { codigo: EStatusReajusteContrato.Aprovado, descricao: 'Aprovado' },
    { codigo: EStatusReajusteContrato.Pendente, descricao: 'Pendente' },
    { codigo: EStatusReajusteContrato.Reprovado, descricao: 'Reprovado' }
]

export class ContratoReajuste {
    dataVigencia: Date;
    usinaEntrega: Usina;
    usinaEntregaCodigo: number = 0;
    usinaCodigo: number = 0;
    contratoAno: number = 0;
    contratoNumero: number = 0;
    contrato: Contrato;
    idAprovacaoVersao: string = '';
    idReprovacao: string = '';
    dataConfirmacao: Date;
    dataCarta: Date;
    obraNome: string = '';
    tracos: ObraTracoReajuste[] = [];
    bombas: ObraBombaReajuste[] = []
}