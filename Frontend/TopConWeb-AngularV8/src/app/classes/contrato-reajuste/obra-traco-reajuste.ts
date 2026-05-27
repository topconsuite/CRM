import { Pedra } from "../traco/pedra";
import { ResistenciaTipo } from "../traco/resistencia-tipo";
import { Slump } from "../traco/slump";
import { Uso } from "../traco/uso";

export class ObraTracoReajuste {
    dataVigencia: Date;
    usinaCodigo: number;
    contratoAno: number;
    contratoNumero: number;
    uso: Uso;
    pedra: Pedra;
    slump: Slump;
    resistenciaTipo: ResistenciaTipo;
    mpa: number;
    consumo: number;
    precoVigente: number;
    precoRecalculado: number;
    descricaoPersonalizada: string;
    descricao: string;
}