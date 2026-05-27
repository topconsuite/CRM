import { Usina } from './../usina/usina';
import { Endereco } from './../endereco/endereco';
import { CadastroGeral } from './../cadastro-geral/cadastro-geral';
import { CondicaoPagamento } from './../pagamento/condicao-pagamento';
import { TipoCobranca } from './../pagamento/tipo-cobranca';
import { Pagamento } from './../pagamento/pagamento';
import { ObraTraco } from './obra-traco';
import { ObraBomba } from './obra-bomba';
import { ObraLog } from './obra-log';
import { ObraTaxa } from './obra-taxa';
import { Contrato } from '../contrato/contrato';
import { ObraTributacaoMunicipal } from './obra-tributacao-municipal';
import { ObraDemaisServicos } from './obra-demais-servicos';
import { Status, EStatusComercial } from '../proposta/proposta.classes';
import { ObraReajuste } from './obra-reajuste';
import { ObraFrente } from './obra-frente';
import { ObraIndicador } from './obra-indicador';
import {TributacaoPisCofins} from "../tributacao-pis-cofins/tributacao-pis-cofins";
import { TributacaoReforma } from '../tributacao-reforma/tributacao-reforma';
import { ObraProjecao } from './obra-projecao';

export class ConstrucaoCivilTipoAlvara { codigo: number; descricao: string; }

export enum EStatusCadastro
{
    PreCadastro = 0,
    EmAnalise = 1,
    Aprovado = 2,
    Reprovado = 3,
    Revalidacao = 4,
    Pendente = 5,
    AguardandoProgramacao = 6,
    Cancelado = 7,
    Encerrado = 8
}

export enum EStatusEngenharia
{
    NaoNecessita = 0,
    Aguardando = 1,
    Aprovado = 2,
    //Reprovado = 3
}

export enum EStatusFinanceiro
{
    NaoNecessita = 0,
    AguardandoConfirmacao = 1,
    Aprovado = 2,
    //Reprovado = 3,
    AguardandoDadosPagamento = 4
}

export enum EObraDemaisStatusComercial {
    NaoNecessita = 0,
    AguardandoAprovacao = 1,
    Aprovado = 2,
    Reprovado = 3
}

export enum EStatusProjecao
{
    Igual = 0,
    Divergente = 1,
    NaoPossui = 2
}

export enum EConstrucaoCivilTipoAlvara
{
    SemDefinicao = 0,
    Ampliacao = 1,
    ConstrucaoNova = 2,
    Demolicao = 3,
    Reforma = 4
}

export const statusCadastro: Status[] = [
    {codigo: EStatusCadastro.PreCadastro, descricao: 'Pré-cadastro', color: '#CCCCCC'},
    {codigo: EStatusCadastro.EmAnalise, descricao: 'Em análise', color: '#ffc800'},
    {codigo: EStatusCadastro.Aprovado, descricao: 'Aprovado', color: '#00a500'},
    {codigo: EStatusCadastro.Reprovado, descricao: 'Reprovado', color: '#FF8072'},
    {codigo: EStatusCadastro.Revalidacao, descricao: 'Re-validação de cadastro', color: '#ffc800'},
    {codigo: EStatusCadastro.Pendente, descricao: 'Pendente', color: '#ff00ff'},
    {codigo: EStatusCadastro.AguardandoProgramacao, descricao: 'Aguardando programação', color: '#ff00ff'},
    {codigo: EStatusCadastro.Cancelado, descricao: 'Cancelado', color: '#FF8072'},
    {codigo: EStatusCadastro.Encerrado, descricao: 'Encerrado', color: '#ff00ff'}
];

export const statuProjecao: Status[] = [
    {codigo: EStatusProjecao.Igual, descricao: 'Igual', color: '#00a500'},
    {codigo: EStatusProjecao.Divergente, descricao: 'Divergente', color: '#FF8072'},
    {codigo: EStatusProjecao.NaoPossui, descricao: 'Não Possui', color: '#CCCCCC'},
];

export const statusEngenharia: Status[] = [
    {codigo: EStatusEngenharia.NaoNecessita, descricao: 'Não necessita', color: '#CCCCCC'},
    {codigo: EStatusEngenharia.Aguardando, descricao: 'Aguardando aprovação', color: '#ffc800'},
    {codigo: EStatusEngenharia.Aprovado, descricao: 'Aprovado', color: '#00a500'},
    //{codigo: EStatusEngenharia.Reprovado, descricao: 'Reprovado', color: '#FF8072'}
];

export const statusFinanceiro: Status[] = [
    {codigo: EStatusFinanceiro.NaoNecessita, descricao: 'Não necessita', color: '#CCCCCC'},
    {codigo: EStatusFinanceiro.AguardandoConfirmacao, descricao: 'Aguardando confirmação', color: '#ffc800'},
    {codigo: EStatusFinanceiro.Aprovado, descricao: 'Aprovado', color: '#00a500'},
    //{codigo: EStatusFinanceiro.Reprovado, descricao: 'Reprovado', color: '#FF8072'},
    {codigo: EStatusFinanceiro.AguardandoDadosPagamento, descricao: 'Aguardando dados de pagamento', color: '#ff00ff'}
];

export const construcaoCivilTipoAlvara: ConstrucaoCivilTipoAlvara[] = [
    {codigo: EConstrucaoCivilTipoAlvara.SemDefinicao, descricao: 'Sem Definição'},
    {codigo: EConstrucaoCivilTipoAlvara.Ampliacao, descricao: 'Ampliação'},
    {codigo: EConstrucaoCivilTipoAlvara.ConstrucaoNova, descricao: 'Construção Nova'},
    {codigo: EConstrucaoCivilTipoAlvara.Demolicao, descricao: 'Demolição'},
    {codigo: EConstrucaoCivilTipoAlvara.Reforma, descricao: 'Reforma'}
];

export class Obra {
    usinaCodigo: number = 0;
    numero: number = 0;
    anoChamada: number = 0;
    numChamada: number = 0;
    anoContrato: number = 0;
    numContrato: number = 0;
    usinaEntrega: Usina;
    distanciaUsina: number = 0;
    distanciaUsinaGoogleApi: number = 0;
    nome: string = '';
    endereco: Endereco = new Endereco();
    viaCaptacao: CadastroGeral;
    tipoObra: CadastroGeral;
    porteObra: CadastroGeral;
    email: string = '';
    referenciaAcesso: string = '';
    previsaoInicio: Date;
    previsaoTermino: Date;
    radioNextel: string = '';

    enderecoMunicipioCodigo: number = 0;

    contatoPrincipalNome: string = '';
    contatoPrincipalFuncao: CadastroGeral;
    contatoPrincipalTelefoneDdd: number = 0;
    contatoPrincipalTelefoneNumero: number = 0;
    contatoPrincipalCelularDdd: number = 0;
    contatoPrincipalCelularNumero: number = 0;
    
    contatoSecundarioNome: string = '';
    contatoSecundarioFuncao: CadastroGeral;
    contatoSecundarioTelefoneDdd: number = 0;
    contatoSecundarioTelefoneNumero: number = 0;
    contatoSecundarioCelularDdd: number = 0;
    contatoSecundarioCelularNumero: number = 0;

    volumeEstimado: number = 0;
    volumePorCarga: number = 0;
    condicaoPagamento: CondicaoPagamento = null;
    tipoCobranca: TipoCobranca = null;
    observacaoNf: string = '';
    obraTracos: ObraTraco[] = [];
    obraFrentes: ObraFrente[] = [];
    obraBombas: ObraBomba[] = [];
    obraDemaisServicos: ObraDemaisServicos[] = [];
    obraTaxas: ObraTaxa[] = [];
    obraPagamentos: Pagamento[] = [];
    obraLogs: ObraLog[] = [];
    obraTributacoesMunicipais: ObraTributacaoMunicipal[] = [];

    vibradorQuantidade: number = 0;
    vibradorValorUnitario: number = 0;

    contrato: Contrato;

    cei: string = '';

    codigoCib: string = '';
    construcaoCivilTipoAlvara: EConstrucaoCivilTipoAlvara = EConstrucaoCivilTipoAlvara.SemDefinicao;

    tributacaoPisCofins: TributacaoPisCofins;

    tributacaoIS: TributacaoReforma;
    tributacaoIBS: TributacaoReforma;
    tributacaoCBS: TributacaoReforma;

    statusComercial: EStatusComercial = EStatusComercial.NaoNecessita;
    statusCadastro: EStatusCadastro = EStatusCadastro.PreCadastro;
    statusEngenharia: EStatusEngenharia = EStatusEngenharia.NaoNecessita;
    statusFinanceiro: EStatusFinanceiro = EStatusFinanceiro.NaoNecessita;

    volumeStatusComercial: EObraDemaisStatusComercial = EObraDemaisStatusComercial.NaoNecessita;
    condicaoPagamentoStatusComercial: EObraDemaisStatusComercial = EObraDemaisStatusComercial.NaoNecessita;

    pendenteAprovacaoDistanciaUsinaCEP: boolean;

    observacaoInterna: string = '';

    tempoBtNaObra: number = 0;
    tempoAteAObra: number = 0;
    tempoDescarga: number = 0;

    tempoCicloPrevisto: number = 0;
    custoProjetadoTransporte: number = 0;
    codigoBeneficioFiscal: string = '';

    obraReajuste: ObraReajuste = new ObraReajuste();

    emailResponsavelTecnico: string = '';

    usaAdicionalZMRC: string = 'N';

    necessitaAprovacaoZMRC: string = 'N';
    indicador: ObraIndicador = new ObraIndicador();

    statusProjecao: EStatusProjecao = EStatusProjecao.NaoPossui;
    obraProjecao: ObraProjecao[] = [];
}

export class ObraSimplesDTO
{
    intervenienteNome: string = '';
    usinaEntregaNome: string = '';
    vendedorNome: string = '';
    origem: string = '';
    usinaCodigo: number = 0;
    numero: number = 0;
    anoChamada: number = 0;
    numChamada: number = 0;
    anoContrato: number = 0;
    numContrato: number = 0;
    statusProjecao: EStatusProjecao = EStatusProjecao.NaoPossui;
}