import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule, Routes } from '@angular/router';
import { HttpModule } from '@angular/http';
import { MatMomentDateModule } from '@angular/material-moment-adapter';
import { MatButtonModule, MatIconModule, MatProgressSpinnerModule, MatDialogModule, MatAutocompleteModule, MatInputModule, MatFormFieldModule } from '@angular/material';
import { TranslateModule } from '@ngx-translate/core';
import 'hammerjs';

import { FuseModule } from '@fuse/fuse.module';
import { FuseSharedModule } from '@fuse/shared.module';
import { FuseProgressBarModule, FuseSidebarModule, FuseThemeOptionsModule } from '@fuse/components';

import { fuseConfig } from 'app/fuse-config';

import { AppComponent } from 'app/app.component';
import { LayoutModule } from 'app/layout/layout.module';
import { ComponentsModule } from 'app/main/components/components.module';
import { FuseHomeModule } from 'app/main/home/home.module';
import { PagesModule } from 'app/main/pages/pages.module';

import { AuthService } from './services/auth/auth.service';
import { BaseService } from './services/base.service';
import { UserService } from './services/user.service';
import { ApiBaseUrlService } from './services/api-base-url.service';
import { AuthGuardService } from './services/auth/auth-guard.service';
import { VersionService } from './services/version.service';
import { CadastroGeralService } from './services/cadastro-geral.service';
import { CadastroDiversoService } from './services/cadastro-diverso.service';
import { ContratoService } from './services/contrato.service';
import { UsinaService } from './services/usina.service';
import { VendedorService } from './services/vendedor.service';
import { IntervenienteService } from './services/interveniente.service';
import { EnderecoService } from './services/endereco.service';
import { TracoPrecoService } from './services/traco-preco.service';
import { SlumpService } from './services/slump.service';
import { PedraService } from './services/pedra.service';
import { PecaAConcretarService } from './services/peca-a-concretar.service';
import { BombaPrecoService } from './services/bomba-preco.service';
import { PagamentoService } from './services/pagamento.service';
import { PropostaService } from './services/proposta.service';
import { ProgramacaoService } from './services/programacao.service';
import { ParametroService } from './services/parametro.service';
import { ObraService } from './services/obra.service';
import { ObraTaxaService } from './services/obra-taxa.service';
import { RelatorioService } from './services/relatorio.service';
import { TituloContasAReceberService } from './services/titulo-contas-a-receber.service';
import { DemaisServicosService } from './services/demais-servicos.service';
import { MercadoriaService } from './services/mercadoria.service';
import { UnidadeService } from './services/unidade.service';
import { FuncionarioService } from './services/funcionario.service';
import { InterceptedHttpService, LoadingDialog } from './services/auth/intercepted-http.service';
import { CondicaoPagamentoService } from './services/condicao-pagamento.service';
import { CustoServicoService } from './services/custo-servico.service';
import { AssinaturaEletronicaService } from './services/assinatura-eletronica.service';
import { FilialService } from './services/filial.service';
import { PreTracoPrecoService } from './services/pre-traco-preco.service';
import { AprovacaoComercialService } from './services/aprovacao-comercial.service';
import { GrupoEconomicoService } from './services/grupo-economico.service';
import { SegmentacaoService } from './services/segmentacao.service';
import { AcessoAprovacoesConfigService } from './services/acesso-aprovacoes-config.service';

import { ContratoReajusteService } from './services/contrato-reajuste.service';
import { ObraProjecaoService } from './services/obra-projecao.service';
import {TributacaoPisCofinsService} from "./services/tributacao-pis-cofins.service";
import { VisitaTipoService } from './services/visita-tipo.service';

import { LeadFaseService } from './services/lead-fase.service';
import { MotivoPerdaService } from './services/motivo-perda.service';
import { LeadService } from './services/lead.service';
import { OportunidadeTipoService } from './services/oportunidade-tipo.service';
import { ConcorrenteService } from './services/concorrente.service';
import { VisitaService } from './services/visita-service';
import { TarefaService } from './services/tarefa.service';
import { CompromissoService } from './services/compromisso.service';
import { OportunidadeService } from './services/oportunidade.service';
import { SsoService } from './services/sso.service';
import { BoletoExternoService } from './services/boleto-externo.service';
import { TributacaoReformaService } from './services/tributacao-reforma.service';

const appRoutes: Routes = [
    {path: '', redirectTo: 'home', pathMatch: 'full', canActivate: [AuthGuardService]}
];

@NgModule({
    declarations: [
        AppComponent,
        LoadingDialog
    ],
    entryComponents: [
        LoadingDialog
    ],
    imports     : [
        BrowserModule,
        BrowserAnimationsModule,
        HttpClientModule,
        RouterModule.forRoot(appRoutes),

        TranslateModule.forRoot(),

        HttpModule,

        // Material moment date module
        MatMomentDateModule,

        // Material
        MatButtonModule,
        MatIconModule,
        MatProgressSpinnerModule,
        MatDialogModule,
        MatAutocompleteModule,
        MatInputModule,
        MatFormFieldModule,

        // Fuse modules
        FuseModule.forRoot(fuseConfig),
        FuseProgressBarModule,
        FuseSharedModule,
        FuseSidebarModule,
        FuseThemeOptionsModule,

        // App modules
        LayoutModule,
        ComponentsModule,
        FuseHomeModule,
        PagesModule
    ],
    providers: [
        ApiBaseUrlService,
        AuthGuardService,
        AuthService,
        InterceptedHttpService,
        BaseService,
        VersionService,
        CadastroGeralService,
        CadastroDiversoService,
        ContratoService,
        UserService,
        UsinaService,
        VendedorService,
        IntervenienteService,
        EnderecoService,
        TracoPrecoService,
        SlumpService,
        PedraService,
        PecaAConcretarService,
        BombaPrecoService,
        PagamentoService,
        ProgramacaoService,
        PropostaService,
        ParametroService,
        ObraService,
        ObraTaxaService,
        RelatorioService,
        TituloContasAReceberService,
        DemaisServicosService,
        MercadoriaService,
        UnidadeService,
        FuncionarioService,
        CondicaoPagamentoService,
        CustoServicoService,
        AssinaturaEletronicaService,
        FilialService,
        PreTracoPrecoService,
        AprovacaoComercialService,
        GrupoEconomicoService,
        SegmentacaoService,
        ContratoReajusteService,
        ObraProjecaoService,
        TributacaoPisCofinsService,
        TributacaoReformaService,
        AcessoAprovacoesConfigService,
        LeadFaseService,
        LeadService,
        VisitaTipoService,
        MotivoPerdaService,
        OportunidadeTipoService,
        OportunidadeService,
        ConcorrenteService,
        VisitaService,
        TarefaService,
        CompromissoService,
        SsoService,
        BoletoExternoService
    ],
    bootstrap   : [
        AppComponent
    ]
})
export class AppModule
{
}
