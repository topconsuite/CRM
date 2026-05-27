export enum ELeadInteracaoTipo {
    AtividadeRealizada = 1,
    InteracaoCliente = 2,
    Chamada = 3,
    Todos = 99
}

export const LeadInteracaoTipos = [
    { codigo: ELeadInteracaoTipo.Todos, descricao: 'Todos', sigla: '' },
    { codigo: ELeadInteracaoTipo.AtividadeRealizada, descricao: 'Atividade Realizada', sigla: 'ATIVIDADE' },
    { codigo: ELeadInteracaoTipo.InteracaoCliente, descricao: 'Interação com Cliente', sigla: 'INTERACAO' },
    { codigo: ELeadInteracaoTipo.Chamada, descricao: 'Chamada', sigla: 'CHAMADA' },
];

export class LeadInteracao {
    usina: number = 0;
    numeroLead: number = 0;
    anoLead: number = 0;
    tipo: string = '';
    descricao: string = '';
    data: Date = new Date();
    hora: string = new Date().getHours().toString().padStart(2, '0') + ':' + new Date().getMinutes().toString().padStart(2, '0') + ':00';
    horaString: string = new Date().getHours().toString().padStart(2, '0') + new Date().getMinutes().toString().padStart(2, '0');
    dataRetorno: Date = null;
    horaRetorno: string = '';
    horaRetornoString: string = '';
    idCadastro: string = '';
}