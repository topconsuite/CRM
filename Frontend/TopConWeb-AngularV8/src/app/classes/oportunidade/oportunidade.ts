import { CadastroGeral } from "../cadastro-geral/cadastro-geral";
import { Endereco } from "../endereco/endereco";
import { MotivoPerda } from "../motivo-perda/motivo-perda";
import { Proposta } from "../proposta/proposta";
import { Segmentacao } from "../segmentacao/segmentacao";
import { Usina } from "../usina/usina";
import { Vendedor } from "../vendedor/vendedor";
import { Concorrente } from "./concorrente";
import { OportunidadeContato } from "./oportunidade-contato";
import { OportunidadeFase } from "./oportunidade-fase";
import { OportunidadeTipo } from "./oportunidade-tipo";

export enum EFaseOportunidade {
    Qualificacao = 1,
    NecessitaAnalise = 2,
    Apresentacao = 3,
    Proposta = 4,
    Negociacao = 5,
    Formalizacao = 6,
    FechadoGanha = 7,
    FechadoPerdido = 8
}

export enum EClassificacaoTemperatura {
    Quente = 1,
    Morno = 2,
    Frio = 3
}

export const ClassificacaoTemperaturas = [
    { codigo: EClassificacaoTemperatura.Quente, descricao: 'QUENTE', color: '#FF8072' },
    { codigo: EClassificacaoTemperatura.Morno, descricao: 'MORNO', color: '#ffc800' },
    { codigo: EClassificacaoTemperatura.Frio, descricao: 'FRIO', color: '#9a9afa' }
];


export enum EObraFase
{
    Planejamento = 1,
    Execucao = 2,
    Conclusao = 3
}

export const FasesObra = [
    { codigo: EObraFase.Planejamento, descricao: 'PLANEJAMENTO' },
    { codigo: EObraFase.Execucao, descricao: 'EXECUCAO' },
    { codigo: EObraFase.Conclusao, descricao: 'CONCLUSAO' }
]

export class Oportunidade {
    usina: Usina;
    usinaCodigo: number = 0;

    numero: number = 0;
    ano: number = 0;

    numeroLead: number = 0;
    anoLead: number = 0;

    numeroVisita: number = 0;
    anoVisita: number = 0;

    data: Date = new Date();
    oportunidadeNome: string = '';
    intervenienteCodigo: number = 0;
    cliente: string = '';
    dddTelefone: number = 0;
    telefone: number = 0;
    dddCelular: number = 0;
    celular: number = 0;
    email: string = '';
    
    vendedor: Vendedor;
    vendedorCodigo: number = 0;

    segmentacao: Segmentacao;
    segmentacaoCodigo: number = 0;

    oportunidadeTipo: OportunidadeTipo;
    oportunidadeTipoCodigo: number = 0;

    viaCaptacao: CadastroGeral;
    viaCaptacaoCodigo: number = 0;

    fase: OportunidadeFase;
    faseCodigo: number = 0;

    classificacao: EClassificacaoTemperatura;

    proximaEtapa: string = '';
    previsaoFechamento: Date;

    motivoPerda: MotivoPerda;
    motivoPerdaCodigo: number = 0;

    concorrente: Concorrente;
    concorrenteCodigo: number = 0;

    observacaoInterna: string = '';
    obraNome: string = '';

    porteObra: CadastroGeral;
    porteObraCodigo: number = 0;

    obraFase: EObraFase;
    volumeEstimadoObra: number = 0.0;
    valorEstimadoObra: number = 0.0;
    previsaoInicio: Date;
    previsaoTermino: Date;
    endereco: Endereco = new Endereco();
    
    usinaEntrega: Usina;
    usinaEntregaCodigo: number = 0;

    propostas: Proposta[] = [];

    distanciaUsina: number = 0.0;
    referenciaAcesso: string = ''
    contatoPrincipal: OportunidadeContato = new OportunidadeContato();
    contatoSecundario: OportunidadeContato = new OportunidadeContato();
}