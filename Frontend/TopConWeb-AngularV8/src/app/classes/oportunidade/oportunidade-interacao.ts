export enum EOportunidadeInteracaoTipo {
    AtividadeRealizada = 1,
    InteracaoCliente = 2,
    Chamada = 3,
    Todos = 99
}

export const OportunidadeInteracaoTipos = [
    { codigo: EOportunidadeInteracaoTipo.Todos, descricao: 'Todos', sigla: '' },
    { codigo: EOportunidadeInteracaoTipo.AtividadeRealizada, descricao: 'Atividade Realizada', sigla: 'ATIVIDADE' },
    { codigo: EOportunidadeInteracaoTipo.InteracaoCliente, descricao: 'Interação com Cliente', sigla: 'INTERACAO' },
    { codigo: EOportunidadeInteracaoTipo.Chamada, descricao: 'Chamada', sigla: 'CHAMADA' },
];

export class OportunidadeInteracao {
    usina: number = 0;
    numeroOportunidade: number = 0;
    anoOportunidade: number = 0;
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