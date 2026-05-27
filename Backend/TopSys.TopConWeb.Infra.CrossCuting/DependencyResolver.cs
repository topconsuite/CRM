using Microsoft.Practices.Unity;
using Topsys.TopConWeb.SharedKernel.Services;
using TopSys.TopConWeb.Application;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Helpers;
using TopSys.TopConWeb.Domain.Interfaces.Integrations;
using TopSys.TopConWeb.Domain.Interfaces.LegacyServices;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Repositories.AprovacaoComercial;
using TopSys.TopConWeb.Domain.Interfaces.Repositories.AssinaturaEletronicaIntegracao;
using TopSys.TopConWeb.Domain.Interfaces.Repositories.CartaoPagamentoIntegracao;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Domain.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;
using TopSys.TopConWeb.Infra.Data.Repositories;
using TopSys.TopConWeb.Infra.Data.Repositories.AssinaturaEletronicaIntegracao;
using TopSys.TopConWeb.Infra.Data.Repositories.CartaoPagamentoIntegracao;
using TopSys.TopConWeb.Infra.Data.UoW;
using TopSys.TopConWeb.Infra.Integrations.Services;
using TopSys.TopConWeb.Infra.Legacy.Services;
using TopSys.TopConWeb.Infra.Reports;
using TopSys.TopConWeb.SharedKernel;
using TopSys.TopConWeb.SharedKernel.Events;
using TopSys.TopConWeb.Domain.Entities;
using Topsys.TopConWeb.SharedKernel.Helpers;
using System.Net.Http.Headers;
using TopSys.TopConWeb.Domain.Helpers;

namespace TopSys.TopConWeb.Infra.CrossCuting
{
    public static class DependencyResolver
    {
        /// <summary>
        /// TransientLifetimeManager - Cada Resolve gera uma nova instância.
        /// HierarchicalLifetimeManager - Utiliza Singleton
        /// </summary>
        /// <param name="container"></param>
        public static void Resolve(UnityContainer container)
        {
            //Registro DataContext
            container.RegisterType<AppDataContext, AppDataContext>(new HierarchicalLifetimeManager());

            //Registro UnitOfWork
            container.RegisterType<IUnitOfWork, UnitOfWork>(new HierarchicalLifetimeManager());

            //Registro dos Repositórios
            container.RegisterType(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            container.RegisterType<IObraRepository, ObraRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IUsuarioRepository, UsuarioRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IObraTaxaRepository, ObraTaxaRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IDemaisAprovacaoRepository, DemaisAprovacaoRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IAprovacaoScriptRepository, AprovacaoScriptRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IContratoRepository, ContratoRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IIntervenienteSequenceRepository, IntervenienteSequenceRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IIntervenienteRepository, IntervenienteRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ICadastroGeralRepository, CadastroGeralRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ICadastroDiversoRepository, CadastroDiversoRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IEnderecoRepository, EnderecoRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IMunicipioRepository, MunicipioRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IUsinaRepository, UsinaRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IVendedorRepository, VendedorRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IUsoRepository, UsoRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IPedraRepository, PedraRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ISlumpRepository, SlumpRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IResistenciaTipoRepository, ResistenciaTipoRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ITracoPrecoRepository, TracoPrecoRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ITracoCustoRepository, TracoCustoRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IPecaAConcretarRepository, PecaAConcretarRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IBombaPrecoRepository, BombaPrecoRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ICondicaoPagamentoRepository, CondicaoPagamentoRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ITipoCobrancaRepository, TipoCobrancaRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ICartaoBandeiraRepository, CartaoBandeiraRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IPortadorRepository, PortadorRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IPropostaRepository, PropostaRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IProgramacaoRepository, ProgramacaoRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IParametroRepository, ParametroRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ILogGeralRepository, LogGeralRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<INotaFiscalFisicaRepository, NotaFiscalFisicaRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IUsinaDistanciaCepRepository, UsinaDistanciaCepRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IEmpresaRepository, EmpresaRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IRepasseReajusteRepository, RepasseReajusteRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ITituloContasAReceberRepository, TituloContasAReceberRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ITituloContasAPagarRepository, TituloContasAPagarRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IDemaisServicosRepository, DemaisServicosRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IUnidadeRepository, UnidadeRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IMercadoriaRepository, MercadoriaRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IMunicipioTributacaoRepository, MunicipioTributacaoRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ICieloLioDadosRepository, CieloLioDadosRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ISolicitacaoPagamentoRepository, SolicitacaoPagamentoRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IContasAReceberRepository, ContasAReceberRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IParametroFinanceiroRepository, ParametroFinanceiroRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ICartaoTransacaoRepository, CartaoTransacaoRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IContratoPagamentoRepository, ContratoPagamentoRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IMovimentoBancoRepository, MovimentoBancoRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IFuncionarioRepository, FuncionarioRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IFuncionarioComplementoRepository, FuncionarioComplementoRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ICondicaoPagamentoParcelaRepository, CondicaoPagamentoParcelaRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ICustoServicoRepository, CustoServicoRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IFilialRepository, FilialRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IClicksignRepository, ClicksignRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IClicksignConfiguracaoRepository, ClicksignConfiguracaoRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IPreTracoPrecoRepository, PreTracoPrecoRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IIntegracaoRepository, IntegracaoRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IOpportunityRepository, OpportunityRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IArquivoRepository, ArquivoRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IEquipamentoRepository, EquipamentoRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ICentroCustoRepository, CentroCustoRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IObraFrenteRepository, ObraFrenteRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IBancoRepository, BancoRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ISituacaoFinanceiraRepository, SituacaoFinanceiraRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ITipoDeCobrancaRepository, TipoDeCobrancaRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IOperacaoFinanceiraRepository, OperacaoFinanceiraRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IFaturaRepository, FaturaRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ITipoDocumentoRepository, TipoDocumentoRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<INotaFiscalDigitalRepository, NotaFiscalDigitalRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ISegmentacaoRepository, SegmentacaoRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IDatabaseRepository, DatabaseRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IContratoTracoReajusteRepository, ContratoTracoReajusteRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IContratoBombaReajusteRepository, ContratoBombaReajusteRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IPropostaPropagandaRepository, PropostaPropagandaRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IUsuarioWebFiltroRepository, UsuarioWebFiltroRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IBoletoExternoRepository, BoletoExternoRepository>(new HierarchicalLifetimeManager());

            container.RegisterType<IPrensaRepository, PrensaRepository>(new HierarchicalLifetimeManager());

            container.RegisterType<IAprovacaoComercialUsinaRepository, AprovacaoComercialUsinaRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IAprovacaoComercialHierarquiaRepository, AprovacaoComercialHierarquiaRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IAprovacaoComercialHierarquiaCondicaoRepository, AprovacaoComercialHierarquiaCondicaoRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IAprovacaoComercialPendenteRepository, AprovacaoComercialPendenteRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IGrupoEconomicoRepository, GrupoEconomicoRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IObraProjecaoRepository, ObraProjecaoRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ITributacaoPisCofinsRepository, TributacaoPisCofinsRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ITributacaoReformaRepository, TributacaoReformaRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ILiberacaoAcessoRepository, LiberacaoAcessoRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IVisitaTipoRepository, VisitaTipoRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IOportunidadeTipoRepository, OportunidadeTipoRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IConcorrenteRepository, ConcorrenteRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ILeadFaseRepository, LeadFaseRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IMotivoPerdaRepository, MotivoPerdaRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ILeadRepository, LeadRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ILeadContatoRepository, LeadContatoRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IVisitaRepository, VisitaRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ITarefaRepository, TarefaRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ICompromissoRepository, CompromissoRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IOportunidadeRepository, OportunidadeRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IWebHookRepository, WebHookRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ISsoRepository, SsoRepository>(new HierarchicalLifetimeManager());


            //Registro dos Serviços
            container.RegisterType(typeof(IServiceBase<>), typeof(ServiceBase<>));
            container.RegisterType<IObraService, ObraService>(new HierarchicalLifetimeManager());
            container.RegisterType<IUsuarioService, UsuarioService>(new HierarchicalLifetimeManager());
            container.RegisterType<IObraTaxaService, ObraTaxaService>(new HierarchicalLifetimeManager());
            container.RegisterType<IDemaisAprovacaoService, DemaisAprovacaoService>(new HierarchicalLifetimeManager());
            container.RegisterType<IAprovacaoScriptService, AprovacaoScriptService>(new HierarchicalLifetimeManager());
            container.RegisterType<IContratoService, ContratoService>(new HierarchicalLifetimeManager());
            container.RegisterType<IContratoVersaoService, ContratoVersaoService>(new HierarchicalLifetimeManager());
            container.RegisterType<IIntervenienteSequenceControlService, IntervenienteSequenceControlService>(new HierarchicalLifetimeManager());
            container.RegisterType<IIntervenienteService, IntervenienteService>(new HierarchicalLifetimeManager());
            container.RegisterType<ICadastroGeralService, CadastroGeralService>(new HierarchicalLifetimeManager());
            container.RegisterType<ICadastroDiversoService, CadastroDiversoService>(new HierarchicalLifetimeManager());
            container.RegisterType<IEnderecoService, EnderecoService>(new HierarchicalLifetimeManager());
            container.RegisterType<IMunicipioService, MunicipioService>(new HierarchicalLifetimeManager());
            container.RegisterType<IUsinaService, UsinaService>(new HierarchicalLifetimeManager());
            container.RegisterType<IVendedorService, VendedorService>(new HierarchicalLifetimeManager());
            container.RegisterType<IUsoService, UsoService>(new HierarchicalLifetimeManager());
            container.RegisterType<IPedraService, PedraService>(new HierarchicalLifetimeManager());
            container.RegisterType<ISlumpService, SlumpService>(new HierarchicalLifetimeManager());
            container.RegisterType<IResistenciaTipoService, ResistenciaTipoService>(new HierarchicalLifetimeManager());
            container.RegisterType<ITracoPrecoService, TracoPrecoService>(new HierarchicalLifetimeManager());
            container.RegisterType<ITracoCustoService, TracoCustoService>(new HierarchicalLifetimeManager());
            container.RegisterType<IPecaAConcretarService, PecaAConcretarService>(new HierarchicalLifetimeManager());
            container.RegisterType<IBombaPrecoService, BombaPrecoService>(new HierarchicalLifetimeManager());
            container.RegisterType<ICondicaoPagamentoService, CondicaoPagamentoService>(new HierarchicalLifetimeManager());
            container.RegisterType<ITipoCobrancaService, TipoCobrancaService>(new HierarchicalLifetimeManager());
            container.RegisterType<ICartaoBandeiraService, CartaoBandeiraService>(new HierarchicalLifetimeManager());
            container.RegisterType<IPortadorService, PortadorService>(new HierarchicalLifetimeManager());
            container.RegisterType<IPropostaService, PropostaService>(new HierarchicalLifetimeManager());
            container.RegisterType<IProgramacaoService, ProgramacaoService>(new HierarchicalLifetimeManager());
            container.RegisterType<IParametroService, ParametroService>(new HierarchicalLifetimeManager());
            container.RegisterType<ILogGeralService, LogGeralService>(new HierarchicalLifetimeManager());
            container.RegisterType<INotaFiscalFisicaService, NotaFiscalFisicaService>(new HierarchicalLifetimeManager());
            container.RegisterType<IMunicipioTributacaoService, MunicipioTributacaoService>(new HierarchicalLifetimeManager());
            container.RegisterType<IUsinaDistanciaCepService, UsinaDistanciaCepService>(new HierarchicalLifetimeManager());
            container.RegisterType<IEmpresaService, EmpresaService>(new HierarchicalLifetimeManager());
            container.RegisterType<ITituloContasAReceberService, TituloContasAReceberService>(new HierarchicalLifetimeManager());
            container.RegisterType<IDemaisServicosService, DemaisServicosService>(new HierarchicalLifetimeManager());
            container.RegisterType<IUnidadeService, UnidadeService>(new HierarchicalLifetimeManager());
            container.RegisterType<IMercadoriaService, MercadoriaService>(new HierarchicalLifetimeManager());
            container.RegisterType<IContasAReceberService, ContasAReceberService>(new HierarchicalLifetimeManager());
            container.RegisterType<IOrganizacaoService, OrganizacaoService>(new HierarchicalLifetimeManager());
            container.RegisterType<IFuncionarioService, FuncionarioService>(new HierarchicalLifetimeManager());
            container.RegisterType<IFuncionarioComplementoService, FuncionarioComplementoService>(new HierarchicalLifetimeManager());
            container.RegisterType<IMovimentoBancoService, MovimentoBancoService>(new HierarchicalLifetimeManager());
            container.RegisterType<ICustoServicoService, CustoServicoService>(new HierarchicalLifetimeManager());
            container.RegisterType<IPreTracoPrecoService, PreTracoPrecoService>(new HierarchicalLifetimeManager());
            container.RegisterType<IIntegracaoService, IntegracaoService>(new HierarchicalLifetimeManager());
            container.RegisterType<IOpportunityService, OpportunityService>(new HierarchicalLifetimeManager());          
            container.RegisterType<ICalculoImpostosService, CalculoImpostosService>(new HierarchicalLifetimeManager());
            container.RegisterType<IAprovacaoComercialPendenteService, AprovacaoComercialPendenteService>(new HierarchicalLifetimeManager());
            container.RegisterType<IEquipamentoService, EquipamentoService>(new HierarchicalLifetimeManager());
            container.RegisterType<ICentroCustoService, CentroCustoService>(new HierarchicalLifetimeManager());
            container.RegisterType<IObraFrenteService, ObraFrenteService>(new HierarchicalLifetimeManager());
            container.RegisterType<IBancoService, BancoService>(new HierarchicalLifetimeManager());
            container.RegisterType<ISituacaoFinanceiraService, SituacaoFinanceiraService>(new HierarchicalLifetimeManager());
            container.RegisterType<ITipoDeCobrancaService, TipoDeCobrancaService>(new HierarchicalLifetimeManager());
            container.RegisterType<IOperacaoFinanceiraService, OperacaoFinanceiraService>(new HierarchicalLifetimeManager());
            container.RegisterType<IFaturaService, FaturaService>(new HierarchicalLifetimeManager());
            container.RegisterType<ITipoDocumentoService, TipoDocumentoService>(new HierarchicalLifetimeManager());
            container.RegisterType<INotaFiscalDigitalService, NotaFiscalDigitalService>(new HierarchicalLifetimeManager());
            container.RegisterType<ISegmentacaoService, SegmentacaoService>(new HierarchicalLifetimeManager());
            container.RegisterType<IContratoTracoReajusteService, ContratoTracoReajusteService>(new HierarchicalLifetimeManager());
            container.RegisterType<IContratoBombaReajusteService, ContratoBombaReajusteService>(new HierarchicalLifetimeManager());
            container.RegisterType<IOportunidadeService, OportunidadeService>(new HierarchicalLifetimeManager());
            container.RegisterType<IPropostaPropagandaService, PropostaPropagandaService>(new HierarchicalLifetimeManager());
            container.RegisterType<IBoletoExternoService, BoletoExternoService>(new HierarchicalLifetimeManager());

            container.RegisterType<IPrensaService, PrensaService>(new HierarchicalLifetimeManager());

            container.RegisterType<IPreTracoPrecoService, PreTracoPrecoService>(new HierarchicalLifetimeManager());
            container.RegisterType<IArquivoService, ArquivoService>(new HierarchicalLifetimeManager());

            container.RegisterType<IdentityHelperService, IdentityHelperService>(new HierarchicalLifetimeManager());
            container.RegisterType<IFilialService, FilialService>(new HierarchicalLifetimeManager());

            container.RegisterType<IAprovacaoComercialHierarquiaService, AprovacaoComercialHierarquiaService>();
            container.RegisterType<IAprovacaoComercialService, AprovacaoComercialService>();
            container.RegisterType<IGrupoEconomicoService, GrupoEconomicoService>();
            container.RegisterType<IObraProjecaoService, ObraProjecaoService>();
            container.RegisterType<ITributacaoPisCofinsService, TributacaoPisCofinsService>(new HierarchicalLifetimeManager());
            container.RegisterType<ITributacaoReformaService, TributacaoReformaService>(new HierarchicalLifetimeManager());
            container.RegisterType<ILiberacaoAcessoService, LiberacaoAcessoService>(new HierarchicalLifetimeManager());
            container.RegisterType<IVisitaTipoService, VisitaTipoService>(new HierarchicalLifetimeManager());
            container.RegisterType<IOportunidadeTipoService, OportunidadeTipoService>(new HierarchicalLifetimeManager());
            container.RegisterType<IConcorrenteService, ConcorrenteService>(new HierarchicalLifetimeManager());
            container.RegisterType<ILeadFaseService, LeadFaseService>(new HierarchicalLifetimeManager());
            container.RegisterType<IMotivoPerdaService, MotivoPerdaService>(new HierarchicalLifetimeManager());
            container.RegisterType<ILeadService, ILeadService>(new HierarchicalLifetimeManager());
            container.RegisterType<ILeadContatoService, ILeadContatoService>(new HierarchicalLifetimeManager());
            container.RegisterType<IVisitaService, VisitaService>(new HierarchicalLifetimeManager());
            container.RegisterType<ITarefaService, TarefaService>(new HierarchicalLifetimeManager());
            container.RegisterType<ICompromissoService, CompromissoService>(new HierarchicalLifetimeManager());
            container.RegisterType<IWebHookService, WebHookService>(new HierarchicalLifetimeManager());
            container.RegisterType<IClicksignConfiguracaoService, ClicksignConfiguracaoService>(new HierarchicalLifetimeManager());
            container.RegisterType<ISsoService, SsoService>(new HierarchicalLifetimeManager());


            //Registro dos Application Services
            container.RegisterType(typeof(IApplicationServiceBase<>), typeof(ApplicationServiceBase<>));
            container.RegisterType<IObraApplicationService, ObraApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IUsuarioApplicationService, UsuarioApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IObraTaxaApplicationService, ObraTaxaApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IContratoApplicationService, ContratoApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<ICadastroGeralApplicationService, CadastroGeralApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<ICadastroDiversoApplicationService, CadastroDiversoApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IEnderecoApplicationService, EnderecoApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IIntervenienteApplicationService, IntervenienteApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IUsinaApplicationService, UsinaApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IVendedorApplicationService, VendedorApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IUsoApplicationService, UsoApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IPedraApplicationService, PedraApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<ISlumpApplicationService, SlumpApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IResistenciaTipoApplicationService, ResistenciaTipoApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<ITracoPrecoApplicationService, TracoPrecoApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IPecaAConcretarApplicationService, PecaAConcretarApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IBombaPrecoApplicationService, BombaPrecoApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<ICondicaoPagamentoApplicationService, CondicaoPagamentoApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<ITipoCobrancaApplicationService, TipoCobrancaApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<ICartaoBandeiraApplicationService, CartaoBandeiraApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IPortadorApplicationService, PortadorApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IPropostaApplicationService, PropostaApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IProgramacaoApplicationService, ProgramacaoApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IParametroApplicationService, ParametroApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IEmpresaApplicationService, EmpresaApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<ITituloContasAReceberApplicationService, TituloContasAReceberApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IDemaisServicosApplicationService, DemaisServicosApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IUnidadeApplicationService, UnidadeApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IMercadoriaApplicationService, MercadoriaApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IMunicipioTributacaoApplicationService, MunicipioTributacaoApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<ICartaoPagamentoIntegracaoApplicationService, CartaoPagamentoIntegracaoApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IContasAReceberApplicationService, ContasAReceberApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IFuncionarioApplicationService, FuncionarioApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IFuncionarioComplementoApplicationService, FuncionarioComplementoApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IMovimentoBancoApplicationService, MovimentoBancoApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<ICustoServicoApplicationService, CustoServicoApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IAssinaturaEletronicaApplicationService, AssinaturaEletronicaApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IClicksignConfiguracaoApplicationService, ClicksignConfiguracaoApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IFilialApplicationService, FilialApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IPreTracoPrecoApplicationService, PreTracoPrecoApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IIntegracaoApplicationService, IntegracaoApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IOpportunityApplicationService, OpportunityApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IEquipamentoApplicationService, EquipamentoApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IAprovacaoComercialHierarquiaApplicationService, AprovacaoComercialHierarquiaApplicationService>();
            container.RegisterType<IAprovacaoComercialApplicationService, AprovacaoComercialApplicationService>();
            container.RegisterType<IGrupoEconomicoApplicationService, GrupoEconomicoApplicationService>();
            container.RegisterType<ICentroCustoApplicationService, CentroCustoApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IObraFrenteApplicationService, ObraFrenteApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IBancoApplicationService, BancoApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<ITipoDeCobrancaApplicationService, TipoDeCobrancaApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<ISituacaoFinanceiraApplicationService, SituacaoFinanceiraApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IRemessaApplicationService, RemessaApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IOperacaoFinanceiraApplicationService, OperacaoFinanceiraApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IFaturaApplicationService, FaturaApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<ITipoDocumentoApplicationService, TipoDocumentoApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<INotaFiscalDigitalApplicationService, NotaFiscalDigitalApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<ISegmentacaoApplicationService, SegmentacaoApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IContratoTracoReajusteApplicationService, ContratoTracoReajusteApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IContratoBombaReajusteApplicationService, ContratoBombaReajusteApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IObraProjecaoApplicationService, ObraProjecaoApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<ITributacaoPisCofinsApplicationService, TributacaoPisCofinsApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<ITributacaoReformaApplicationService, TributacaoReformaApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<ILiberacaoAcessoApplicationService, LiberacaoAcessoApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IVisitaTipoApplicationService, VisitaTipoApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IOportunidadeTipoApplicationService, OportunidadeTipoApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IConcorrenteApplicationService, ConcorrenteApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<ILeadFaseApplicationService, LeadFaseApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IMotivoPerdaApplicationService, MotivoPerdaApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<ILeadApplicationService, LeadApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IVisitaApplicationService, VisitaApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<ITarefaApplicationService, TarefaApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<ICompromissoApplicationService, CompromissoApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IOportunidadeApplicationService, OportunidadeApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IPropostaPropagandaApplicationService, PropostaPropagandaApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IWebHookApplicationService, WebHookApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<ISsoApplicationService, SsoApplicationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IBoletoExternoApplicationService, BoletoExternoApplicationService>(new HierarchicalLifetimeManager());

            container.RegisterType<IPrensaApplicationService, PrensaApplicationService>(new HierarchicalLifetimeManager());

            //Registro das Notificações de Domínio
            container.RegisterType<IHandler<DomainNotification>, DomainNotificationHandler>(new HierarchicalLifetimeManager());

            //Registro dos Serviços do sistema legado
            container.RegisterType<IComercialLegacyService, ComercialLegacyService>(new HierarchicalLifetimeManager());

            //Registro dos Serviços de Integrações
            container.RegisterType<IViaCepService, ViaCepService>(new HierarchicalLifetimeManager());
            container.RegisterType<IGoogleServices, GoogleServices>(new HierarchicalLifetimeManager());
            container.RegisterType<ICieloLioIntegrationService, CieloLioIntegrationService>(new HierarchicalLifetimeManager());
            container.RegisterType<IAssinaturaEletronicaIntegrationService, ClicksignIntegrationService>(new HierarchicalLifetimeManager());

            //Registro do ReportService
            container.RegisterType<ReportService, ReportService>(new HierarchicalLifetimeManager());

            //Registro dos Parâmetros
            container.RegisterType<ParametroIntervenienteSequence, ParametroIntervenienteSequence>(new HierarchicalLifetimeManager());

            //Header
            container.RegisterType<IHeaderProvider, HeaderProvider>(new PerThreadLifetimeManager());
        }
    }
}
