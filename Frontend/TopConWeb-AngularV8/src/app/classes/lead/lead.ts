import { CadastroGeral } from "../cadastro-geral/cadastro-geral";
import { Endereco } from "../endereco/endereco";
import { MotivoPerda } from "../motivo-perda/motivo-perda";
import { Usina } from "../usina/usina";
import { Vendedor } from "../vendedor/vendedor";
import { LeadContato } from "./lead-contato";
import { LeadFase } from "./lead-fase";

export class Fase { codigo: number; color: string; }
export class Classificacao { codigo: number; descricao: string; color: string }

export enum EFaseLead {
    Identificacao = 1,
    Qualificacao = 2,
    Trabalhando = 3,
    Nutrindo = 4,
    Apresentacao = 5,
    Oportunidade = 6,
    FechadoPerdido = 7
}

export enum EClassificacaoLead {
    Quente = 1,
    Morno = 2,
    Frio = 3
}

export const faseLead: Fase[] = [
    { codigo: EFaseLead.Identificacao, color: '#9a9afa' },
    { codigo: EFaseLead.Qualificacao, color: '#9a9afa' },
    { codigo: EFaseLead.Trabalhando, color: '#ffc800' },
    { codigo: EFaseLead.Nutrindo, color: '#ffc800' },
    { codigo: EFaseLead.Apresentacao, color: '#ffc800' },
    { codigo: EFaseLead.Oportunidade, color: '#00a500' },
    { codigo: EFaseLead.FechadoPerdido, color: '#FF8072' }
]

export const classificacaoLead: Classificacao[] = [
    { codigo: EClassificacaoLead.Quente, descricao: 'Quente', color: '#FF8072' },
    { codigo: EClassificacaoLead.Morno, descricao: 'Morno', color: '#ffc800' },
    { codigo: EClassificacaoLead.Frio, descricao: 'Frio', color: '#9a9afa' }
]

export class Lead {
    usina: Usina;
    usinaCodigo: number = 0;
    numero: number = 0;
    ano: number = 0;
    visitaNumero: number = 0;
    visitaAno: number = 0;
    oportunidadeNumero: number = 0;
    oportunidadeAno: number = 0;
    data: Date = new Date();
    cliente: string = '';
    dddTelefone: number = 0;
    telefone: number = 0;
    dddCelular: number = 0;
    celular: number = 0;
    email: string = '';
    vendedor: Vendedor;
    viaCaptacao: CadastroGeral;
    fase: LeadFase = new LeadFase();
    classificacao: number = EClassificacaoLead.Frio;
    proximaEtapa: string;
    obraNome: string;
    endereco: Endereco = new Endereco();
    motivoPerda: MotivoPerda = new MotivoPerda();
    observacaoInterna: string;

    contatoPrincipal: LeadContato = new LeadContato();
    contatoSecundario: LeadContato = new LeadContato();
}