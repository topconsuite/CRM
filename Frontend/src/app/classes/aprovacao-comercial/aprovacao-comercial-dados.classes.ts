export enum EAprovacaoComercialPendenteStatus {
    AguardandoAprovacao = 0,
    Aprovado = 1,
    Reprovado = 2,
    Descartado = 3,
    AguardandoAprovacaoNivelAnterior = 4,
}

export class AprovacaoComercialDados {

    tracos: AprovacaoComercialDadosItem[] = [];
    bombas: AprovacaoComercialDadosItem[] = [];
    volumes: AprovacaoComercialDadosItem[] = [];
    condicoesPagamento: AprovacaoComercialDadosItem[] = [];


}

export class AprovacaoComercialDadosItem {

    nivelAutoridade: number = 0;
    nivelDescricao: string = '';

    sequencia: number = 0;

    quantidadeNotificacoes: number = 0;
    quantidadeNotificacoesAprovadas: number = 0;
    quantidadeNotificacoesReprovadas: number = 0;
    status: EAprovacaoComercialPendenteStatus;
    aprovadores: string = '';
    reprovadores: string = '';

}
