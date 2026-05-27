import { Pedra } from "../traco/pedra";
import { ResistenciaTipo } from "../traco/resistencia-tipo";
import { Slump } from "../traco/slump";
import { TracoPreco } from "../traco/traco-preco";
import { Uso } from "../traco/uso";
import { Usina } from "../usina/usina";

export class PreTracoPreco {

    id: string = '';
    
    usinaCodigo: number = 0;
    usina: Usina;

    usoCodigo: number = 0;
    uso: Uso;

    pedraCodigo: number = 0;
    pedra: Pedra;

    slumpCodigo: number = 0;
    slump: Slump;

    resistenciaTipoCodigo: number = 0;
    resistenciaTipo: ResistenciaTipo;

    mpa: number = 0;
    consumo: number = 0;

    custoMaterial: number = 0;
    valorServico: number = 0;
    markup: number = 0;
    m3Preco: number = 0;
    idCiencia: string = '';
    dataCiencia: Date;

    tracoEspecificacao: string = '';
    externalID: string = '';

    createdAt: Date;
    updatedAt: Date;

    numeracaoProduto: number = 0;

    tracoPrecoVigente: TracoPreco

}
