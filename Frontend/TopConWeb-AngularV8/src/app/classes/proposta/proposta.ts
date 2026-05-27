import { Usina } from './../usina/usina';
import { Vendedor } from './../vendedor/vendedor';
import { Interveniente, intervenienteTipos } from './../interveniente/interveniente';
import { Obra } from './../obra/obra';
import { Endereco } from './../endereco/endereco';
import { PropostaDadosPessoais } from './proposta-dados-pessoais';

import { EContratoFinalidade, EStatusContrato, medicaoContratos } from './../contrato/contrato'
import { ESituacaoAprovacaoComercialAlcadaUsuario } from '../obra/obra-consulta';
import { Segmentacao } from '../segmentacao/segmentacao';
import { CadastroGeral } from '../cadastro-geral/cadastro-geral';

export class Status { codigo: number; descricao: string; color: string; }

export class ModeloDocumentoDanfeRomaneio { codigo: number; descricao: string; }

export enum EStatusProposta{
    EmNegociacao = 1,
    Aprovado = 2,
    Perdido = 3
}

export enum EStatusComercial{
    NaoNecessita = 0,
    Aguardando = 1,
    Aprovado = 2,
    Reprovado = 3
}

export enum EStatusObraTaxa{
    Nova = 0
}

export const statusProposta: Status[] = [
    {codigo: EStatusProposta.EmNegociacao, descricao: 'Em Negociação', color: '#ffc800'},
    {codigo: EStatusProposta.Aprovado, descricao: 'Aprovado', color: '#00a500'},
    {codigo: EStatusProposta.Perdido, descricao: 'Perdido', color: '#FF8072'}
];

export const statusComercial: Status[] = [
    {codigo: EStatusComercial.NaoNecessita, descricao: 'Não necessita', color: '#CCCCCC'},
    {codigo: EStatusComercial.Aguardando, descricao: 'Aguardando aprovação', color: '#ffc800'},
    {codigo: EStatusComercial.Aprovado, descricao: 'Aprovado', color: '#00a500'},
    {codigo: EStatusComercial.Reprovado, descricao: 'Reprovado', color: '#FF8072'}
];

export const statusComercialAlcada: Status[] = [
    {codigo: ESituacaoAprovacaoComercialAlcadaUsuario.AguardandoAprovacao, descricao: 'Aguardando aprovação seu nível de hierarquia', color: '#ffc800'},
    {codigo: ESituacaoAprovacaoComercialAlcadaUsuario.AguardandoAprovacaoOutroNivel, descricao: 'Aguardando aprovação de outro nível de hierarquia', color: '#ff00ff'},
]

export const statusObraTaxa: Status[] = [
    {codigo: EStatusObraTaxa.Nova, descricao: 'Nova Taxa', color: '#33B1F6'}
];

export const statusGeracaoContrato: Status[] = [
    {codigo: 0, descricao: 'Não gerado', color: '#CCCCCC'},
    {codigo: 1, descricao: 'Gerado', color: '#CCCCCC'}
];

export const statusContrato: Status[] = [
    {codigo: EStatusContrato.Inexistente, descricao: 'Não gerado', color: '#CCCCCC'},
    {codigo: EStatusContrato.NaoGerado, descricao: 'Não gerado', color: '#CCCCCC'},
    {codigo: EStatusContrato.PreAnalise, descricao: 'Pré análise', color: '#ffc800'},
    {codigo: EStatusContrato.Reprovado, descricao: 'Reprovado', color: '#FF8072'},
    {codigo: EStatusContrato.EmAnalise, descricao: 'Em análise', color: '#ffc800'},
    {codigo: EStatusContrato.Pendente, descricao: 'Pendente', color: '#ffc800'},
    {codigo: EStatusContrato.Aprovado, descricao: 'Aprovado', color: '#00a500'},
    {codigo: EStatusContrato.AgConfPagamento, descricao: 'Aguardando confirmação de pagamento', color: '#ffc800'},
    {codigo: EStatusContrato.Cancelado, descricao: 'Cancelado', color: '#FF8072'},
    {codigo: EStatusContrato.AgDtProgramacao, descricao: 'Aguardando data de programação', color: '#ffc800'},
    {codigo: EStatusContrato.RevalidaCadastro, descricao: 'Revalidação de cadastro', color: '#ffc800'},
    {codigo: EStatusContrato.AgDadosPagamento, descricao: 'Aguardando dados de pagamento', color: '#ffc800'},
    {codigo: EStatusContrato.AgAprovacaoComercial, descricao: 'Aguardando aprovação comercial', color: '#ffc800'},
    {codigo: EStatusContrato.Encerrado, descricao: 'Encerrado', color: '#8e8e8e'}
];

export const modelosDanfeRomaneio: ModeloDocumentoDanfeRomaneio[] = [
    { codigo: 0, descricao: 'Padrão' },
    { codigo: 1, descricao: 'Materiais' },
    { codigo: 2, descricao: 'Traço' }
];

export class Proposta {
    usina: Usina;
    ano: number = 0;
    numero: number = 0;
    statusProposta: EStatusProposta = EStatusProposta.EmNegociacao;
    statusComercial: EStatusComercial = EStatusComercial.NaoNecessita;
    statusContrato: EStatusContrato = EStatusContrato.Inexistente;
    statusGeracaoContrato: number = 0;
    data: Date = new Date();
    vendedor: Vendedor;
    //representante: Vendedor = null;
    vendedorPadrinho: Vendedor;
    interveniente: Interveniente = new Interveniente();
    obra: Obra = new Obra();
    intervenienteCodigo: number = 0;
    intervenienteRazao: string = '';
    tracoPrecoNumeroTabela: number = 0;
    contato: string = '';
    telefoneDdd: number = 0;
    telefoneNumero: number = 0;
    ramal: number = 0;
    celularDdd: number = 0;
    celularNumero: number = 0;
    endereco: Endereco = new Endereco();
    email: string = '';
    emailCobranca: string = '';
    idCadastro: string = '';
    idAtualizacao: string = '';
    observacao: string = '';
    intervenienteTipo: string = intervenienteTipos[0].codigo;
    valorConcreto: number = 0;
    valorBomba: number = 0;
    valorExtras: number = 0;
    valorTotalContrato: number = 0;
    volumeTotal: number = 0;
    idEmissao: string = '';
    nomeMae: string = '';
    nomeConjuge: string = '';
    status: number = 0;
    cpfCnpj: string = '';
    intervenienteNome: string = '';
    inscricaoEstadual: string = 'ISENTO';
    inscricaoMunicipal: string = '';
    rg: string = '';
    orgaoExpedidor: string = '';
    profissao: string = '';
    empresaTrabalho: string = '';
    telefoneComercialDdd: number = 0;
    telefoneComercialNumero: number = 0;
    faturamentoAosCuidados: string = '';
    intervenienteIdExterno: string = '';

    faturamento: PropostaDadosPessoais = null;
    utilizaDadosFaturamento: boolean = false;
    utilizaEnderecoFaturamento: boolean = false;

    cobranca: PropostaDadosPessoais = null;
    utilizaDadosCobranca: boolean = false;
    utilizaEnderecoCobranca: boolean = false;

    responsavelSolidario: PropostaDadosPessoais = null;
    utilizaResponsavelSolidario: boolean = false;
    
    codigoObraPrefeitura: string = '';
    origemUsinaCodigo: number = 0;
    origemObraCodigo: number = 0;
    dataEncerramentoContrato: Date;
    isContratoEncerrado: boolean = false;
    isContratoFechado: boolean = false;
    
    modeloDocumentoRemessaConcreto: number = 0;
    modeloDocumentoRemessaBomba: number = 0;
    modeloItensDanfeERomaneio: number = 0;

    condicoesGerais: string = '';
    validadeProposta: Date;

    segmentacao: number = 0;
    segmento: Segmentacao = null;
    dataUltimaVersaoGerada: Date;

    anoVisita: number = 0;
    numeroVisita: number = 0;
    anoLead: number = 0;
    numeroLead: number = 0;
    anoOportunidade: number = 0;
    numeroOportunidade: number = 0;

    contratoFinalidade: EContratoFinalidade = EContratoFinalidade.PrestacaoServico;
    fimVigenciaContrato: Date;

    origem: string = "";

    aprovacaoMedicao: string = medicaoContratos[0].codigo;
    tempoAprovacaoMedicaoCadastro: CadastroGeral;
    tempoAprovacaoMedicao: number = 0;
  }