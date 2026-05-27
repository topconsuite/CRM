export class PeriodoAusenciaUsuario {
    codigo: number = 0;
    usuario: string = '';
    tipoLiberacao: string = '';
    tipoAusencia: string = '';
    inicioPeriodo: Date = new Date();
    fimPeriodo: Date = new Date();
    criadoEm: Date = new Date();
    atualizadoEm: Date = new Date();
    checked: boolean = false;
}