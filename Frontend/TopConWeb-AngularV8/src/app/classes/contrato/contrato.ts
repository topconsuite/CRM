import { ContratoTracoReajuste } from "./contrato-traco-reajuste";
import { ContratoBombaReajuste } from './contrato-bomba-reajuste';
import { CadastroGeral, Interveniente } from '../bomba/bomba.classes';
import { Funcionario } from '../funcionario/funcionario';
import { ContratoClicksignEnvio, EStatusClicksignDocumento } from "../assinatura-eletronica/contrato-clicksing-envio";

export enum EStatusContrato {
    Inexistente = 0,
    NaoGerado = 9131,
    PreAnalise = 9132,
    Reprovado = 9133,
    EmAnalise = 9134,
    Pendente = 9135,
    Aprovado = 9136,
    AgConfPagamento = 9137,
    Cancelado = 9138,
    AgDtProgramacao = 9139,
    RevalidaCadastro = 9140,
    AgDadosPagamento = 9141,
    AgAprovacaoComercial = 9144,
    Encerrado = 9145
}

export enum EContratoFinalidade {
    PrestacaoServico = 1,
    AmostraGratis = 2,
    Doacao = 3,
    Reposicao = 4
}

export const exibicaoContratos = [
    {codigo: 1, descricao: 'Ativos'},
    {codigo: 2, descricao: 'Encerrados'}
];

export const medicaoContratos = [
    {codigo: 'N', descricao: 'Não'},
    {codigo: 'S', descricao: 'Sim'}
]

export class Contrato{
    numero: number;
    ano: number;
    status: EStatusContrato;
    usina: number;
    interveniente: Interveniente;
    contratoTracoReajustes: ContratoTracoReajuste[] = [];
    contratoBombaReajustes: ContratoBombaReajuste[] = [];
    descricaoCoincidencia: string;
    aguardandoAprovacao: string;
    analista: Funcionario;
    fechadoSimNao: string;
    idAprovacaoEngenharia: string;
    aprovaEngenharia: string;
    dataEncerramento: Date;
    codigoObraPrefeitura: string;
    faturamentoPendente: boolean;
    statusClicksignDocumento: EStatusClicksignDocumento;
    modeloDocumentoRemessaConcreto: number;
    ModeloDocumentoRemessaBomba: number;
    modeloItensDanfeERomaneio: number;
    percentualRetencaoContratual: number;
    maoObraPropria: string;
    percentualLocacao: number;
    numeroContratoAnterior: string;   
    dataContrato: Date;
    inicioVigencia: Date;
    fimVigencia: Date;
    contratoFinalidade: EContratoFinalidade = EContratoFinalidade.PrestacaoServico;

    aprovacaoMedicao: string = medicaoContratos[0].codigo;
    tempoAprovacaoMedicaoCadastro: CadastroGeral;
    tempoAprovacaoMedicao: number = 0;

}


