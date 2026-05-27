using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using TopSys.TopConWeb.Application.DTOS.Request.Contrato.AprovarCoincidenciasCadastraisRequest;
using TopSys.TopConWeb.Application.DTOS.Request.Contrato.ContratoRevalidacaoCadastroRequest;
using TopSys.TopConWeb.Application.DTOS.Request.Contrato.ReprovarCoincidenciasCadastraisRequest;
using TopSys.TopConWeb.Application.DTOS.Response.Contrato.ContratoGeradoResponse;
using TopSys.TopConWeb.Application.DTOS.Response.Contrato.ContratoRevalidacaoCadastroResponse;
using TopSys.TopConWeb.Application.DTOS.Response.Contrato;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;
using TopSys.TopConWeb.Infra.Reports;
using System.IO;
using TopSys.TopConWeb.SharedKernel.Validation;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.LegacyServices;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Application.DTOS.Response.Contrato.ContratoIntegracao;
using System;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;
using TopSys.TopConWeb.Domain.Interfaces.Integrations;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Application.DTOS.Request.Contrato.ContratoIntegracao;

namespace TopSys.TopConWeb.Application
{
    public class ContratoApplicationService : ApplicationServiceBase<Contrato>, IContratoApplicationService
    {

        private readonly IContratoService _contratoService;
        private readonly IIntervenienteService _intervenienteService;
        private readonly IObraService _obraService;
        private readonly IContratoVersaoService _contratoVersaoService;
        private readonly IComercialLegacyService _comercialLegacyService;
        private readonly IParametroService _parametroService;
        private readonly ReportService _reportService;
        private readonly IHeaderProvider _headerProvider;
        private readonly IContratoPagamentoRepository _contratoPagamentoRepository;
        private readonly IWebHookApplicationService _webHookApplicationService;
        private readonly IObraApplicationService _obraApplicationService;

        public ContratoApplicationService(
            IContratoService contratoService,
            IIntervenienteService intervenienteService,
            IObraService obraService,
            IContratoVersaoService contratoVersaoService,
            IComercialLegacyService comercialLegacyService,
            IParametroService parametroService,
            IContratoPagamentoRepository contratoPagamentoRepository,
            IWebHookApplicationService webHookApplicationService,
            IUnitOfWork unityOfWork,
            ReportService reportService,
            IHeaderProvider headerProvider, 
            IObraApplicationService obraApplicationService)
            : base(contratoService, unityOfWork)
        {
            _contratoService = contratoService;
            _intervenienteService = intervenienteService;
            _obraService = obraService;
            _contratoVersaoService = contratoVersaoService;
            _comercialLegacyService = comercialLegacyService;
            _parametroService = parametroService;
            _reportService = reportService;
            _headerProvider = headerProvider;
            _webHookApplicationService = webHookApplicationService;
            _contratoPagamentoRepository = contratoPagamentoRepository;
            _obraApplicationService = obraApplicationService;
        }

        public void AprovarContratoRevalidacaoDeCadastro(string usuario, ContratoRevalidacaoCadastroRequest contratoRevalidacaoCadastro, ref string mensagem)
        {
            using (var scope = new TransactionScope())
            {
                string observacaoLog = contratoRevalidacaoCadastro.observacaoLog;

                var contrato = AutoMapper.Mapper.Map(contratoRevalidacaoCadastro, new Contrato());

                contrato = _contratoService.ObterPorId(contrato.Usina, contrato.Ano, contrato.Numero);
                contrato.Interveniente = _intervenienteService.ObterPorId(contrato.IntervenienteCodigo);

                var interv = contrato.Interveniente;

                _intervenienteService.AtualizarLimite(contrato.Interveniente, interv.LimiteData, interv.LimiteValor, interv.BloqueioMotivoCodigo ?? 0);

                if (Commit())
                    _contratoService.AprovarContratoRevalidacaoContrato(usuario, contrato, observacaoLog, ref mensagem);

                if (!_notifications.HasNotifications())
                    scope.Complete();
                else
                    scope.Dispose();
            }
        }

        public bool GerarEValidarContrato(string usuario, int propostaUsina, int propostaAno, int propostaNumero, out ContratoGeradoResponse contrato, out string mensagem)
        {
            var success = _contratoService.GerarContrato(usuario, propostaUsina, propostaAno, propostaNumero, out Contrato ctr, out mensagem);
            contrato = AutoMapper.Mapper.Map(ctr, new ContratoGeradoResponse());

            if (success)
            {

                var obra = _obraService
                    .ListarFiltrados(t => t.UsinaCodigo == propostaUsina && t.AnoChamada == propostaAno && t.NumChamada == propostaNumero)
                    .FirstOrDefault();

                _obraService.AprovarAutomaticamentePagamentosDaCieloLio(obra);

                string[] cartaoTransacaoNotifications = { "cartaoTransacao", "Status Processo", "contasAReceber", "cartaoBandeira", "contasAReceber-propostaStatus", "contasAReceber-Contrato Pagamentos", "contasAReceber-Contrato Pagamentos Cartao", "contasAReceber-ContratoPagamentoDetalheCartao", "TipoCobranca" };
                _notifications.RemoveNotificationsByKey(cartaoTransacaoNotifications);

                var statusFinanceiroAnterior = obra.StatusFinanceiro;

                _obraService.AtualizarStatusFinanceiro(obra, usuario);

                _obraApplicationService.ProcessarAdicaoWebhookContratoPendenteAprovacaoFinanceira(obra.Numero, obra.UsinaCodigo, statusFinanceiroAnterior);

                var aprovContratoDirAuto = _parametroService.ObterParametroN("TopCon", "AprovContratoDirAuto").Equals("1");
                var contratoNovo = _contratoService.ObterPorId(ctr.Usina, ctr.Ano, ctr.Numero);

                _comercialLegacyService.ValidarContrato(contratoNovo, usuario, out mensagem, aprovContratoDirAuto);

                var contratoPagamentos = _contratoPagamentoRepository.ListarContratoPagamentosDetalhados(contrato.Usina, contrato.Ano, contrato.Numero).ToList();
                _webHookApplicationService.AdicionarWebHookContratoPagamento(ctr, contratoPagamentos, EWebHookTipoEvento.Insert);

                Commit();
            }

            return success;
                
        }

        public bool GerarContrato(string usuario, int propostaUsina, int propostaAno, int propostaNumero, out ContratoGeradoResponse contrato, out string mensagem)
        {
            var success = _contratoService.GerarContrato(usuario, propostaUsina, propostaAno, propostaNumero, out Contrato ctr, out  mensagem);
            contrato = AutoMapper.Mapper.Map(ctr, new ContratoGeradoResponse());

            if (success)
            {

                var obra = _obraService
                    .ListarFiltrados(t => t.UsinaCodigo == propostaUsina && t.AnoChamada == propostaAno && t.NumChamada == propostaNumero)
                    .FirstOrDefault();

                _obraService.AprovarAutomaticamentePagamentosDaCieloLio(obra);

                string[] cartaoTransacaoNotifications = { "cartaoTransacao", "Status Processo", "contasAReceber", "cartaoBandeira", "contasAReceber-propostaStatus", "contasAReceber-Contrato Pagamentos", "contasAReceber-Contrato Pagamentos Cartao", "contasAReceber-ContratoPagamentoDetalheCartao", "TipoCobranca" };
                _notifications.RemoveNotificationsByKey(cartaoTransacaoNotifications);

                var statusFinanceiroAnterior = obra.StatusFinanceiro;

                _obraService.AtualizarStatusFinanceiro(obra, usuario);

                _obraApplicationService.ProcessarAdicaoWebhookContratoPendenteAprovacaoFinanceira(obra.Numero, obra.UsinaCodigo, statusFinanceiroAnterior);

                var contratoPagamentos = _contratoPagamentoRepository.ListarContratoPagamentosDetalhados(contrato.Usina, contrato.Ano, contrato.Numero).ToList();
                _webHookApplicationService.AdicionarWebHookContratoPagamento(ctr, contratoPagamentos, EWebHookTipoEvento.Insert);
            }

            return success;
        }

        public IEnumerable<ContratoRevalidacaoCadastroResponse> ListarContratosRevalidacaoCadastro()
        {
            return AutoMapper.Mapper.Map(_contratoService.ListarContratosRevalidacaoCadastro(), new List<ContratoRevalidacaoCadastroResponse>());
        }

        public void AprovarCoincidenciasCadastrais(string usuario, AprovarCoincidenciasCadastraisRequest aprovaCoincidenciasCadastraisRequest)
        {
            var contrato = _contratoService.ObterPorId(aprovaCoincidenciasCadastraisRequest.Usina, aprovaCoincidenciasCadastraisRequest.Ano, aprovaCoincidenciasCadastraisRequest.Numero);
            contrato.AprovarCoincidencia(usuario);
            Commit();
        }

        public void ReprovarCoincidenciasCadastrais(string usuario, ReprovarCoincidenciasCadastraisRequest reprovaCoincidenciasCadastraisRequest)
        {
            var contrato = _contratoService.ObterPorId(reprovaCoincidenciasCadastraisRequest.Usina, reprovaCoincidenciasCadastraisRequest.Ano, reprovaCoincidenciasCadastraisRequest.Numero);
            contrato.ReprovarCoincidencia(usuario);
            Commit();
        }

        public ICollection<ContratoVersaoResponse> ListarContratoVersoesAprovados(int codUsina, int anoContrato, int numeroContrato)
        {
            return AutoMapper.Mapper.Map(_contratoVersaoService.ListarContratoVersoesAprovados(codUsina, anoContrato, numeroContrato), new List<ContratoVersaoResponse>());
        }

        public Stream ObterAditivoReport(int versao, int codUsina, int anoProposta, int numeroProposta, int anoContrato, int numeroContrato)
        {
            var tracosAlterados = "";

            var diferencas = _contratoVersaoService.ObterAditivoReport(versao, codUsina, anoProposta, numeroProposta, out tracosAlterados);
            return _reportService.GetAditivoReport(versao, codUsina, anoContrato, numeroContrato, diferencas, tracosAlterados);
        }

        public void SalvarPDFContratoVersao(int versao, int codUsina, int anoContrato, int numeroContrato)
        {
            var contrato = _reportService.GetContratoReport(codUsina, anoContrato, numeroContrato, 2, true);
            _contratoVersaoService.SalvarPDFContratoVersao(versao, codUsina, anoContrato, numeroContrato, contrato);
        }

        public Stream ObterPDFContratoVersao(int versao, int codUsina, int anoContrato, int numeroContrato)
        {
            var ultimaVersao = _contratoService.GetUltimaVersaoContrato(codUsina, anoContrato, numeroContrato);

            var contrato = _contratoService.ObterPorId<ContratoVersao>(versao, codUsina, anoContrato, numeroContrato);

            if (versao == ultimaVersao)
            {
                if (_parametroService.ObterParametroN("web", "GeraAditivoContratoSemAprovCadastro") == "1" && contrato.Status != EContratoStatus.Aprovado)
                {
                    _contratoService.CriarTabelaTemporariaTaxaExtraVersao(versao, codUsina, anoContrato, numeroContrato);

                    return _reportService.GetContratoVersaoReport(codUsina, anoContrato, numeroContrato, 1, versao);
                }

                return _reportService.GetContratoReport(codUsina, anoContrato, numeroContrato, 1);
            }
                
            
            return _contratoVersaoService.ObterPDFContratoVersao(versao, codUsina, anoContrato, numeroContrato);
        }

        public ContratoVersaoParametrosResponse ObterParametrosContratoVersao()
        {
            ContratoVersaoParametros parametros = new ContratoVersaoParametros();

            parametros.VersionamentoTraco = _parametroService.ObterParametroN("web", "VersionamentoTraco").Contains("true");
            parametros.VersionamentoBomba = _parametroService.ObterParametroN("web", "VersionamentoBomba").Contains("true");
            parametros.VersionamentoTaxaExtra = _parametroService.ObterParametroN("web", "VersionamentoTaxaExtra").Contains("true");
            parametros.VersionamentoCondicaoPagamento = _parametroService.ObterParametroN("web", "VersionamentoCondicaoPagamento").Contains("true");
            parametros.VersionamentoEnderecoObra = _parametroService.ObterParametroN("web", "VersionamentoEnderecoObra").Contains("true");
            parametros.VersionamentoDemaisServicos = _parametroService.ObterParametroN("web", "VersionamentoDemaisServicos").Contains("true");
            parametros.VersionamentoReajusteContrato = _parametroService.ObterParametroN("web", "VersionamentoReajusteContrato").Contains("true");

            return AutoMapper.Mapper.Map(parametros, new ContratoVersaoParametrosResponse());
        }

        public ResultDTO<ContratoSimplificadoResponse> ObterPorId(int idUsina, int contratoAno, int contratoNumero)
        {
            try
            {
                var contratoBanco = _contratoService.ObterPorId(idUsina, contratoAno, contratoNumero);
                var contrato = AutoMapper.Mapper.Map(contratoBanco, new ContratoSimplificadoResponse());
                var contratoPagamentos = _contratoPagamentoRepository.ListarContratoPagamentosDetalhados(idUsina, contratoAno, contratoNumero);

                contrato.VendedorExternalId = _contratoService.ObterPorId<Vendedor>(contrato.VendedorCodigo).ExternalId;

                if (contrato == null)
                    return new ResultDTO<ContratoSimplificadoResponse>(
                    EResultDTOStatus.Error,
                    EResourcesContratoSimplificado.CONTRATO_SIMPLIFICADO_ERROR_TCON393831.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesContratoSimplificado.CONTRATO_SIMPLIFICADO_ERROR_TCON393831.GetMessageCode());

                var obraBanco = _contratoService.ListarFiltrados<Obra>(f => f.UsinaCodigo == idUsina && f.AnoContrato == contratoAno && f.NumContrato == contratoNumero).FirstOrDefault();
                var obraMunicipo = _contratoService.ListarFiltrados<Municipio>(x => x.Codigo == obraBanco.EnderecoMunicipioCodigo).FirstOrDefault();

                contrato.EnderecoCep = obraBanco.EnderecoCep;
                contrato.EnderecoLogradouro = obraBanco.EnderecoLogradouro;
                contrato.EnderecoNumero = obraBanco.EnderecoNumero;
                contrato.EnderecoComplemento = obraBanco.EnderecoComplemento;
                contrato.EnderecoBairro = obraBanco.EnderecoBairro;
                contrato.EnderecoMunicipioCodigo = obraBanco.EnderecoMunicipioCodigo;
                contrato.EnderecoUf = obraMunicipo.Uf;
                contrato.EnderecoMunicipio = obraMunicipo.Nome;
                contrato.Pagamentos = AutoMapper.Mapper.Map(contratoPagamentos, new List<ContratoPagamentoSimplificadoResponse>());

                return new ResultDTO<ContratoSimplificadoResponse>(EResultDTOStatus.Success, "", contrato);
            }
            catch (Exception)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<ContratoSimplificadoResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }

        public ICollection<ContratoFinalidadeResponse> ListarFinalidades()
        {
            return AutoMapper.Mapper.Map(_contratoService.ListarFinalidades(), new List<ContratoFinalidadeResponse>());
        }

        public ResultDTO<ContratoAprovacaoFinanceiraResponse> AprovacaoFinanceira(PagamentosRequest pagamentosRequest)
        {
            try
            {
                List<Error> errors = new List<Error>();

                List<int> detalhes = new List<int>();

                var versaoAtual = _contratoService.GetUltimaVersaoContrato(pagamentosRequest.UsinaCodigo, pagamentosRequest.ContratoAno, pagamentosRequest.ContratoNumero);

                var obraAux = _obraService.ObterObraPorContrato(pagamentosRequest.UsinaCodigo, pagamentosRequest.ContratoAno, pagamentosRequest.ContratoNumero);

                if (versaoAtual == 0)
                {
                    var obra = _obraService.ListarObraPagamentos(obraAux.UsinaCodigo, obraAux.Numero);

                    foreach (var pagamentoRequest in pagamentosRequest.Pagamentos)
                    {
                        var pagamento = obra.ObraPagamentos.Where(t => t.Sequencia == pagamentoRequest.Sequencia).FirstOrDefault();

                        if (pagamento != null)
                            if ((new[] { "DP", "CC", "CD", "CH", "CP", "BE", "DN" }).Contains(pagamento.Forma))
                                foreach (var detalhe in obra.ContratoPagamentos.Where(t => t.Sequencia == pagamento.Sequencia).FirstOrDefault().Detalhes)
                                    detalhes.Add(detalhe.DetalheSequencia);

                        errors = ValidaAprovacaoPagamentos(pagamentoRequest, pagamento, detalhes);

                        detalhes.Clear();

                        if (errors.Count == 0)
                            pagamento.StatusAprovacao = AtualizaStatusAprovacao(pagamentoRequest, pagamento);
                    }

                    if (errors.Count > 0)
                        return new ResultDTO<ContratoAprovacaoFinanceiraResponse>(
                            EResultDTOStatus.Error,
                            EResourcesDefaultMessages.DEFAULT_MESSAGES_ERROR_PROCESSING_RECORDS.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                            errors);

                    if (obra.ObraPagamentos.Where(t => t.StatusAprovacao == EStatusAprovacao.Aprovado).ToList().Count > 0)
                        _obraService.AprovarObraPagamentos("API", obra, new List<MovimentoBanco>());

                    foreach (var pagamentoRequest in pagamentosRequest.Pagamentos.Where(t => t.Reprovado))
                    {
                        var pagamento = obra.ObraPagamentos.Where(t => t.Sequencia == pagamentoRequest.Sequencia && t.StatusAprovacao == EStatusAprovacao.Pendente).FirstOrDefault();

                        if (pagamento != null)
                            _comercialLegacyService.DesaprovarCondicaoPagamento(pagamentosRequest.UsinaCodigo, pagamentosRequest.ContratoAno, pagamentosRequest.ContratoNumero, pagamento.Sequencia, "API", true);
                    }

                    _obraService.AtualizarStatusFinanceiro(obra, "API");

                    return new ResultDTO<ContratoAprovacaoFinanceiraResponse>(EResultDTOStatus.Success, "Success when making financial approval");
                }
                
                var obraVersao = _obraService.ListarObraPagamentos(versaoAtual, obraAux.UsinaCodigo, obraAux.Numero);

                foreach (var pagamentoRequest in pagamentosRequest.Pagamentos)
                {
                    var pagamento = obraVersao.ObraPagamentos.Where(t => t.Sequencia == pagamentoRequest.Sequencia).FirstOrDefault();

                    if (pagamento !=  null)
                        if ((new[] { "DP", "CC", "CD", "CH", "CP", "BE", "DN" }).Contains(pagamento.Forma))
                            foreach (var detalhe in obraVersao.ContratoPagamentos.Where(t => t.Sequencia == pagamento.Sequencia).FirstOrDefault().Detalhes)
                                detalhes.Add(detalhe.DetalheSequencia);

                    errors = ValidaAprovacaoPagamentos(pagamentoRequest, pagamento, detalhes);

                    detalhes.Clear();

                    if (errors.Count == 0)
                        pagamento.StatusAprovacao = AtualizaStatusAprovacao(pagamentoRequest, pagamento);
                }

                if (errors.Count > 0)
                    return new ResultDTO<ContratoAprovacaoFinanceiraResponse>(
                        EResultDTOStatus.Error,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_ERROR_PROCESSING_RECORDS.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        errors);

                if (obraVersao.ObraPagamentos.Where(t => t.StatusAprovacao == EStatusAprovacao.Aprovado).ToList().Count > 0)
                    _obraService.AprovarObraPagamentos("API", obraVersao, new List<MovimentoBanco>());

                foreach (var pagamentoRequest in pagamentosRequest.Pagamentos.Where(t => t.Reprovado))
                {
                    var pagamento = obraVersao.ObraPagamentos.Where(t => t.Sequencia == pagamentoRequest.Sequencia && t.StatusAprovacao == EStatusAprovacao.Pendente).FirstOrDefault();

                    if (pagamento != null)
                        _comercialLegacyService.DesaprovarCondicaoPagamento(pagamentosRequest.UsinaCodigo, pagamentosRequest.ContratoAno, pagamentosRequest.ContratoNumero, pagamento.Sequencia, "API", true);
                }

                _obraService.AtualizarStatusFinanceiro(obraVersao, "API");

                var contratoAtual = _contratoService.ContratoVersaoObterPorId(obraVersao.NumeroVersao, obraVersao.UsinaCodigo, obraVersao.AnoContrato.Value, obraVersao.NumContrato.Value);
                if (contratoAtual != null)
                {
                    if (contratoAtual.Status == EContratoStatus.Aprovado)
                    {
                        _obraApplicationService.AtualizarContratoComVersao(obraVersao.NumeroVersao, obraVersao.UsinaCodigo, obraVersao.AnoChamada ?? 0, obraVersao.NumChamada ?? 0);

                        var obraTracosVersao = _obraService.ListarFiltrados<ObraTracoVersao>(t => t.UsinaCodigo == obraVersao.UsinaCodigo && t.ObraCodigo == obraVersao.Numero && t.NumeroVersao == obraVersao.NumeroVersao);
                        var obraTracos = AutoMapper.Mapper.Map(obraTracosVersao, new List<ObraTraco>());
                        foreach (var traco in obraTracos)
                        {
                            traco.PrecoReajustadoAtual = traco.M3PrecoProposto;
                        }
                        _obraApplicationService.AtualizarValorReajustePropostaItemVersao(obraVersao.NumeroVersao, obraVersao.UsinaCodigo, obraVersao.AnoChamada ?? 0, obraVersao.NumChamada ?? 0, obraTracos);

                        var obraBombasVersao = _obraService.ListarFiltrados<ObraBombaVersao>(t => t.UsinaCodigo == obraVersao.UsinaCodigo && t.ObraCodigo == obraVersao.Numero && t.NumeroVersao == obraVersao.NumeroVersao);
                        var obraBombas = AutoMapper.Mapper.Map(obraBombasVersao, new List<ObraBomba>());
                        foreach (var bomba in obraBombas)
                        {
                            bomba.M3ReajustadoAteAtual = bomba.M3PropostoAte;
                            bomba.TaxaMinimaReajustadaAtual = bomba.TaxaMinimaPrecoProposto;
                            bomba.M3PrecoReajustadoAtual = bomba.M3PrecoProposto;
                        }
                        _obraApplicationService.AtualizarValorReajustePropostaBombaVersao(obraVersao.NumeroVersao, obraVersao.UsinaCodigo, obraVersao.AnoChamada ?? 0, obraVersao.NumChamada ?? 0, obraBombas);

                        Commit();
                    }
                }

                return new ResultDTO<ContratoAprovacaoFinanceiraResponse>(EResultDTOStatus.Success, "Success when making financial approval");
            }
            catch (Exception)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<ContratoAprovacaoFinanceiraResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
            } 
        }

        private List<Error> ValidaAprovacaoPagamentos(PagamentoAprovarRequest pagamentoRequest, ObraPagamento pagamento, List<int> detalhes)
        {
            var errors = new List<Error>();

            if (pagamento == null)
                errors.Add(new Error(
                    EResourcesContratoSimplificado.CONTRATO_SIMPLIFICADO_ERROR_TCON393833.GetMessageCode(),
                    EResourcesContratoSimplificado.CONTRATO_SIMPLIFICADO_ERROR_TCON393833.GetResourceMessage(_headerProvider.GetAcceptLanguage())));
            else
            {
                if (pagamento.NecessitaAprovacao)
                {
                    if (pagamento.StatusAprovacao == EStatusAprovacao.NaoNecessita &&
                           (new[] { "DP", "CC", "CD", "CH", "CP", "BE", "DN" }).Contains(pagamento.Forma) &&
                           detalhes.Count == 0)
                    {
                        errors.Add(new Error(
                            EResourcesContratoSimplificado.CONTRATO_SIMPLIFICADO_ERROR_TCON393832.GetMessageCode(),
                            EResourcesContratoSimplificado.CONTRATO_SIMPLIFICADO_ERROR_TCON393832.GetResourceMessage(_headerProvider.GetAcceptLanguage()), pagamento.Sequencia));

                        return errors;
                    }
                }

                if (pagamento.StatusAprovacao == EStatusAprovacao.NaoNecessita && pagamentoRequest.Aprovado)
                    errors.Add(new Error(
                        EResourcesContratoSimplificado.CONTRATO_SIMPLIFICADO_ERROR_TCON393834.GetMessageCode(),
                        EResourcesContratoSimplificado.CONTRATO_SIMPLIFICADO_ERROR_TCON393834.GetResourceMessage(_headerProvider.GetAcceptLanguage()), pagamento.Sequencia));
            }
            
            return errors;
        }

        private EStatusAprovacao AtualizaStatusAprovacao(PagamentoAprovarRequest pagamentoRequest, ObraPagamento pagamento)
        {
            EStatusAprovacao statusAprovacao = EStatusAprovacao.Pendente;

            if (pagamentoRequest.Aprovado)
                statusAprovacao = EStatusAprovacao.Aprovado;

            if (pagamentoRequest.Reprovado)
                if (pagamento.StatusAprovacao != EStatusAprovacao.NaoNecessita)
                    statusAprovacao = EStatusAprovacao.Reprovado;

            return statusAprovacao;
        }
    }
}
