import { Usina } from './../usina/usina';
import { Proposta } from './../proposta/proposta';
import { Endereco } from '../endereco/endereco';
import { ResistenciaTipo, Pedra, Slump, Uso } from '../traco/traco.classes';
import { Vendedor } from '../vendedor/vendedor';
import { ProgramacaoDemaisServicos } from './programacao-demais-servicos';

export class Status { codigo: number; descricao: string; color: string}

export enum EProgramacaoConfirmacao {
    Sim = 'S',
    Nao = 'N',
    Parcial = 'P'
}

export enum EProgramacaoTipoAlteracao {

    Insert = 1,
    Copy = 2,
    Update = 3

}

export const confirmacaoOpcoes: { codigo: EProgramacaoConfirmacao; descricao: string; }[] = [
    {codigo: EProgramacaoConfirmacao.Sim, descricao: "Sim" },
    {codigo: EProgramacaoConfirmacao.Nao, descricao: "Não" },
    {codigo: EProgramacaoConfirmacao.Parcial, descricao: "Parcial" }
];

export class CorpoDeProvaMoldador { codigo: number; descricao: string; }
export const corpoDeProvaMoldadores: CorpoDeProvaMoldador[] = [
    {codigo: 1, descricao: "MOTORISTA"},
    {codigo: 2, descricao: "LABORATÓRIO"}
];

export class CorpoDeProvaTipo { descricao: string; }
export const corpoDeProvaTipos: CorpoDeProvaTipo[] = [
    {descricao: "Nenhum"},
    {descricao: "Parcial"},
    {descricao: "Total"}
];

export class CorpoDeProvaMoldagemRemota { codigo: number; descricao: string; }
export const corpoDeProvaMoldagemRemota: CorpoDeProvaMoldagemRemota[] = [
    {codigo: 1, descricao: "Padrao da Central"},
    {codigo: 2, descricao: "Na usina"},
    {codigo: 3, descricao: "Na obra"}
];

export enum EProgramacaoStatus {
    AguardandoConfirmacao = 9161,
    Programado = 9162,
    Rejeitado = 9163,
    Cancelada = 9164,
    Revalidacao = 9165,
    AguardandoAnaliseLimiteCredito = 9166,
    LimiteCreditoInsuficiente = 9167,
    AprovacaoInadimplente = 9168
}

export const statusProgramacao: Status[] = [
    {codigo: EProgramacaoStatus.AguardandoConfirmacao , descricao: "Aguardando confirmação", color:'#ffc800' },
    {codigo: EProgramacaoStatus.Programado , descricao: "Programado", color:'#00a500' },
    {codigo: EProgramacaoStatus.Rejeitado , descricao: "Rejeitado", color:'#FF8072' },
    {codigo: EProgramacaoStatus.Cancelada , descricao: "Cancelada" , color:'#FF8072'},
    {codigo: EProgramacaoStatus.Revalidacao , descricao: "Revalidação de programação", color:'#ffc800' },
    {codigo: EProgramacaoStatus.AguardandoAnaliseLimiteCredito , descricao: "Aguardando Análise de Limite de Crédito", color: 'FF8072' },
    {codigo: EProgramacaoStatus.LimiteCreditoInsuficiente , descricao: "Limite de Crédito Insuficiente", color: '#FF8072' },
    {codigo: EProgramacaoStatus.AprovacaoInadimplente, descricao: "Aguardando Aprovação de Inadimplência.", color:'#FF8072' }
];

export class Programacao{
    usina: Usina;
    contratoAno: number = 0;
    contratoNumero: number = 0;
    sequencia: number = 0;
    propostaAno: number = 0;
    propostaNumero: number = 0;
    proposta: Proposta;
    obraNumero: number = 0;
    obraNome: string = '';
    usinaEntrega: Usina;
    endereco: Endereco;

    dataConcretagem: Date;
    horario: string = '';
    necessitaConfirmacao: EProgramacaoConfirmacao = EProgramacaoConfirmacao.Sim;
    volumeLiberado: number = 0;
    intervaloEmMinutosEntreCargas: number = 0;
    
    resistenciaTipo: ResistenciaTipo
    mpa: number = 0;
    consumo: number = 0;
    pedra: Pedra;
    slump: Slump;
    slumpNotaFiscal: string;
    uso: Uso;
    pecaConcretar: string;
    andar: string;
    obraTracoSequencia: number = 0;
    
    volumeTotal: number = 0;
    volumeEntregue: number = 0;
    volumePorCarga: number = 0;
    
    tracoPesadoResistenciaTipo: ResistenciaTipo;
    tracoPesadoMpa: number = 0;
    tracoPesadoConsumo: number = 0;
    tracoPesadoPedra: Pedra;
    tracoPesadoSlump: Slump;
    tracoPesadoUso: Uso;
    
    obraBombaSequencia: number = 0;
    distanciaTubulacao: number = 0;
    horarioBomba: string = '';

    demaisServicos: ProgramacaoDemaisServicos[] = [];

    vibradorQuantidade: number = 0;
    vibradorValorUnitario: number = 0;
    vibradorValorTotal: number = 0;
    vibradorVendedor: Vendedor;
    
    status: EProgramacaoStatus = EProgramacaoStatus.AguardandoConfirmacao;

    solicitante: string = '';
    observacao: string = '';

    observacaoInterna: string = '';

    corpoDeProvaTipo: string = 'Nenhum';
    corpoDeProvaQuantidade: number = 0;
    corpoDeProvaIntervalo: number = 0;
    corpoDeProvaMoldador: number = 0;
    corpoDeProvaMoldagemRemota: string = '';

    temNotaFicalEmitida: boolean = false;

    obraFrenteSequencia: number = 0;

    valorTotal: number = 0;
    valorConcreto: number = 0;
    valorExtras: number = 0;
    valorBomba: number = 0;
    valorDemaisServicos: number = 0;

    valorTotalRemessasEmitidas: number = 0;

    tempoAteAObra: number = 0;
    tempoBtNaObra: number = 0;
    tempoDescarga: number = 0;
}