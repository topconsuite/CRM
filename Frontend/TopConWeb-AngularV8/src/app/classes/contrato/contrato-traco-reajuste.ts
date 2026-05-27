import { Uso } from "../traco/uso";
import { ResistenciaTipo, Pedra, Slump } from "../traco/traco.classes";

export class ContratoTracoReajuste{
    obraTracoSequencia: number = 0;
    contratoAno: number = 0;
    contratoNumero: number = 0;
    uso: Uso;
    resistenciaTipo: ResistenciaTipo;
    mpa: number = 0.0;
    consumo: number = 0;
    pedra: Pedra;
    slump: Slump;
    dataVigencia: Date = new Date();
    precoRecalculado: number = 0.0;
    valorServicoRecalculado: number = 0.0;
    custoRecalculado: number = 0.0;
    dataConfirmacao: Date;
}