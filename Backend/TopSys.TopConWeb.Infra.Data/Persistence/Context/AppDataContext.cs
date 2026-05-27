using Dapper;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Entities.AprovacaoComercialAlcada;
using TopSys.TopConWeb.Domain.Entities.Lead;
using TopSys.TopConWeb.Domain.Entities.LiberacaoAcesso;
using TopSys.TopConWeb.Domain.Entities.MotivoPerdas;
using TopSys.TopConWeb.Domain.Entities.ObraFrentes;
using TopSys.TopConWeb.Domain.Entities.Oportunidades;
using TopSys.TopConWeb.Domain.Entities.Visitas;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Infra.Data.Persistence.Map;
using TopSys.TopConWeb.Infra.Data.Persistence.Map.AprovacaoComercial;
using TopSys.TopConWeb.Infra.Data.Persistence.Map.LiberacaoAcesso;
using TopSys.TopConWeb.Infra.Data.Persistence.Map.Oportunidades;
using TopSys.TopConWeb.Infra.Data.Persistence.Map.Opportunities;
using TopSys.TopConWeb.Infra.Data.Persistence.Map.Visitas;

namespace TopSys.TopConWeb.Infra.Data.Persistence.Context
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class AppDataContext : DbContext
    {
        public AppDataContext() : base("AppCnnStr") {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;

            // Adicionando o comando para definir sql_mode, pois do nada o TopconWeb está parando de funcionar devido a problemas de sql_mode.
            // Mesmo o sql_mode configurado no my.ni do banco.
            // Essa solução é pra ser temporararia até encontrar oque está acontecendo realmente.
            Database.ExecuteSqlCommand("SET sql_mode=''");
        }

        static AppDataContext()
        {
            DbConfiguration.SetConfiguration(new MySql.Data.Entity.MySqlEFConfiguration());
            //DbInterception.Add(new CustomEFInterceptor());
        }

        public DbConnection Connection { get { return Database.Connection; } }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Configurations.Add(new AprovacaoScriptMap());
            modelBuilder.Configurations.Add(new DemaisAprovacaoMap());
            modelBuilder.Configurations.Add(new AprovacaoTipoMap());
            modelBuilder.Configurations.Add(new CadastroGeralMap());
            modelBuilder.Configurations.Add(new CadastroGeralViaCaptacaoMap());
            modelBuilder.Configurations.Add(new CadastroDiversoMap());
            modelBuilder.Configurations.Add(new TipoCobrancaMap());
            modelBuilder.Configurations.Add(new CartaoBandeiraMap());
            modelBuilder.Configurations.Add(new EnderecoMap());
            modelBuilder.Configurations.Add(new IntervenienteMap());
            modelBuilder.Configurations.Add(new ContratoMap());
            modelBuilder.Configurations.Add(new ContratoVersaoMap());
            modelBuilder.Configurations.Add(new PropostaMap());
            modelBuilder.Configurations.Add(new PropostaVersaoMap());
            modelBuilder.Configurations.Add(new MunicipioMap());
            modelBuilder.Configurations.Add(new CondicaoPagamentoMap());
            modelBuilder.Configurations.Add(new CondicaoPagamentoParcelaMap());
            modelBuilder.Configurations.Add(new ObraMap());
            modelBuilder.Configurations.Add(new ObraVersaoMap());
            modelBuilder.Configurations.Add(new ObraReajusteMap());
            modelBuilder.Configurations.Add(new ObraReajusteVersaoMap());
            modelBuilder.Configurations.Add(new UsoMap());
            modelBuilder.Configurations.Add(new PedraMap());
            modelBuilder.Configurations.Add(new SlumpMap());
            modelBuilder.Configurations.Add(new SlumpRealMap());
            modelBuilder.Configurations.Add(new ResistenciaTipoMap());
            modelBuilder.Configurations.Add(new MensagemPadraoMap());
            modelBuilder.Configurations.Add(new ObraMensagemPadraoMap());
            modelBuilder.Configurations.Add(new ObraMensagemPadraoVersaoMap()); 
            modelBuilder.Configurations.Add(new ObraTaxaMap());
            modelBuilder.Configurations.Add(new ObraTaxaVersaoMap());
            modelBuilder.Configurations.Add(new ObraTracoMap());
            modelBuilder.Configurations.Add(new ObraTracoVersaoMap());
            modelBuilder.Configurations.Add(new ObraBombaMap());
            modelBuilder.Configurations.Add(new ObraBombaVersaoMap());
            modelBuilder.Configurations.Add(new ObraLogMap());
            modelBuilder.Configurations.Add(new ObraLogVersaoMap());
            modelBuilder.Configurations.Add(new ObraTributacaoMunicipalMap());
            modelBuilder.Configurations.Add(new ObraTributacaoMunicipalVersaoMap());
            modelBuilder.Configurations.Add(new ObraDemaisServicosMap());
            modelBuilder.Configurations.Add(new ObraDemaisServicosVersaoMap());
            modelBuilder.Configurations.Add(new TaxaExtraMap());
            modelBuilder.Configurations.Add(new TaxaExtraVersaoMap());
            modelBuilder.Configurations.Add(new UsinaMap());
            modelBuilder.Configurations.Add(new UsuarioMap());
            modelBuilder.Configurations.Add(new VendedorMap());
            modelBuilder.Configurations.Add(new TracoPrecoMap());
            modelBuilder.Configurations.Add(new TracoCustoMap());
            modelBuilder.Configurations.Add(new PecaAConcretarMap());
            modelBuilder.Configurations.Add(new BombaPrecoMap());
            modelBuilder.Configurations.Add(new BombaPrecoTerceiroMap());
            modelBuilder.Configurations.Add(new ContaMap());
            modelBuilder.Configurations.Add(new PortadorMap());
            modelBuilder.Configurations.Add(new ContratoPagamentoMap());
            modelBuilder.Configurations.Add(new ContratoPagamentoVersaoMap()); 
            modelBuilder.Configurations.Add(new ContratoPagamentoForSavingMap());
            modelBuilder.Configurations.Add(new ContratoPagamentoForSavingVersaoMap());
            modelBuilder.Configurations.Add(new ContratoPagamentoDetalheMap());
            modelBuilder.Configurations.Add(new ContratoPagamentoDetalheVersaoMap());
            modelBuilder.Configurations.Add(new ContratoPagamentoDetalheDepositoMap());
            modelBuilder.Configurations.Add(new ContratoPagamentoDetalheDepositoVersaoMap());
            modelBuilder.Configurations.Add(new ContratoPagamentoDetalheCartaoMap());
            modelBuilder.Configurations.Add(new ContratoPagamentoDetalheCartaoVersaoMap());
            modelBuilder.Configurations.Add(new ContratoPagamentoDetalheBoletoMap());
            modelBuilder.Configurations.Add(new ContratoPagamentoDetalheBoletoVersaoMap());
            modelBuilder.Configurations.Add(new ContratoPagamentoDetalheDinheiroMap());
            modelBuilder.Configurations.Add(new ContratoPagamentoDetalheDinheiroVersaoMap());
            modelBuilder.Configurations.Add(new ContratoPagamentoDetalheChequeMap());
            modelBuilder.Configurations.Add(new ContratoPagamentoDetalheChequeVersaoMap());
            modelBuilder.Configurations.Add(new PropostaPagamentoMap());
            modelBuilder.Configurations.Add(new PropostaPagamentoVersaoMap());
            modelBuilder.Configurations.Add(new PropostaPagamentoDetalheMap());
            modelBuilder.Configurations.Add(new PropostaPagamentoDetalheVersaoMap());
            modelBuilder.Configurations.Add(new PropostaPagamentoDetalheDepositoMap());
            modelBuilder.Configurations.Add(new PropostaPagamentoDetalheDepositoVersaoMap());
            modelBuilder.Configurations.Add(new PropostaPagamentoDetalheCartaoMap());
            modelBuilder.Configurations.Add(new PropostaPagamentoDetalheCartaoVersaoMap());
            modelBuilder.Configurations.Add(new PropostaPagamentoDetalheBoletoMap());
            modelBuilder.Configurations.Add(new PropostaPagamentoDetalheBoletoVersaoMap());
            modelBuilder.Configurations.Add(new PropostaPagamentoDetalheChequeMap());
            modelBuilder.Configurations.Add(new PropostaPagamentoDetalheChequeVersaoMap());
            modelBuilder.Configurations.Add(new PropostaPagamentoDetalheDinheiroMap());
            modelBuilder.Configurations.Add(new PropostaPagamentoDetalheDinheiroVersaoMap());
            modelBuilder.Configurations.Add(new PropostaFaturamentoMap());
            modelBuilder.Configurations.Add(new PropostaFaturamentoVersaoMap());
            modelBuilder.Configurations.Add(new PropostaCobrancaMap());
            modelBuilder.Configurations.Add(new PropostaCobrancaVersaoMap());
            modelBuilder.Configurations.Add(new PropostaResponsavelSolidarioMap());
            modelBuilder.Configurations.Add(new PropostaResponsavelSolidarioVersaoMap());
            modelBuilder.Configurations.Add(new ProgramacaoMap());
            modelBuilder.Configurations.Add(new ProgramacaoHoraMap());
            modelBuilder.Configurations.Add(new ProgramacaoLogMap());
            modelBuilder.Configurations.Add(new ProgramacaoDemaisServicosMap());
            modelBuilder.Configurations.Add(new ProgramacaoDemaisServicosVersaoMap());
            modelBuilder.Configurations.Add(new LogGeralMap());
            modelBuilder.Configurations.Add(new NotaFiscalFisicaMap());
            modelBuilder.Configurations.Add(new NotaFiscalFisicaItemMap());
            modelBuilder.Configurations.Add(new NotaFiscalFisicaComplementoMap());
            modelBuilder.Configurations.Add(new NotaFiscalFisicaDemaisServicosMap());
            modelBuilder.Configurations.Add(new ReaproveitamentoMap());
            modelBuilder.Configurations.Add(new UsinaDistanciaCepMap());
            modelBuilder.Configurations.Add(new EmpresaMap());
            modelBuilder.Configurations.Add(new IntervenienteControleFaixaMap());
            modelBuilder.Configurations.Add(new IntervenienteLocalMap());
            modelBuilder.Configurations.Add(new RepasseReajusteMap());
            modelBuilder.Configurations.Add(new ContratoTracoReajusteMap());
            modelBuilder.Configurations.Add(new ContratoTracoReajusteVersaoMap());
            modelBuilder.Configurations.Add(new TituloContasAReceberMap());
            modelBuilder.Configurations.Add(new TituloContasAPagarMap());
            modelBuilder.Configurations.Add(new DemaisServicosMap());
            modelBuilder.Configurations.Add(new MercadoriaMap());
            modelBuilder.Configurations.Add(new UnidadeMap());
            modelBuilder.Configurations.Add(new ContratoBombaReajusteMap());
            modelBuilder.Configurations.Add(new ContratoBombaReajusteVersaoMap());
            modelBuilder.Configurations.Add(new FuncionarioMap());
            modelBuilder.Configurations.Add(new FuncionarioComplementoMap());
            modelBuilder.Configurations.Add(new IntervenienteHistoricoMap());
            modelBuilder.Configurations.Add(new IntervenienteAnexoMap());
            modelBuilder.Configurations.Add(new CustoServicoMap());
            modelBuilder.Configurations.Add(new FilialMap());
            modelBuilder.Configurations.Add(new PreTracoPrecoMap());
            modelBuilder.Configurations.Add(new OpportunityFailureReasonMap());
            modelBuilder.Configurations.Add(new OpportunityOriginMap());
            modelBuilder.Configurations.Add(new OpportunityTypeMap());
            modelBuilder.Configurations.Add(new OpportunityMap());
            modelBuilder.Configurations.Add(new EquipamentoMap());
            modelBuilder.Configurations.Add(new GrupoEconomicoMap());
            modelBuilder.Configurations.Add(new CentroCustoMap());
            modelBuilder.Configurations.Add(new SituacaoFinanceiraMap());
            modelBuilder.Configurations.Add(new TipoDeCobrancaMap()); 
            modelBuilder.Configurations.Add(new OperacaoFinanceiraMap());
            modelBuilder.Configurations.Add(new FaturaMap());
            modelBuilder.Configurations.Add(new FaturaItemMap());
            modelBuilder.Configurations.Add(new TipoDocumentoMap());
            modelBuilder.Configurations.Add(new NotaFiscalDigitalMap());
            modelBuilder.Configurations.Add(new NotaFiscalDigitalItemMap());
            modelBuilder.Configurations.Add(new NotaFiscalDigitalComplementoMap());
            modelBuilder.Configurations.Add(new NotaFiscalDigitalItemComplementoMap());
            modelBuilder.Configurations.Add(new NotaFiscalDigitalDetalhesFiscaisMap());
            modelBuilder.Configurations.Add(new NotaFiscalDigitalDetalhesDistribuicaoMap());
            modelBuilder.Configurations.Add(new ObraIndicadorMap());
            modelBuilder.Configurations.Add(new ObraIndicadorVersaoMap());
            modelBuilder.Configurations.Add(new ObraProjecaoMap());
            modelBuilder.Configurations.Add(new LeadFaseMap());
            modelBuilder.Configurations.Add(new ContratoReajusteLogMap());
            modelBuilder.Configurations.Add(new ContratoReajusteVersaoMap());

            modelBuilder.Configurations.Add(new PrensaMap());

            modelBuilder.Configurations.Add(new LeadMap());
            modelBuilder.Configurations.Add(new LeadContatoMap());
            modelBuilder.Configurations.Add(new LeadLogMap());
            modelBuilder.Configurations.Add(new LeadAnexoMap());
            modelBuilder.Configurations.Add(new LeadInteracaoMap());

            modelBuilder.Configurations.Add(new MotivoPerdaMap());
            modelBuilder.Configurations.Add(new MotivoPerdaLogMap());

            // Aprovação Comercial Alçada
            modelBuilder.Configurations.Add(new AprovacaoComercialUsinaMap());
            modelBuilder.Configurations.Add(new AprovacaoComercialTipoPessoaMap());
            modelBuilder.Configurations.Add(new AprovacaoComercialHierarquiaMap());
            modelBuilder.Configurations.Add(new AprovacaoComercialHierarquiaCondicaoMap());
            modelBuilder.Configurations.Add(new AprovacaoComercialHierarquiaCondicaoPagamentoMap());
            modelBuilder.Configurations.Add(new AprovacaoComercialHierarquiaUsuariosMap());
            modelBuilder.Configurations.Add(new AprovacaoComercialPendenteMap());
            modelBuilder.Configurations.Add(new AprovacaoComercialPendenteTracoMap());
            modelBuilder.Configurations.Add(new AprovacaoComercialPendenteBombaMap());
            modelBuilder.Configurations.Add(new AprovacaoComercialPendenteVolumeMap());
            modelBuilder.Configurations.Add(new AprovacaoComercialPendenteCondicaoPagamentoMap());

            modelBuilder.Configurations.Add(new ObraFrenteMap());
            modelBuilder.Configurations.Add(new SegmentacaoMap());
            
            modelBuilder.Configurations.Add(new TributacaoPisCofinsMap());
            modelBuilder.Configurations.Add(new TributacaoReformaMap());

            modelBuilder.Configurations.Add(new GrupoAcessoMap());
            modelBuilder.Configurations.Add(new LiberacaoAcessoMap());
            modelBuilder.Configurations.Add(new PeriodoAusenciaUsuarioMap());
            modelBuilder.Configurations.Add(new LiberacaoAcessoLogMap());

            modelBuilder.Configurations.Add(new VisitaTipoMap());

            modelBuilder.Configurations.Add(new VisitaMap());
            modelBuilder.Configurations.Add(new VisitaLogMap());
            modelBuilder.Configurations.Add(new VisitaContatoMap());
            modelBuilder.Configurations.Add(new VisitaAnexoMap());
            modelBuilder.Configurations.Add(new VisitaHistoricoMap());

            modelBuilder.Configurations.Add(new ConcorrenteMap());            
            modelBuilder.Configurations.Add(new OportunidadeTipoMap());
            modelBuilder.Configurations.Add(new OportunidadeLogMap());
            modelBuilder.Configurations.Add(new OportunidadeContatoMap());
            modelBuilder.Configurations.Add(new OportunidadeMap());
            modelBuilder.Configurations.Add(new OportunidadeFaseMap());
            modelBuilder.Configurations.Add(new OportunidadeInteracaoMap());
            modelBuilder.Configurations.Add(new OportunidadeAnexoMap());

            modelBuilder.Configurations.Add(new CompromissoMap());
            modelBuilder.Configurations.Add(new CompromissoLogMap());
            modelBuilder.Configurations.Add(new TarefaMap());
            modelBuilder.Configurations.Add(new TarefaLogMap());
            modelBuilder.Configurations.Add(new ContratoFinalidadeMap());

            modelBuilder.Configurations.Add(new ParametrosSSOMap());
            modelBuilder.Configurations.Add(new ClicksignConfiguracaoMap());
        }

        public DbSet<Concorrente> Concorrentes { get; set; }
        public DbSet<OportunidadeTipo> OportunidadesTipos { get; set; }

        public DbSet<VisitaTipo> TiposVisitas { get; set; }

        public DbSet<GrupoAcesso> GruposAcessos { get; set; }
        public DbSet<LiberacaoAcesso> LiberacoesAcessos { get; set; }
        public DbSet<PeriodoAusenciaUsuario> PeriodosAusenciaUsuarios { get; set; }
        public DbSet<LiberacaoAcessoLog> LiberacoesAcessosLogs { get; set; }

        // Aprovação Comercial Alçada
        public DbSet<AprovacaoComercialTipoPessoa> AprovacaoComercialTipoPessoas { get; set; }

        public DbSet<AprovacaoComercialUsina> AprovacaoComercialUsinas { get; set; }

        public DbSet<AprovacaoComercialHierarquia> AprovacaoComercialHierarquias { get; set; }
        public DbSet<AprovacaoComercialHierarquiaCondicao> AprovacaoComercialHierarquiaCondicaos { get; set; }
        public DbSet<AprovacaoComercialHierarquiaCondicaoPagamento> AprovacaoComercialHierarquiaCondicaoPagamento { get; set; }
        public DbSet<AprovacaoComercialHierarquiaUsuario> AprovacaoComercialHierarquiaUsuarios { get; set; }

        public DbSet<AprovacaoComercialPendente> AprovacaoComercialPendentes { get; set; }
        public DbSet<AprovacaoComercialPendenteTraco> AprovacaoComercialPendenteTracos { get; set; }
        public DbSet<AprovacaoComercialPendenteBomba> AprovacaoComercialPendenteBombas { get; set; }
        public DbSet<AprovacaoComercialPendenteVolume> aprovacaoComercialPendenteVolumes { get; set; }

        public DbSet<AprovacaoScript> AprovacoesScripts { get; set; }
        public DbSet<DemaisAprovacao> DemaisAprovacoes { get; set; }
        public DbSet<Contrato> Contratos { get; set; }
        public DbSet<ContratoVersao> ContratosVersoes { get; set; }
        public DbSet<CondicaoPagamento> CondicoesPagamento { get; set; }
        public DbSet<CondicaoPagamentoParcela> CondicaoPagamentoParcelas { get; set; }
        public DbSet<CadastroGeral> CadastrosGerais { get; set; }
        public DbSet<CadastroDiverso> CadastrosDiversos { get; set; }
        public DbSet<TipoCobranca> TiposCobranca { get; set; }
        public DbSet<CartaoBandeira> CartaoBandeiras { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Interveniente> Intervenientes { get; set; }
        public DbSet<IntervenienteSequence> IntervenienteControleFaixa { get; set; }
        public DbSet<Vendedor> Vendedores { get; set; }
        public DbSet<Municipio> Municipios { get; set; }
        public DbSet<MensagemPadrao> MensagensPadrao { get; set; }
        public DbSet<Obra> Obras { get; set; }
        public DbSet<ObraVersao> ObrasVersoes { get; set; }
        public DbSet<ObraBomba> ObraBombas { get; set; }
        public DbSet<ObraBombaVersao> ObraBombasVersoes { get; set; }
        public DbSet<ObraLog> ObraLogs { get; set; }
        public DbSet<ObraLogVersao> ObraLogsVersoes { get; set; }
        public DbSet<ObraTraco> ObraTracos { get; set; }
        public DbSet<ObraTracoVersao> ObraTracosVersoes { get; set; }
        public DbSet<ObraTaxa> ObraTaxas { get; set; }
        public DbSet<ObraTaxaVersao> ObraTaxasVersoes { get; set; }
        public DbSet<ObraMensagemPadrao> ObraMensagensPadrao { get; set; }
        public DbSet<ObraMensagemPadraoVersao> ObraMensagensPadraoVersoes { get; set; }
        public DbSet<ObraTributacaoMunicipal> ObraTributacoesMunicipais { get; set; }
        public DbSet<ObraTributacaoMunicipalVersao> ObraTributacoesMunicipaisVersoes { get; set; }
        public DbSet<ObraDemaisServicos> ObraDemaisServicos { get; set; }
        public DbSet<ObraDemaisServicosVersao> ObraDemaisServicosVersoes { get; set; }
        public DbSet<Pedra> Pedras { get; set; }
        public DbSet<Proposta> Propostas { get; set; }
        public DbSet<PropostaVersao> PropostasVersoes { get; set; }
        public DbSet<PropostaFaturamento> PropostaFaturamentos { get; set; }
        public DbSet<PropostaFaturamentoVersao> PropostaFaturamentosVersoes { get; set; }
        public DbSet<PropostaCobranca> PropostaCobrancas { get; set; }
        public DbSet<PropostaCobrancaVersao> PropostaCobrancasVersoes { get; set; }
        public DbSet<PropostaResponsavelSolidario> PropostaResponsaveisSolidarios { get; set; }
        public DbSet<PropostaResponsavelSolidarioVersao> PropostaResponsaveisSolidariosVersoes { get; set; }
        public DbSet<ResistenciaTipo> ResitenciasTipos { get; set; }
        public DbSet<Slump> Slumps { get; set; }
        public DbSet<SlumpReal> SlumpsReais { get; set; }
        public DbSet<TaxaExtra> TaxasExtras { get; set; }
        public DbSet<TaxaExtraVersao> TaxasExtrasVersoes { get; set; }
        public DbSet<Usina> Usinas { get; set; }
        public DbSet<ClicksignConfiguracao> ClicksignConfiguracoes { get; set; }
        public DbSet<Uso> Usos { get; set; }
        public DbSet<TracoPreco> TracoPrecos { get; set; }
        public DbSet<TracoCusto> TracoCustos { get; set; }
        public DbSet<PecaAConcretar> PecasAConcretar { get; set; }
        public DbSet<BombaPreco> BombaPrecos { get; set; }
        public DbSet<BombaPrecoTerceiro> BombaPrecosTerceiros { get; set; }
        public DbSet<Conta> Contas { get; set; }
        public DbSet<Portador> Portadores { get; set; }
        public DbSet<ContratoPagamento> ContratoPagamentos { get; set; }
        public DbSet<ContratoPagamentoVersao> ContratoPagamentosVersoes { get; set; }
        public DbSet<ContratoPagamentoForSaving> ContratoPagamentosForSaving { get; set; }
        public DbSet<ContratoPagamentoForSavingVersao> ContratoPagamentosForSavingVersoes { get; set; }
        public DbSet<ContratoPagamentoDetalheCartao> ContratoPagamentoDetalhesCartao { get; set; }
        public DbSet<ContratoPagamentoDetalheCartaoVersao> ContratoPagamentoDetalhesCartaoVersoes { get; set; }
        public DbSet<ContratoPagamentoDetalheDeposito> ContratoPagamentoDetalhesDeposito { get; set; }
        public DbSet<ContratoPagamentoDetalheDepositoVersao> ContratoPagamentoDetalhesDepositoVersoes { get; set; }
        public DbSet<ContratoPagamentoDetalheBoleto> ContratoPagamentoDetalhesBoleto { get; set; }
        public DbSet<ContratoPagamentoDetalheBoletoVersao> ContratoPagamentoDetalhesBoletoVersoes { get; set; }
        public DbSet<ContratoPagamentoDetalheDinheiro> ContratoPagamentoDetalhesDinheiro { get; set; }
        public DbSet<ContratoPagamentoDetalheDinheiroVersao> ContratoPagamentoDetalhesDinheiroVersoes { get; set; }
        public DbSet<ContratoPagamentoDetalheCheque> ContratoPagamentoDetalhesCheque { get; set; }
        public DbSet<ContratoPagamentoDetalheChequeVersao> ContratoPagamentoDetalhesChequeVersoes { get; set; }
        public DbSet<PropostaPagamento> PropostaPagamentos { get; set; }
        public DbSet<PropostaPagamentoVersao> PropostaPagamentosVersoes { get; set; }
        public DbSet<PropostaPagamentoDetalhe> PropostaPagamentoDetalhes { get; set; }
        public DbSet<PropostaPagamentoDetalheVersao> PropostaPagamentoDetalhesVersoes { get; set; }
        public DbSet<PropostaPagamentoDetalheCartao> PropostaPagamentoDetalhesCartao { get; set; }
        public DbSet<PropostaPagamentoDetalheCartaoVersao> PropostaPagamentoDetalhesCartaoVersoes { get; set; }
        public DbSet<PropostaPagamentoDetalheDeposito> PropostaPagamentoDetalhesDeposito { get; set; }
        public DbSet<PropostaPagamentoDetalheDepositoVersao> PropostaPagamentoDetalhesDepositoVersoes { get; set; }
        public DbSet<PropostaPagamentoDetalheBoleto> PropostaPagamentoDetalhesBoleto { get; set; }
        public DbSet<PropostaPagamentoDetalheBoletoVersao> PropostaPagamentoDetalhesBoletoVersoes { get; set; }
        public DbSet<PropostaPagamentoDetalheDinheiro> PropostaPagamentoDetalhesDinheiro { get; set; }
        public DbSet<PropostaPagamentoDetalheDinheiroVersao> PropostaPagamentoDetalhesDinheiroVersoes { get; set; }
        public DbSet<PropostaPagamentoDetalheCheque> PropostaPagamentoDetalhesCheque { get; set; }
        public DbSet<PropostaPagamentoDetalheChequeVersao> PropostaPagamentoDetalhesChequeVersoes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Programacao> Programacoes { get; set; }
        public DbSet<ProgramacaoHora> ProgramacaoHoras { get; set; }
        public DbSet<ProgramacaoLog> ProgramacaoLogs { get; set; }
        public DbSet<ProgramacaoDemaisServicos> ProgramacaoDemaisServicos { get; set; }
        public DbSet<ProgramacaoDemaisServicosVersao> ProgramacaoDemaisServicosVersoes { get; set; }
        public DbSet<LogGeral> LogsGerais { get; set; }
        public DbSet<NotaFiscalFisica> NotasFiscaisFisicas { get; set; }
        public DbSet<NotaFiscalFisicaComplemento> NotasFiscaisFisicasComplemento { get; set; }
        public DbSet<NotaFiscalFisicaDemaisServicos> NotasFiscaisFisicasDemaisServicos { get; set; }
        public DbSet<Reaproveitamento> Reaproveitamentos { get; set; }
        public DbSet<UsinaDistanciaCep> UsinaDistanciasCep { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<IntervenienteLocal> IntervenienteLocais { get; set; }
        public DbSet<RepasseReajuste> RepasseReajustes { get; set; }
        public DbSet<ContratoTracoReajuste> ContratoTracoReajustes { get; set; }
        public DbSet<ContratoTracoReajusteVersao> ContratoTracoReajustesVersoes { get; set; }
        public DbSet<TituloContasAReceber> TitulosContasAReceber { get; set; }
        public DbSet<TituloContasAPagar> TitulosContasAPagar { get; set; }
        public DbSet<DemaisServicos> DemaisServicos { get; set; }
        public DbSet<Mercadoria> Mercadoria { get; set; }
        public DbSet<Unidade> Unidade { get; set; }
        public DbSet<ContratoBombaReajuste> ContratoBombaReajustes { get; set; }
        public DbSet<ContratoBombaReajusteVersao> ContratoBombaReajustesVersoes { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<FuncionarioComplemento> FuncionarioComplementos { get; set; }
        public DbSet<IntervenienteHistorico> IntervenienteHistoricos { get; set; }
        public DbSet<IntervenienteAnexo> IntervenienteAnexos { get; set; }
        public DbSet<CustoServico> CustoServicos { get; set; }
        public DbSet<Filial> Filial { get; set; }
        public DbSet<PreTracoPreco> PreTracoPreco { get; set; }
        public DbSet<OpportunityFailureReason> OpportunityFailureReason { get; set; }
        public DbSet<OpportunityOrigin> OpportunityOrigin { get; set; }
        public DbSet<OpportunityType> OpportunityType { get; set; }
        public DbSet<Opportunity> Opportunity { get; set; }
        public DbSet<Equipamento> Equipamentos { get; set; }
        public DbSet<GrupoEconomico> GruposEconomicos { get; set; }
        public DbSet<CentroCusto> CentroCusto { get; set; }
        public DbSet<SituacaoFinanceira> SituacaoFinanceira { get; set; }
        public DbSet<TipoDeCobranca> TipoDeCobranca { get; set; }
        public DbSet<OperacaoFinanceira> OperacoesFinanceiras { get; set; }
        public DbSet<Fatura> Faturas { get; set; }
        public DbSet<NotaFiscalDigital> NotasFiscaisDigital { get; set; }
        public DbSet<NotaFiscalDigitalComplemento> NotasFiscaisDigitalComplemento { get; set; }
        public DbSet<NotaFiscalDigitalItemComplemento> NotaFiscalDigitalItemComplemento { get; set; }
        public DbSet<NotaFiscalDigitalDetalhesFiscais> NotasFiscaisDigitalDetalhesFiscais { get; set; }
        public DbSet<NotaFiscalDigitalDetalhesDistribuicao> NotasFiscaisDigitalDetalhesDistribuicao { get; set; }
        public DbSet<TipoDocumento> TipoDocumento { get; set; }

        public DbSet<ObraFrente> ObraFrente { get; set; }
        public DbSet<Segmentacao> Segmentacao { get; set; }

        public DbSet<ObraProjecao> ObraProjecao { get; set; }
        
        public DbSet<TributacaoPisCofins> TributacoesPisCofins { get; set; }

        public DbSet<TributacaoReforma> TributacoesReforma { get; set; }
        public DbSet<LeadFase> LeadFases { get; set; }

        public DbSet<Lead> Leads { get; set; }
        public DbSet<LeadAnexo> LeadAnexos { get; set; }
        public DbSet<LeadInteracao> LeadInteracoes { get; set; }

        public DbSet<Visita> Visitas { get; set; }
        public DbSet<VisitaLog> VisitaLogs { get; set; }
        public DbSet<VisitaContato> VisitaContatos { get; set; }
        public DbSet<VisitaAnexo> VisitaAnexos { get; set; }
        public DbSet<VisitaHistorico> VisitaHistoricos { get; set; }

        public DbSet<LeadContato> LeadContatos { get; set; }

        public DbSet<MotivoPerda> MotivosPerda { get; set; }
        public DbSet<Compromisso> Compromisso { get; set; }

        public DbSet<Tarefa> Tarefa { get; set; }

        public DbSet<Oportunidade> Oportunidades { get; set; }
        public DbSet<OportunidadeContato> OportunidadeContatos { get; set; }
        public DbSet<OportunidadeFase> OportunidadeFases { get; set; }
        public DbSet<OportunidadeAnexo> OportunidadeAnexo { get; set; }
        public DbSet<OportunidadeInteracao> OportunidadeInteracoes { get; set; }

        public DbSet<ContratoFinalidade> ContratoFinalidades { get; set; }

        public IEnumerable<TableFieldDescribe> DescribeTable(string tableName)
        {
            return Database.Connection.Query<TableFieldDescribe>($"DESC {tableName}");
        }
        public bool FieldExistsInTable(IEnumerable<TableFieldDescribe> tableDescribe, string fieldName)
        {
            return tableDescribe.Select(t => t.Field).Contains(fieldName);
        }
    }

    public struct TableFieldDescribe
    {
        public string Field { get; set; }
        public string Type { get; set; }
        public string Null { get; set; }
        public string Key { get; set; }
        public string Default { get; set; }
    }
}
