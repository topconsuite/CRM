import { Endereco } from './../endereco/endereco';
import { Vendedor } from '../vendedor/vendedor';
import { CadastroGeral } from '../bomba/bomba.classes';
import { GrupoEconomico } from '../grupo-economico/grupo-economico';

export class IntervenienteTipo { codigo: string; descricao: string; }
export const intervenienteTipos: IntervenienteTipo[] = [
    {codigo: 'F', descricao: 'Fisica'},
    {codigo: 'J', descricao: 'Juridica'},
    {codigo: 'C', descricao: 'Construtora'},
    {codigo: 'P', descricao: 'Publica'}
];

export class Interveniente {
    codigo: number = 0;
    nome: string = '';
    razao: string = '';
    intervenienteTipo: string = intervenienteTipos[0].codigo;
    cpfCnpj: string = '';
    rg: string = '';
    orgaoExpedidor: string = '';
    inscricaoEstadual: string = '';
    inscricaoMunicipal: string = '';
    endereco: Endereco = new Endereco();
    profissao: string = '';
    empresaTrabalho: string = '';
    telefoneComercialDdd: number = 0;
    telefoneComercialNumero: number = 0;
    nomeMae: string = '';
    nomeConjuge: string = '';
    contato: string = '';
    telefoneDdd: number = 0;
    telefoneNumero: number = 0;
    ramal: number = 0;
    celularDdd: number = 0;
    celularNumero: number = 0;
    email: string = '';
    emailCobranca: string = '';
    observacao: string = '';
    limiteValor: number = 0.00;
    vendedor: Vendedor;
    bloqueioMotivo: CadastroGeral;
    bloqueioObservacao: string = '';
    retemIss: string = '';
    idAprovacaoRetencaoIss: string = '';
    limiteData: Date;
    grupoEconomicoCodigo: number = 0;
    grupoEconomico: GrupoEconomico;
    idExterno: string = '';
}