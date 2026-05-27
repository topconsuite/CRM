export class LiberacaoAcesso {
    codigo: number = 0;
    usuario: string = '';
    grupo: number = 0;
    tipoLiberacao: string = '';
    diaSemana: string = '';
    turno: number = 0;
    horaEntrada: Date = new Date();
    horaSaida: Date = new Date();
    bloquear: boolean = false;
    criadoEm: Date = new Date();
    atualizadoEm: Date = new Date();
}

export class Usuario { 
    id: string = '';
    nome: string = '';
}