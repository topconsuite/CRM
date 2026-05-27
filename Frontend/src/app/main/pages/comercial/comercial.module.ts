import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';
import { 
    MatIconModule, MatCheckboxModule, MatCardModule, MatExpansionModule, MatInputModule, 
    MatTooltipModule, MatRadioModule, MatTableModule, MatButtonModule, MatDividerModule, MatMenuModule
} from '@angular/material';

import { FuseSharedModule } from '@fuse/shared.module';

import { TextMaskModule } from 'angular2-text-mask';

import { ComponentsModule } from './../../components/components.module';

//PAGAMENTOS
import { SolicitacaoPagamentoCartaoPageComponent, SolicitacaoPagamentoCartaoPageComponentNotModal } from './pagamentos/solicitacao-pagamento-cartao-page/solicitacao-pagamento-cartao-page.component';

// CADASTROS
import { DemaisServicosPageComponent } from './cadastros/demais-servicos-page/demais-servicos-page.component';
import { TabelaVendaPageComponent } from './cadastros/tabela-venda-page/tabela-venda-page.component';
import { CustoServicoMarkupPageComponent } from './cadastros/custo-servico-markup/custo-servico-markup-page.component'; 
import { GrupoEconomicoPageComponent } from './cadastros/grupo-economico-page/grupo-economico-page.component';
import { VisitaTipoPageComponent } from './cadastros/visita-tipo-page/visita-tipo-page.component';
import { MotivoPerdaPageComponent } from './cadastros/motivo-perda-page/motivo-perda-page.component';
import { OportunidadeTipoPageComponent } from './cadastros/oportunidade-tipo-page/oportunidade-tipo-page.component';
import { ConcorrentePageComponent } from './cadastros/concorrente-page/concorrente-page.component';

//LEAD
import { LeadPageComponent } from './lead/lead-page/lead-page.component';
import { LeadListaPageComponent } from './lead/lead-lista-page/lead-lista-page.component';

// PROPOSTA
import { PropostaPageComponent } from './proposta/proposta-page/proposta-page.component';
import { PropostaListaPageComponent } from './proposta/proposta-lista-page/proposta-lista-page.component';

//CONTRATO
import { ContratoPageComponent } from './contrato/contrato-page/contrato-page.component';
import { ContratoListaPageComponent } from './contrato/contrato-lista-page/contrato-lista-page.component';

// PROGRAMAÇÃO
import { ProgramacaoListaPageComponent } from './programacao/programacao-lista-page/programacao-lista-page.component';
import { ProgramacaoPageComponent } from './programacao/programacao-page/programacao-page.component';

// RELATÓRIOS
import { RelatorioProducaoPageComponent } from './relatorios/relatorio-producao-page/relatorio-producao-page.component';
import { PropostaAprovacoesPageComponent } from './proposta/proposta-aprovacoes-page/proposta-aprovacoes-page.component';
import { ComercialDashboardComponent } from './comercial-dashboard/comercial-dashboard.component';
import { CondicaoPagamentoPageComponent } from './cadastros/condicao-pagamento-page/condicao-pagamento-page.component';
import { AssinaturaEletronicaCadastroComponent } from './configuracoes/assinatura-eletronica-page/assinatura-eletronica-cadastro/assinatura-eletronica-cadastro.component';
import { TabelaVendaAprovacaoPageComponent } from './aprovacoes/tabela-venda-aprovacao-page/tabela-venda-aprovacao-page/tabela-venda-aprovacao-page.component';
import { VersionamentoContratoPageComponent } from './configuracoes/versionamento-contrato/versionamento-contrato-page/versionamento-contrato-page.component';
import { AprovacaoComercialConfigPageComponent } from './configuracoes/aprovacao-comercial-config-page/aprovacao-comercial-config-page.component';
import { AcessoAprovacoesConfigPageComponent } from './configuracoes/acesso-aprovacoes-config-page/acesso-aprovacoes-config-page.component';

import { ContratoReajusteAprovacaoPageComponent } from './aprovacoes/contrato-reajuste-aprovacao-page/contrato-reajuste-aprovacao-page.component';
import { VisitaListaPageComponent } from './visita/visita-lista-page/visita-lista-page.component';
import { VisitaPageComponent } from './visita/visita-page/visita-page.component';
import { AgendaPageComponent } from './agenda/agenda-page/agenda-page.component';
import { OportunidadePageComponent } from './oportunidade/oportunidade-page/oportunidade-page.component';
import { OportunidadeListaPageComponent } from './oportunidade/oportunidade-lista-page/oportunidade-lista-page.component';
import { CarteiraPageComponent } from './contrato/carteira-page/carteira-page.component';



const routes = [
    {
        path     : 'pages/comercial',
        component: ComercialDashboardComponent
    },
    {
        path     : 'pages/comercial/cadastros/demais-servicos',
        component: DemaisServicosPageComponent
    },
    {
        path     : 'pages/comercial/lead/novo',
        component: LeadPageComponent
    },
    {
        path     : 'pages/comercial/visita/usina/:idUsina/ano/:ano/numero/:numero/gerar-lead',
        component: LeadPageComponent
    },
    {
        path     : 'pages/comercial/lead/usina/:idUsina/ano/:ano/numero/:numero',
        component: LeadPageComponent
    },
    {
        path     : 'pages/comercial/lead/lista',
        component: LeadListaPageComponent
    },
    {
        path     : 'pages/comercial/proposta/nova',
        component: PropostaPageComponent
    },
    {
        path     : 'pages/comercial/oportunidade/usina/:idUsina/ano/:ano/numero/:numero/gerar-proposta',
        component: PropostaPageComponent
    },
    {
        path     : 'pages/comercial/proposta/usina/:idUsina/ano/:ano/numero/:numero',
        component: PropostaPageComponent
    },
    {
        path     : 'pages/comercial/proposta/lista',
        component: PropostaListaPageComponent
    },
    {
        path     : 'pages/comercial/proposta/aprovacoes',
        component: PropostaAprovacoesPageComponent
    },
    {
        path     : 'pages/comercial/programacao/lista',
        component: ProgramacaoListaPageComponent
    },
    {
        path     : 'pages/comercial/programacao/usina/:idUsina/proposta-ano/:propostaAno/proposta-numero/:propostaNumero/sequencia/:sequencia',
        component: ProgramacaoPageComponent
    },
    {
        path     : 'pages/comercial/programacao/usina/:idUsina/proposta-ano/:propostaAno/proposta-numero/:propostaNumero/nova',
        component: ProgramacaoPageComponent
    },
    {
        path     : 'pages/comercial/programacao/usina/:idUsina/proposta-ano/:propostaAno/proposta-numero/:propostaNumero/sequencia/:sequencia/clonar',
        component: ProgramacaoPageComponent
    },
    {
        path     : 'pages/comercial/relatorios/producao',
        component: RelatorioProducaoPageComponent
    },
    {
        path     : 'pages/comercial/pagamentos/solicitacao',
        component: SolicitacaoPagamentoCartaoPageComponentNotModal
    },
    {
        path     : 'pages/comercial/cadastros/condicao-pagamento',
        component: CondicaoPagamentoPageComponent
    },
    {
        path     : 'pages/comercial/cadastros/grupos-economicos',
        component: GrupoEconomicoPageComponent
    },
    {
        path     : 'pages/comercial/cadastros/tabela-venda',
        component: TabelaVendaPageComponent
    },
    {
        path     : 'pages/comercial/aprovacoes/tabela-venda',
        component: TabelaVendaAprovacaoPageComponent
    },
    {
        path     : 'pages/comercial/aprovacoes/contrato-reajuste',
        component: ContratoReajusteAprovacaoPageComponent
    },
    {
        path     : 'pages/comercial/configuracoes/assinatura-eletronica',
        component: AssinaturaEletronicaCadastroComponent
    },
    {
        path     : 'pages/comercial/cadastros/custo-servico-markup',
        component: CustoServicoMarkupPageComponent
    },
    {
        path     : 'pages/comercial/configuracoes/versionamento-contrato',
        component: VersionamentoContratoPageComponent
    },
    {
        path     : 'pages/comercial/configuracoes/aprovacao-comercial',
        component: AprovacaoComercialConfigPageComponent
    },
    {
        path     : 'pages/comercial/configuracoes/acesso-aprovacoes',
        component: AcessoAprovacoesConfigPageComponent
    },
    {
        path     : 'pages/comercial/cadastros/tipos-visita',
        component: VisitaTipoPageComponent
    },
    {
        path     : 'pages/comercial/cadastros/motivos-perda',
        component: MotivoPerdaPageComponent
    },
    {
        path     : 'pages/comercial/cadastros/tipos-oportunidade',
        component: OportunidadeTipoPageComponent
    },
    {
        path     : 'pages/comercial/cadastros/concorrentes',
        component: ConcorrentePageComponent
    },
    {
        path     : 'pages/comercial/visita/nova',
        component: VisitaPageComponent
    },
    {
        path     : 'pages/comercial/visita/usina/:idUsina/ano/:ano/numero/:numero',
        component: VisitaPageComponent
    },
    {
        path     : 'pages/comercial/visita/lista',
        component: VisitaListaPageComponent
    },
    {
        path     : 'pages/comercial/agenda',
        component: AgendaPageComponent
    },
    {
        path     : 'pages/comercial/contrato/lista',
        component: ContratoListaPageComponent
    },
    {
        path     : 'pages/comercial/contrato/usina/:idUsina/ano/:ano/numero/:numero/anoProposta/:anoProposta/numeroProposta/:numeroProposta',
        component: ContratoPageComponent
    },
    {
        path     : 'pages/comercial/oportunidade/nova',
        component: OportunidadePageComponent
    },
    {
        path     : 'pages/comercial/lead/usina/:idUsina/ano/:ano/numero/:numero/gerar-oportunidade',
        component: OportunidadePageComponent
    },
    {
        path     : 'pages/comercial/oportunidade/usina/:idUsina/ano/:ano/numero/:numero',
        component: OportunidadePageComponent
    },
    {
        path     : 'pages/comercial/oportunidade/lista',
        component: OportunidadeListaPageComponent
    },
    {
        path     : 'pages/comercial/contrato/carteira',
        component: CarteiraPageComponent
    },
];

@NgModule({
    declarations: [
        LeadPageComponent,
        LeadListaPageComponent,
        PropostaPageComponent,
        PropostaListaPageComponent,
        ProgramacaoListaPageComponent,
        ProgramacaoPageComponent,
        RelatorioProducaoPageComponent,
        DemaisServicosPageComponent,
        SolicitacaoPagamentoCartaoPageComponent,
        SolicitacaoPagamentoCartaoPageComponentNotModal,
        PropostaAprovacoesPageComponent,
        ComercialDashboardComponent,
        CondicaoPagamentoPageComponent,
        AssinaturaEletronicaCadastroComponent,
		TabelaVendaPageComponent,
        CustoServicoMarkupPageComponent,
        TabelaVendaAprovacaoPageComponent,
        ContratoReajusteAprovacaoPageComponent,
        VersionamentoContratoPageComponent,
        AprovacaoComercialConfigPageComponent,
        GrupoEconomicoPageComponent,
        AcessoAprovacoesConfigPageComponent,
        VisitaTipoPageComponent,
        MotivoPerdaPageComponent,
        OportunidadeTipoPageComponent,
        ConcorrentePageComponent,
        VisitaTipoPageComponent,
        VisitaListaPageComponent,
        VisitaPageComponent,
        AgendaPageComponent,
        ConcorrentePageComponent,
        ContratoListaPageComponent,
        ContratoPageComponent,
        OportunidadePageComponent,
        OportunidadeListaPageComponent,
        CarteiraPageComponent
    ],
    imports     : [
        FuseSharedModule,
        TextMaskModule,
        BrowserModule,
        FormsModule,
        HttpModule,
        RouterModule.forChild(routes),
        ComponentsModule,
        MatIconModule, MatCheckboxModule, MatCardModule, MatExpansionModule, MatInputModule,
        MatTooltipModule, MatRadioModule, MatTableModule, MatButtonModule, MatDividerModule, MatMenuModule
    ],
    exports: [
        LeadPageComponent,
        LeadListaPageComponent,
        PropostaPageComponent,
        PropostaListaPageComponent,
        ProgramacaoListaPageComponent,
        ProgramacaoPageComponent,
        RelatorioProducaoPageComponent,
        DemaisServicosPageComponent,
        SolicitacaoPagamentoCartaoPageComponent,
        SolicitacaoPagamentoCartaoPageComponentNotModal,
        PropostaAprovacoesPageComponent,
        ComercialDashboardComponent,
        CondicaoPagamentoPageComponent,
        AssinaturaEletronicaCadastroComponent,
        CustoServicoMarkupPageComponent,
        VersionamentoContratoPageComponent,
        AprovacaoComercialConfigPageComponent,
        AcessoAprovacoesConfigPageComponent,
        VisitaTipoPageComponent,
        MotivoPerdaPageComponent,
        OportunidadeTipoPageComponent,
        AgendaPageComponent,
        ConcorrentePageComponent,
        ContratoListaPageComponent,
        ContratoPageComponent,
        OportunidadePageComponent,
        OportunidadeListaPageComponent
    ],
    entryComponents: [
        SolicitacaoPagamentoCartaoPageComponent
    ]
})

export class ComercialModule
{

}
