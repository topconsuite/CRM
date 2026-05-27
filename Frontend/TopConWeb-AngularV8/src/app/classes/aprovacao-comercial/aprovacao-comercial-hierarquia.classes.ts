import { AprovacaoComercialTipoPessoa } from "./aprovacao-comercial-tipo-pessoa.classes";
import { AprovacaoComercialUsina } from "./aprovacao-comercial-usina.classes";

export enum EAprovacaoComercialHierarquiaValor {
    ValorVendaTracos = 0,
    ValorVendaBomba = 1,
    MargemMCC = 2,
    MargemTransporte = 3,
    Ebtida = 4,
    Volume = 5
}

export const aprovacaoComercialHierarquiaValores: any[] = [
        { codigo: 0, desc: 'Valor de Venda Traços', valor: "VendaTracos" },
        { codigo: 1, desc: 'Valor de Venda Bomba', valor: "VendaBomba" },
        { codigo: 2, desc: 'Margem de MCC', valor: "MargemMCC" },
        { codigo: 3, desc: 'Margem de Transporte', valor: "MargemTransporte" },
        { codigo: 4, desc: 'Valor de Ebtida', valor: "Ebtida" },
        { codigo: 5, desc: 'Volume M3', valor:'Volume' }
    ];


export class AprovacaoComercialHierarquia {
    id: string = '';
    aprovacaoComercialUsinaId: string = '';
    aprovacaoComercialUsina: AprovacaoComercialUsina = new AprovacaoComercialUsina();
    titulo: string = '';
    nivelAutoridade: number = 0;
    quantidadeAprovacoesNecessarias: number = 0;
    aprovacaoObrigatoria: boolean = false;
    createdAt: Date = new Date();
    updatedAt: Date | null = null;
    condicoes: AprovacaoComercialHierarquiaCondicao[] = [];
}

export class Usuario { 
    id: string = '';
    nome: string = '';
}
  
export class AprovacaoComercialHierarquiaCondicao {
    id: string = '00000000-0000-0000-0000-000000000000';
    valorDe: number = 0;
    valorAte: number = 0;
    percentualDe: number = 0;
    percentualAte: number = 0;
    tipoPessoaId: string = '';
    tipoPessoa: AprovacaoComercialTipoPessoa = new AprovacaoComercialTipoPessoa();
    aprovacaoComercialHierarquiaId: string = '';
    valor: string = '';
    tipoValor: EAprovacaoComercialHierarquiaValor = EAprovacaoComercialHierarquiaValor.ValorVendaTracos;
}

export class AprovacaoComercialHierarquiaCondicaoPagamentoItem {
    tipoPessoaId: string = '';
    aprovacaoComercialHierarquiaId: string = '';
    segmentacaoId: number = 0;

    mediaDiasDe: number = 0;
    mediaDiasAte: number = 0;
}

  
export class AprovacaoComercialHierarquiaUsuario {
    id: string = '';
    aprovacaoComercialHierarquiaId: string = '';
    usuarioId: string = '';
}
  