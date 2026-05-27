export class Tarefa {
    codigo: number = 0;
    usuario: string = '';
    descricao: string = '';
    diaInteiro: boolean;
    data: Date = new Date();

    horario: string = '';
    horarioString: string = '';

    observacao: string = '';
    contato: string = '';
    dddTelefone: number = 0; 
    telefone: number = 0;
    dddCelular: number = 0; 
    celular: number = 0;
    email: string = '';
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

    public clone(): Tarefa {
        const novaTarefa = new Tarefa();
        Object.assign(novaTarefa, this);
        return novaTarefa;
    }

}