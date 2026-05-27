import { Usina } from "../usina/usina";
import { AprovacaoComercialHierarquia } from "./aprovacao-comercial-hierarquia.classes";

export enum EAprovacaoComercialUsinaFluxoAprovacao {
  TodosNiveis = 0,
  UltimoNivel = 1
}

export class AprovacaoComercialUsina {
    id: string = '';
    usinaId: number = 0;
    usina: Usina = new Usina();
    ativo: boolean = true;
    createdAt: Date = new Date();
    updatedAt: Date | null = null;
    hierarquias: AprovacaoComercialHierarquia[] = [];
    fluxoAprovacao: EAprovacaoComercialUsinaFluxoAprovacao = EAprovacaoComercialUsinaFluxoAprovacao.TodosNiveis;
  }
  