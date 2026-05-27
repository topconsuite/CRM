import { Time } from "@angular/common";

export class Compromisso {
    codigo: number = 0;
    usuario: string = '';
    descricao: string = '';
    diaInteiro: boolean;
    dataInicio: Date = new Date();
    
    horaInicio: string = '';
    horaInicioString: string = '';

    dataFim: Date = new Date();
    horaFim: string = '';
    horaFimString: string = '';

    local: string = '';
    contato: string = '';
    dddTelefone: number = 0;
    telefone: number = 0;
    dddCelular: number = 0;
    celular: number = 0;
    email: string = '';
    observacao: string = '';
    finalizado: boolean;
    providencia: string = '';
    conclusao: string = '';
    usinaCodigo: number = 0;
    anoVisita: number = 0;
    numeroVisita: number = 0;
    anoLead: number = 0;
    numeroLead: number = 0;
    anoOportunidade: number = 0;
    numeroOportunidade: number = 0;
    dataCriacao: Date = new Date();
    idAgrupamento: string = '';

    public clone(): Compromisso {
        const novoCompromisso = new Compromisso();
        Object.assign(novoCompromisso, this);
        return novoCompromisso;
    }
}