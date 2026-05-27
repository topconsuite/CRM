export enum EVisitaHistoricoTipo {
    
    AtividadeRealizada = 1,
    InteracaoCliente = 2,
    Chamada = 3,
    Todos = 99

}

export const VisitaHistoricoTipos = [
    { codigo: EVisitaHistoricoTipo.Todos, descricao: 'Todos', sigla: '' },
    { codigo: EVisitaHistoricoTipo.AtividadeRealizada, descricao: 'Atividade Realizada', sigla: 'ATIVIDADE' },
    { codigo: EVisitaHistoricoTipo.InteracaoCliente, descricao: 'Interação com Cliente', sigla: 'INTERACAO' },
    { codigo: EVisitaHistoricoTipo.Chamada, descricao: 'Chamada', sigla: 'CHAMADA' },
];

export class VisitaHistorico {

    usina: number = 0;
    numeroVisita: number = 0;
    anoVisita: number = 0;
    
    tipoHistorico: EVisitaHistoricoTipo = EVisitaHistoricoTipo.AtividadeRealizada;
    descricao: string = '';

    data: Date = new Date();

    hora: string = new Date().getHours().toString().padStart(2, '0') + ':' + new Date().getMinutes().toString().padStart(2, '0') + ':00';
    horaString: string = new Date().getHours().toString().padStart(2, '0') + new Date().getMinutes().toString().padStart(2, '0');

    dataRetorno: Date = null;

    horaRetorno: string = '';
    horaRetornoString: string = '';

    idCadastro: string = '';

}