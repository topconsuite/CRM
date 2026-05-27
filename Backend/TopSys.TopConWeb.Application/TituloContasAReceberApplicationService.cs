using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Topsys.TopConWeb.SharedKernel.Common;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;
using TopSys.TopConWeb.Application.DTOS.Request.TituloContasAReceber;
using TopSys.TopConWeb.Application.DTOS.Response.Fatura;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Application.DTOS.Response.TituloContasAReceber;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain;
using TopSys.TopConWeb.Domain.Constants;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Entities.ReguaDeCobranca;
using TopSys.TopConWeb.Domain.Interfaces.Integrations;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Domain.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;

namespace TopSys.TopConWeb.Application
{
    public class TituloContasAReceberApplicationService : ApplicationServiceBase<TituloContasAReceber>, ITituloContasAReceberApplicationService
    {
        private readonly ITituloContasAReceberService _tituloContasAReceberService;
        private readonly IOrganizacaoService _organizacaoService;
        private readonly IIntervenienteService _intervenienteService;
        private readonly IHeaderProvider _headerProvider;

        public TituloContasAReceberApplicationService(ITituloContasAReceberService tituloContasAReceberService, IUnitOfWork unityOfWork, IHeaderProvider headerProvider, IIntervenienteService intervenienteService, IOrganizacaoService organizacaoService)
            : base(tituloContasAReceberService, unityOfWork)
        {
            _tituloContasAReceberService = tituloContasAReceberService;
            _headerProvider = headerProvider;
            _intervenienteService = intervenienteService;
            _organizacaoService = organizacaoService;
        }

        public IEnumerable<TituloContasAReceberResponse> ListarPorNumeroCartaoAutorizacao(int numeroCartao, string autorizacao)
        {
            return AutoMapper.Mapper.Map(_tituloContasAReceberService.ListarPorNumeroCartaoAutorizacao(numeroCartao, autorizacao), new List<TituloContasAReceberResponse>());
        }

        public IEnumerable<TituloContasAReceberResponse> ListarPorNumeroCartaoAutorizacaoDuplicado(int idUsina, int contratoAno, int contratoNumero, int numeroCartao, string autorizacao)
        {
            return AutoMapper.Mapper.Map(_tituloContasAReceberService.ListarPorNumeroCartaoAutorizacaoDuplicado(idUsina, contratoAno, contratoNumero, numeroCartao, autorizacao), new List<TituloContasAReceberResponse>());
        }

        // FOR PUBLIC INTEGRATIONS

        public ResultDTO<PublicoTituloContasAReceberResponseList> ObterPorParametros(int empresa, int tipoDocumento, string serieDocumento, int numeroDocumento, int sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv, int desdobramento)
        {
            var result = AutoMapper.Mapper.Map(_tituloContasAReceberService.ObterPorParametros(empresa, tipoDocumento, serieDocumento, numeroDocumento, sequencia, codBancoBand, numAgencia, numConta, numContaDv, desdobramento), new PublicoTituloContasAReceberResponse());
            if (result != null && result.TipoDeCobranca.Codigo > 0) AutoMapper.Mapper.Map(_tituloContasAReceberService.RetornaTipoDeCobrancaComDescricao(result.TipoDeCobranca.Codigo), result.TipoDeCobranca);
            
            if(result != null) AutoMapper.Mapper.Map(_tituloContasAReceberService.RetornaSegmentacao(result.EmpresaCodigo, result.DocumentoTipoCodigo, result.DocumentoSerie, result.IntervenienteCodigo, result.DocumentoNumero), result.Segmentacao);
            
            List<PublicoTituloContasAReceberResponse> resultList = new List<PublicoTituloContasAReceberResponse>
            {
                result
            };

            if (result == null)
            {
                return new ResultDTO<PublicoTituloContasAReceberResponseList>(
                    EResultDTOStatus.Error,
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());
            }

            PublicoTituloContasAReceberResponseList requestResult = new PublicoTituloContasAReceberResponseList
            {
                ContasAReceberTitulos = resultList
            };

            return new ResultDTO<PublicoTituloContasAReceberResponseList>(EResultDTOStatus.Success, "", requestResult);
        }
        public ResultDTO<PublicoTituloContasAReceberResponseList> ObterPorParametros(int empresa, int tipoDocumento, string serieDocumento, int numeroDocumento,
            int codBancoBand, int numAgencia, int numConta, int numContaDv)
        {
            var result = AutoMapper.Mapper.Map(_tituloContasAReceberService.ObterPorParametros(empresa, tipoDocumento, serieDocumento, numeroDocumento, codBancoBand, numAgencia, numConta, numContaDv), new List<PublicoTituloContasAReceberResponse>());

            if (result.Count != 0)
            {
                foreach (var titulo in result)
                {
                    if (titulo.TipoDeCobranca.Codigo > 0) {AutoMapper.Mapper.Map(_tituloContasAReceberService.RetornaTipoDeCobrancaComDescricao(titulo.TipoDeCobranca.Codigo), titulo.TipoDeCobranca); }
                    
                    AutoMapper.Mapper.Map(_tituloContasAReceberService.RetornaSegmentacao(titulo.EmpresaCodigo, titulo.DocumentoTipoCodigo, titulo.DocumentoSerie, titulo.IntervenienteCodigo, titulo.DocumentoNumero), titulo.Segmentacao);

                }
            }

            if (result.Count == 0)
            {
                return new ResultDTO<PublicoTituloContasAReceberResponseList>(
                    EResultDTOStatus.Error,
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());
            }

            PublicoTituloContasAReceberResponseList requestResult = new PublicoTituloContasAReceberResponseList
            {
                ContasAReceberTitulos = result
            };

            return new ResultDTO<PublicoTituloContasAReceberResponseList>(EResultDTOStatus.Success, "", requestResult);
        }

        public ResultDTO<PublicoReguaDeCobrancaTituloContasAReceberResponse> ObterPorIdReguaDeCobranca(string id)
        {
            var organizacaoResponse = _organizacaoService.ObterOrganizacaoReguaDeCobranca();

            if (organizacaoResponse.Codigo == 0 || !id.EndsWith(organizacaoResponse.Codigo.ToString()))
            {
                return new ResultDTO<PublicoReguaDeCobrancaTituloContasAReceberResponse>(
                    EResultDTOStatus.Error,
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(_headerProvider.GetAcceptLanguage(), nameof(Organizacao)),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode()
                );
            }

            id = id.Remove(id.Length - organizacaoResponse.Codigo.ToString().Length);
         
            var tituloResponse = _tituloContasAReceberService.ObterPorOriginalIdReguaDeCobranca(id);

            if (tituloResponse == null)
            {
                return new ResultDTO<PublicoReguaDeCobrancaTituloContasAReceberResponse>(
                    EResultDTOStatus.Error,
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage(), nameof(TituloContasAReceber)),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());
            }

            var intervenienteResponse = _intervenienteService.ObterPorId(tituloResponse.IntervenienteCodigo);

            if (intervenienteResponse == null)
            {
                return new ResultDTO<PublicoReguaDeCobrancaTituloContasAReceberResponse>(
                    EResultDTOStatus.Error,
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(_headerProvider.GetAcceptLanguage(), nameof(Interveniente)),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode()
                );
            }

            var linkSegundaViaBoletoResponse = _tituloContasAReceberService.ObterLinkSegundaViaBoleto(tituloResponse.BancoBoleto);

            var result = AutoMapper.Mapper.Map<PublicoReguaDeCobrancaTituloContasAReceberResponse>(
                new ReguaDeCobrancaTituloContasAReceber(tituloResponse, intervenienteResponse, organizacaoResponse, linkSegundaViaBoletoResponse));

            return new ResultDTO<PublicoReguaDeCobrancaTituloContasAReceberResponse>(EResultDTOStatus.Success, "", result); ;
        }

        public ResultDTO<PublicoTituloContasAReceberResponseList> Listar(DateTime? dataEmissao, DateTime? dataOperacao, int tipoDocumento, int? centroCusto, string serieDocumento, long? numeroDocumento, int cliente, int pagina = 0, int limite = 0)
        {
            if (pagina == 0) pagina = 1;
            if (limite == 0 || limite > 100) limite = 10;

            var result = AutoMapper.Mapper.Map(_tituloContasAReceberService.Listar(dataEmissao, dataOperacao, tipoDocumento, centroCusto, serieDocumento, numeroDocumento, cliente, pagina, limite), new List<PublicoTituloContasAReceberResponse>());

            if (result.Count != 0)
            {
                foreach (var titulo in result)
                {
                    if (titulo.TipoDeCobranca.Codigo > 0) { AutoMapper.Mapper.Map(_tituloContasAReceberService.RetornaTipoDeCobrancaComDescricao(titulo.TipoDeCobranca.Codigo), titulo.TipoDeCobranca); }
                    
                    AutoMapper.Mapper.Map(_tituloContasAReceberService.RetornaSegmentacao(titulo.EmpresaCodigo, titulo.DocumentoTipoCodigo, titulo.DocumentoSerie, titulo.IntervenienteCodigo, titulo.DocumentoNumero), titulo.Segmentacao);

                }
            }

            if (result.Count == 0)
                return new ResultDTO<PublicoTituloContasAReceberResponseList>(
                    EResultDTOStatus.Error,
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

            PublicoTituloContasAReceberResponseList requestResult = new PublicoTituloContasAReceberResponseList
            {
                ContasAReceberTitulos = result
            };

            return new ResultDTO<PublicoTituloContasAReceberResponseList>(EResultDTOStatus.Success, "", requestResult);
        }

        public ResultDTO<PagedList<PublicoTituloContasAReceberResponse>> ObterPorDataAtualizacao(DateTime dataInicio, DateTime? dataFim, int? page, int? limit)
        {
            try
            {
                if (limit > 100) limit = 10;

                var result = AutoMapper.Mapper.Map(_tituloContasAReceberService.ObterPorDataAtualizacao(dataInicio, dataFim, page ?? 1, limit ?? 10), new PagedList<PublicoTituloContasAReceberResponse>());
                
                if (result.Records.Count() != 0)
                {
                    foreach (var titulo in result.Records)
                    {
                        if (titulo.TipoDeCobranca.Codigo > 0) { AutoMapper.Mapper.Map(_tituloContasAReceberService.RetornaTipoDeCobrancaComDescricao(titulo.TipoDeCobranca.Codigo), titulo.TipoDeCobranca); }
                        
                        AutoMapper.Mapper.Map(_tituloContasAReceberService.RetornaSegmentacao(titulo.EmpresaCodigo, titulo.DocumentoTipoCodigo, titulo.DocumentoSerie, titulo.IntervenienteCodigo, titulo.DocumentoNumero), titulo.Segmentacao);

                    }
                }

                if (result.RecordCount == 0)
                    return new ResultDTO<PagedList<PublicoTituloContasAReceberResponse>>(
                        EResultDTOStatus.Error,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

                return new ResultDTO<PagedList<PublicoTituloContasAReceberResponse>>(EResultDTOStatus.Success, "", result);

            }
            catch (Exception)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<PagedList<PublicoTituloContasAReceberResponse>>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }

        public ResultDTO<PublicoTituloContasAReceberAdicionarResponse> TituloContasAReceberAdicionar(TituloContasAReceberAdicionarRequest[] requests)
        {

            var errorsInRequest = new List<string[]>();
            var errorsReturn = new List<Error>();

            int? operacaoMovimentoBancario = null;

            foreach (TituloContasAReceberAdicionarRequest request in requests)
            {

                TituloContasAReceber tituloContasAReceber = _tituloContasAReceberService.ObterPorParametros(request.EmpresaCodigo, request.DocumentoTipoCodigo, request.DocumentoSerie, (int)request.DocumentoNumero, (int)request.DocumentoSequencia, 0, 0, 0, 0,request.Desdobramento);

                if (tituloContasAReceber != null)
                {
                    return new ResultDTO<PublicoTituloContasAReceberAdicionarResponse>(
                        EResultDTOStatus.Error,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetResourceMessage(_headerProvider.GetAcceptLanguage(), "AccountsReceivable"),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetMessageCode());
                }

                if (_tituloContasAReceberService.DentroDoMesFechamento(tituloContasAReceber.EmpresaCodigo, tituloContasAReceber.DataLiquidacao))
                {
                    return new ResultDTO<PublicoTituloContasAReceberAdicionarResponse>(
                        EResultDTOStatus.Error,
                        EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f38.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f38.GetMessageCode());
                }

                errorsInRequest = _tituloContasAReceberService.ValidaCamposRequestAdicionarTituloContasAReceber(request.EmpresaCodigo, request.DocumentoTipoCodigo, request.IntervenienteCodigo, request.OperacaoFinanceiraCodigo, request.CentroCustoCodigo, request.Situacao, request.BancoPortador, request.OperacaoLiquidacao, request.BancoLiquidacao, request.DataLiquidacao);
                errorsInRequest.AddRange(_tituloContasAReceberService.ValidaSaldoAdicionarRequest(request.Valor, request.SomaRecebimentos));

                if (request.OperacaoLiquidacao > 0 || Math.Abs(request.LiquidacaoValorRecebido + request.LiquidacaoJuros) > 0)
                {
                    errorsInRequest.AddRange(_tituloContasAReceberService.ValidaBancoLiquidacao(request.EmpresaCodigo, request.OperacaoLiquidacao, request.BancoLiquidacao));
                    errorsInRequest.AddRange(_tituloContasAReceberService.ValidaValoresLiquidacao(request.Valor, request.LiquidacaoValorRecebido, request.LiquidacaoJuros, request.LiquidacaoDesconto, request.LiquidacaoDespesas));
                    
                    if (request.Valor>0)
                    {
                        operacaoMovimentoBancario =
                            _tituloContasAReceberService.RetornaOperacaoMovimentoBancario(request.OperacaoLiquidacao);
                    }
                    else
                    {
                        operacaoMovimentoBancario = _tituloContasAReceberService.RetornaOperacaoBaixa(request.OperacaoLiquidacao);
                    }
                    
                    if (operacaoMovimentoBancario == null) errorsInRequest.Add(new string[] { 
                        EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f36.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f36.GetMessageCode()
                    });
                }

                if (errorsInRequest.Count > 0)
                {
                    foreach (string[] erros in errorsInRequest)
                    {
                        errorsReturn.Add(new Error(erros[0], erros[1], errorsInRequest.IndexOf(erros)));
                    }
                }
            }

            if (errorsReturn.Count > 0) 
                return new ResultDTO<PublicoTituloContasAReceberAdicionarResponse>(
                    EResultDTOStatus.Error,
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_ERROR_PROCESSING_RECORDS.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    errorsReturn);

            using (var scope = new TransactionScope())
            {
                try
                {
                    foreach (TituloContasAReceberAdicionarRequest request in requests)
                    {
                        var newTituloContasAReceber = new TituloContasAReceber();

                        newTituloContasAReceber = AutoMapper.Mapper.Map(request, newTituloContasAReceber);

                        //Correcao entity
                        if (newTituloContasAReceber.NossoNumero == null) newTituloContasAReceber.NossoNumero = "";
                        if (newTituloContasAReceber.CartaoNumero == null) newTituloContasAReceber.CartaoNumero = "";
                        newTituloContasAReceber.ValorBruto = newTituloContasAReceber.Valor;


                        if (request.OperacaoLiquidacao > 0)
                        {
                            var TpMovimentotpBaixa = _tituloContasAReceberService.RetornaTipoMovimentoEBaixa(request.EmpresaCodigo);

                            string tipoLiquidacao = _tituloContasAReceberService.DefineTipoLiquidacao(request.Valor, request.LiquidacaoValorRecebido, -1);

                            if (tipoLiquidacao == "TOTAL")
                            {
                                TituloContasAReceber tituloLiquidacao = new TituloContasAReceber();
                                tituloLiquidacao = AutoMapper.Mapper.Map(newTituloContasAReceber, tituloLiquidacao);
                                
                                //CRIA MOVIMENTO BANCARIO(Não irá criar caso seja um título de adiantamento)
                                long idMovimentoBancario = 0;
                                var parametrosMovimento =
                                    _tituloContasAReceberService.RetornaParametrosMovimento(operacaoMovimentoBancario);
                                if (tituloLiquidacao.Valor>0)
                                {

                                    var movimentoBancarioLiquidacao = new MovimentoBanco(tituloLiquidacao,
                                        TpMovimentotpBaixa.Item1, operacaoMovimentoBancario, parametrosMovimento.Item1,
                                        "API ACCOUNTS RECEIVABLE FULL LIQ", "API ACCOUNTS RECEIVABLE");

                                    idMovimentoBancario = _tituloContasAReceberService.AdicionarMovimento(movimentoBancarioLiquidacao);
                                }
                                

                                //LIQUIDA TITULO
                                var bancoLiquidacaoOperacao = _tituloContasAReceberService.RetornaBancoDeLiquidacao(tituloLiquidacao.OperacaoLiquidacao);
                                var valorMoraLiquidada = tituloLiquidacao.LiquidacaoDespesas + tituloLiquidacao.LiquidacaoJuros - tituloLiquidacao.LiquidacaoDesconto;

                                if (tituloLiquidacao.Valor>0)
                                    tituloLiquidacao.IdMovimentoBanco = idMovimentoBancario;
                                
                                tituloLiquidacao.LiquidacaoLoteBaixa = parametrosMovimento.Item2;
                                tituloLiquidacao.DataLiquidacaoCliente = tituloLiquidacao.DataLiquidacao;
                                tituloLiquidacao.ValorMoraNaoLiquidado = 0;
                                tituloLiquidacao.MultaMoraCalculado = 0;
                                tituloLiquidacao.DescontoMora = 0;
                                tituloLiquidacao.IdLiquidacao = "API AR -" + DateTime.Now.ToString("y-MM-dd");
                                tituloLiquidacao.DocumentoLiquidacao = 0;
                                tituloLiquidacao.SomaRecebimentos = newTituloContasAReceber.Valor;
                                tituloLiquidacao.LiquidacaoValorRecebido = tituloLiquidacao.Valor + valorMoraLiquidada;
                                tituloLiquidacao.LiquidadoEmCheque = "N";
                               
                                if (bancoLiquidacaoOperacao != null) tituloLiquidacao.AtualizaBanco = (int)bancoLiquidacaoOperacao;
                                tituloLiquidacao.Saldo = 0;

                                //ATUALIZA MORA
                                tituloLiquidacao.JurosMoraCalculado = 0;

                                _tituloContasAReceberService.Adicionar(tituloLiquidacao);

                                //CRIA VINCULO MOVIMENTO TITULO
                                _tituloContasAReceberService.CriaEAtualizaVinculoMovimentoTitulo(tituloLiquidacao.EmpresaCodigo, tituloLiquidacao.DocumentoTipoCodigo, tituloLiquidacao.DocumentoSerie, int.Parse(tituloLiquidacao.DocumentoSequencia), tituloLiquidacao.Desdobramento, tituloLiquidacao.DocumentoNumero, tituloLiquidacao.LiquidacaoValorRecebido, tituloLiquidacao.IntervenienteCodigo, idMovimentoBancario);
                            }
                            else if (tipoLiquidacao == "PARCIAL")
                            {
                                TituloContasAReceber tituloLiquidacao = new TituloContasAReceber();
                                tituloLiquidacao = AutoMapper.Mapper.Map(newTituloContasAReceber, tituloLiquidacao);
                                
                                
                                //CRIA MOVIMENTO BANCARIO(Não irá criar caso seja um título de adiantamento)
                                long idMovimentoBancario = 0;
                                var parametrosMovimento =
                                    _tituloContasAReceberService.RetornaParametrosMovimento(operacaoMovimentoBancario);
                                if (tituloLiquidacao.Valor>0)
                                {

                                    var movimentoBancarioLiquidacao = new MovimentoBanco(tituloLiquidacao,
                                        TpMovimentotpBaixa.Item1, operacaoMovimentoBancario, parametrosMovimento.Item1,
                                        "API ACCOUNTS RECEIVABLE FULL LIQ", "API ACCOUNTS RECEIVABLE");

                                    idMovimentoBancario = _tituloContasAReceberService.AdicionarMovimento(movimentoBancarioLiquidacao);
                                }

                                //LIQUIDA TITULO
                                var bancoLiquidacaoOperacao = _tituloContasAReceberService.RetornaBancoDeLiquidacao(tituloLiquidacao.OperacaoLiquidacao);
                                var desdobramento = _tituloContasAReceberService.RetornaDesdobramentoMaximo(tituloLiquidacao.EmpresaCodigo, tituloLiquidacao.DocumentoTipoCodigo, tituloLiquidacao.DocumentoSerie, tituloLiquidacao.DocumentoNumero, tituloLiquidacao.DocumentoSequencia);
                                var valorMoraLiquidada = tituloLiquidacao.LiquidacaoDespesas + tituloLiquidacao.LiquidacaoJuros - tituloLiquidacao.LiquidacaoDesconto;

                                tituloLiquidacao.Desdobramento = (int)desdobramento;
                                
                                if (tituloLiquidacao.Valor>0)
                                    tituloLiquidacao.IdMovimentoBanco = idMovimentoBancario;
                                
                                tituloLiquidacao.LiquidacaoLoteBaixa = parametrosMovimento.Item2;
                                tituloLiquidacao.ValorMoraNaoLiquidado = 0;
                                tituloLiquidacao.MultaMoraCalculado = 0;
                                tituloLiquidacao.DescontoMora = 0;
                                tituloLiquidacao.SomaRecebimentos = 0;
                                tituloLiquidacao.IdLiquidacao = "API AR -" + DateTime.Now.ToString("y-MM-dd");
                                tituloLiquidacao.AtualizaBanco = (int)bancoLiquidacaoOperacao;
                                tituloLiquidacao.Valor = 0;
                                tituloLiquidacao.ValorBruto = 0;
                                tituloLiquidacao.Saldo = 0;
                                tituloLiquidacao.ValorRetencoes = 0;
                                tituloLiquidacao.Situacao = 0;
                                tituloLiquidacao.DataSituacao = null;
                                tituloLiquidacao.Observacao = "";
                                tituloLiquidacao.LiquidadoEmCheque = "N";
                                tituloLiquidacao.BancoPortador = 0;       
                                tituloLiquidacao.DataLiquidacaoCliente = request.DataLiquidacao;
                                tituloLiquidacao.DataLiquidacao = request.DataLiquidacao;

                                _tituloContasAReceberService.Adicionar(tituloLiquidacao);

                                //ATUALIZA MORA E CORRIGE CAMPOS DO TÍTULO PRINCIPAL
                                
                                newTituloContasAReceber.SomaRecebimentos += (tituloLiquidacao.LiquidacaoValorRecebido - valorMoraLiquidada);
                                newTituloContasAReceber.MultaMoraCalculado = 0;
                                newTituloContasAReceber.JurosMoraCalculado = 0;
                                newTituloContasAReceber.LiquidacaoValorRecebido = 0;
                                newTituloContasAReceber.DataLiquidacao = null;
                                newTituloContasAReceber.DataLiquidacaoCliente = null;
                                newTituloContasAReceber.OperacaoLiquidacao = 0;
                                newTituloContasAReceber.BancoLiquidacao = 0;

                               
                                _tituloContasAReceberService.Adicionar(newTituloContasAReceber);

                                //CRIA VINCULO MOVIMENTO TITULO
                                _tituloContasAReceberService.CriaEAtualizaVinculoMovimentoTitulo(tituloLiquidacao.EmpresaCodigo, tituloLiquidacao.DocumentoTipoCodigo, tituloLiquidacao.DocumentoSerie, int.Parse(tituloLiquidacao.DocumentoSequencia), tituloLiquidacao.Desdobramento, tituloLiquidacao.DocumentoNumero, tituloLiquidacao.LiquidacaoValorRecebido, tituloLiquidacao.IntervenienteCodigo, idMovimentoBancario);
                            }
                            else throw new Exception();
                        }
                        else
                        {
                            _tituloContasAReceberService.Adicionar(newTituloContasAReceber);
                        }
                    }
                    Commit();
                    scope.Complete();

                    var result = new PublicoTituloContasAReceberAdicionarResponse(requests.Length);
                    return new ResultDTO<PublicoTituloContasAReceberAdicionarResponse>(EResultDTOStatus.Success, "Sucessfully inserted", result, "");
                }
                catch
                {
                    scope.Dispose();
                    var errorMessage =
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(
                            _headerProvider.GetAcceptLanguage());
                    var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                    return new ResultDTO<PublicoTituloContasAReceberAdicionarResponse>(EResultDTOStatus.Error, errorMessage, errorCode);

                }
            }
        }

        public ResultDTO<PublicoTituloContasAReceberResponse> TituloContasAReceberAtualizar(int empresa, int tipoDocumento, string serieDocumento, int numeroDocumento, int sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv, int desdobramento, TituloContasAReceberAtualizarRequest request)
        {
            TituloContasAReceber tituloContasAReceber = _tituloContasAReceberService.ObterPorParametros(empresa, tipoDocumento, serieDocumento, numeroDocumento, sequencia, codBancoBand, numAgencia, numConta, numContaDv, desdobramento);
            var errorsReturn = new List<Error>();

            if (tituloContasAReceber == null)
            {
                return new ResultDTO<PublicoTituloContasAReceberResponse>(
                    EResultDTOStatus.Error,
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());
            }

            if (_tituloContasAReceberService.DentroDoMesFechamento(tituloContasAReceber.EmpresaCodigo, tituloContasAReceber.DataLiquidacao) 
                || _tituloContasAReceberService.DentroDoMesFechamento(tituloContasAReceber.EmpresaCodigo, request.DataLiquidacao))
            {
                return new ResultDTO<PublicoTituloContasAReceberResponse>(
                    EResultDTOStatus.Error,
                    EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f38.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f38.GetMessageCode());
            }

            if (tituloContasAReceber.EstaLiquidado && request.RequisicaoDeCancelamento)
            {
                var errorsOnValidate = _tituloContasAReceberService.ValidaCancelamentoDeTitulo(tituloContasAReceber);

                if (errorsOnValidate.Any())
                {
                    errorsReturn.AddRange(errorsOnValidate
                        .Select((erros, index) => new Error(erros[0], erros[1], null)));
                 
                    return new ResultDTO<PublicoTituloContasAReceberResponse>(
                        EResultDTOStatus.Error,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_ERROR_PROCESSING_RECORDS.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        errorsReturn,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_ERROR_PROCESSING_RECORDS.GetMessageCode());
                    
                }

                return AtualizarTituloContasAReceber(tituloContasAReceber, request, tituloJaLiquidado: true, cancelamento: true);
            }

      
            bool tituloJaLiquidado = false;

            //Verifica se o titulo já foi liquidado e chama o erro caso a liquidação seja total
            if (!(Math.Abs((decimal)tituloContasAReceber.SomaRecebimentos) + Math.Abs((decimal?)request.LiquidacaoValorRecebido??0) <= Math.Abs((decimal)tituloContasAReceber.Valor)))
            {
                if (tituloContasAReceber.OperacaoLiquidacao > 0)
                {
                    tituloJaLiquidado = true;

                    if (tituloContasAReceber.LiquidacaoDesconto != request.LiquidacaoDesconto || tituloContasAReceber.LiquidacaoDespesas != request.LiquidacaoDespesas ||
                        tituloContasAReceber.LiquidacaoJuros != request.LiquidacaoJuros || tituloContasAReceber.LiquidacaoValorRecebido != request.LiquidacaoValorRecebido ||
                        tituloContasAReceber.DataLiquidacao != request.DataLiquidacao || tituloContasAReceber.BancoLiquidacao != request.BancoLiquidacao || tituloContasAReceber.LiquidacaoValorRecebido != request.LiquidacaoValorRecebido)
                    {
                        return new ResultDTO<PublicoTituloContasAReceberResponse>(
                        EResultDTOStatus.Error,
                        EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f37.GetResourceMessage(_headerProvider.GetAcceptLanguage(), "AccountsReceivable"),
                        EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f37.GetMessageCode());
                    }
                }
            }
            
           
            var errorsInRequest = new List<string>();

            var fieldsValidationResult = _tituloContasAReceberService.ValidaCamposRequestAtualizarTituloContasAReceber(tituloContasAReceber.EmpresaCodigo, tituloContasAReceber.DocumentoTipoCodigo, request.IntervenienteCodigo, request.OperacaoFinanceiraCodigo, request.CentroCustoCodigo, request.Situacao, request.BancoPortador, request.OperacaoLiquidacao, request.BancoLiquidacao, request.DataLiquidacao);
            fieldsValidationResult.AddRange(_tituloContasAReceberService.ValidaSaldoAtualizarRequest(request.Valor, null, tituloContasAReceber.Valor, tituloContasAReceber.SomaRecebimentos));

            if (request.OperacaoLiquidacao > 0 && tituloJaLiquidado == false)
            {
                fieldsValidationResult.AddRange(_tituloContasAReceberService.ValidaBancoLiquidacao(tituloContasAReceber.EmpresaCodigo, request.OperacaoLiquidacao, request.BancoLiquidacao));
                fieldsValidationResult.AddRange(_tituloContasAReceberService.ValidaValoresLiquidacao(tituloContasAReceber.Valor, request.LiquidacaoValorRecebido, request.LiquidacaoJuros, request.LiquidacaoDesconto, request.LiquidacaoDespesas));
                fieldsValidationResult.AddRange(_tituloContasAReceberService.ValidaValoresRecebimento(tituloContasAReceber.SomaRecebimentos, tituloContasAReceber.Valor, (request.LiquidacaoValorRecebido-request.LiquidacaoJuros-request.LiquidacaoDespesas)));
            }

            if (request.OperacaoLiquidacao > 0 && !tituloJaLiquidado)
            {
                int? operacaoMovimentoBancario = null;
                if (tituloContasAReceber.Valor > 0)
                    operacaoMovimentoBancario = _tituloContasAReceberService.RetornaOperacaoMovimentoBancario(request.OperacaoLiquidacao);
                else
                {
                    operacaoMovimentoBancario = _tituloContasAReceberService.RetornaOperacaoBaixa(request.OperacaoLiquidacao);
                }

                if (operacaoMovimentoBancario == null)
                {
                    return new ResultDTO<PublicoTituloContasAReceberResponse>(
                        EResultDTOStatus.Error,
                        EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f48.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f48.GetMessageCode());
                }
            }


            if (fieldsValidationResult.Count > 0)
            {
                
                foreach (string[] errors in fieldsValidationResult)
                {
                    errorsReturn.Add(new Error(errors[0], errors[1], fieldsValidationResult.IndexOf(errors)));
                }

                return new ResultDTO<PublicoTituloContasAReceberResponse>(
                    EResultDTOStatus.Error,
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_ERROR_PROCESSING_RECORDS.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    errorsReturn);
            }

            return AtualizarTituloContasAReceber(tituloContasAReceber, request, tituloJaLiquidado);
        }

        public ResultDTO<PublicoTituloContasAReceberResponse> AtualizarTituloContasAReceber (TituloContasAReceber tituloContasAReceber, TituloContasAReceberAtualizarRequest request, bool tituloJaLiquidado, bool cancelamento = false)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    var newRetornoResponse = new PublicoTituloContasAReceberResponse();

                    if (cancelamento)
                    {
                        _tituloContasAReceberService.CancelarRecebimentoDeTitulo(tituloContasAReceber);

                        Commit();

                        if (tituloContasAReceber.Desdobramento == Titulo.DESDOBRAMENTO_TITULO_PRINCIPAL)
                        {
                            tituloContasAReceber = _tituloContasAReceberService.ObterPorParametros(tituloContasAReceber.EmpresaCodigo, tituloContasAReceber.DocumentoTipoCodigo,
                                tituloContasAReceber.DocumentoSerie, (int)tituloContasAReceber.DocumentoNumero, int.Parse(tituloContasAReceber.DocumentoSequencia), tituloContasAReceber.BancoCodigoOficial,
                                tituloContasAReceber.BancoNumeroAgencia, (int)tituloContasAReceber.BancoNumeroConta, tituloContasAReceber.BancoDvConta, tituloContasAReceber.Desdobramento);

                        }
                        else
                        {
                            tituloContasAReceber = new TituloContasAReceber();
                        }

                        scope.Complete();

                        return new ResultDTO<PublicoTituloContasAReceberResponse>(EResultDTOStatus.Success, "Sucessfully update", AutoMapper.Mapper.Map(tituloContasAReceber, newRetornoResponse));
                    }

                    //Correcao entity
                    if (request.DocumentoSerie == null) request.DocumentoSerie = tituloContasAReceber.DocumentoSerie;
                    if (request.DocumentoSequencia == null) request.DocumentoSequencia = int.Parse(tituloContasAReceber.DocumentoSequencia);
                    if (request.DataEmissao == null && tituloContasAReceber.DataEmissao != null) request.DataEmissao = tituloContasAReceber.DataEmissao;
                    if (request.DataOperacao == null && tituloContasAReceber.DataOperacao != null) request.DataOperacao = tituloContasAReceber.DataOperacao;
                    if (request.DataVencimento == null && tituloContasAReceber.DataVencimento != null) request.DataVencimento = tituloContasAReceber.DataVencimento;
                    if (request.DataLiquidacao == null && tituloContasAReceber.DataLiquidacao != null && tituloJaLiquidado) request.DataLiquidacao = tituloContasAReceber.DataLiquidacao;
                    if (request.DataSituacao == null && tituloContasAReceber.DataSituacao != null) request.DataSituacao = tituloContasAReceber.DataSituacao;
                    if (request.IntervenienteCodigo == null) request.IntervenienteCodigo = tituloContasAReceber.IntervenienteCodigo;
                    if (request.Valor != null) tituloContasAReceber.ValorBruto = request.Valor??0 + tituloContasAReceber.ValorRetencoes;
                    if (request.Valor == null) request.Valor = tituloContasAReceber.Valor;

                    var newTituloContasAReceber = AutoMapper.Mapper.Map(request, tituloContasAReceber);

                    _tituloContasAReceberService.Atualizar(newTituloContasAReceber);


                    if (request.OperacaoLiquidacao > 0 && tituloJaLiquidado == false)
                    {
                        int? operacaoMovimentoBancario; 
                        if (tituloContasAReceber.Valor>0)
                            operacaoMovimentoBancario = _tituloContasAReceberService.RetornaOperacaoMovimentoBancario(request.OperacaoLiquidacao);
                        else
                        {
                            operacaoMovimentoBancario = _tituloContasAReceberService.RetornaOperacaoBaixa(request.OperacaoLiquidacao);
                        }
                        if (operacaoMovimentoBancario == null) throw new Exception();

                        var TpMovimentotpBaixa = _tituloContasAReceberService.RetornaTipoMovimentoEBaixa(newTituloContasAReceber.EmpresaCodigo);
                        string tipoLiquidacao = _tituloContasAReceberService.DefineTipoLiquidacao(tituloContasAReceber.Valor, request.LiquidacaoValorRecebido, tituloContasAReceber.SomaRecebimentos);

                        if (tipoLiquidacao == "TOTAL" || tipoLiquidacao == "PARCIAL TOTAL")
                        {

                            //CRIA MOVIMENTO BANCARIO(Não irá criar caso seja um título de adiantamento)
                            long idMovimentoBancario = 0;
                            var parametrosMovimento =
                                _tituloContasAReceberService.RetornaParametrosMovimento(operacaoMovimentoBancario);
                            if (tituloContasAReceber.Valor>0)
                            {
                                
                                var movimentoBancarioLiquidacao = new MovimentoBanco(newTituloContasAReceber,
                                    TpMovimentotpBaixa.Item1, operacaoMovimentoBancario, parametrosMovimento.Item1,
                                    "API ACCOUNTS RECEIVABLE FULL LIQ", "API ACCOUNTS RECEIVABLE");
                                if (tipoLiquidacao == "PARCIAL TOTAL")
                                    movimentoBancarioLiquidacao.Valor = (float)request.LiquidacaoValorRecebido;
                                idMovimentoBancario =
                                    _tituloContasAReceberService.AdicionarMovimento(movimentoBancarioLiquidacao);
                            }

                            //LIQUIDA TITULO
                            var bancoLiquidacaoOperacao = _tituloContasAReceberService.RetornaBancoDeLiquidacao(newTituloContasAReceber.OperacaoLiquidacao);
                            var valorMoraLiquidada = newTituloContasAReceber.LiquidacaoDespesas + newTituloContasAReceber.LiquidacaoJuros - newTituloContasAReceber.LiquidacaoDesconto;
                            
                            if (tituloContasAReceber.Valor>0)
                                newTituloContasAReceber.IdMovimentoBanco = idMovimentoBancario;
                             
                            newTituloContasAReceber.LiquidacaoLoteBaixa = parametrosMovimento.Item2;
                            newTituloContasAReceber.DataLiquidacaoCliente = newTituloContasAReceber.DataLiquidacao;
                            newTituloContasAReceber.ValorMoraNaoLiquidado = 0;
                            newTituloContasAReceber.MultaMoraCalculado = 0;
                            newTituloContasAReceber.DescontoMora = 0;
                            newTituloContasAReceber.SomaRecebimentos = newTituloContasAReceber.Valor;
                            newTituloContasAReceber.IdLiquidacao = "API AR -" + DateTime.Now.ToString("yyyy-MM-dd");
                            newTituloContasAReceber.DocumentoLiquidacao = 0;
                            newTituloContasAReceber.LiquidadoEmCheque = "N";

                            if (tipoLiquidacao == "PARCIAL TOTAL" && _tituloContasAReceberService.RetornaDesdobramentoMaximo(newTituloContasAReceber.EmpresaCodigo, newTituloContasAReceber.DocumentoTipoCodigo, newTituloContasAReceber.DocumentoSerie, newTituloContasAReceber.DocumentoNumero, (newTituloContasAReceber).DocumentoSequencia.ToString()) > 1)
                            {
                                newTituloContasAReceber.LiquidacaoValorRecebido = request.LiquidacaoValorRecebido??0;
                            }
                            else
                            {
                                newTituloContasAReceber.LiquidacaoValorRecebido = newTituloContasAReceber.Valor + valorMoraLiquidada;
                            }
                            
                            if (tituloContasAReceber.DocumentoTipoCodigo == 17 && tituloContasAReceber.Valor < 0)
                                newTituloContasAReceber.AtualizaBanco = 9; 
                            else
                                if (bancoLiquidacaoOperacao != null) newTituloContasAReceber.AtualizaBanco = (int)bancoLiquidacaoOperacao;
                            
                            newTituloContasAReceber.Saldo = 0;

                            //ATUALIZA MORA
                            newTituloContasAReceber.JurosMoraCalculado = 0;

                            _tituloContasAReceberService.Atualizar(newTituloContasAReceber);



                            //CALCULA RATEIO E GERA TITULO MORA
                            if (newTituloContasAReceber.DataVencimento < request.DataLiquidacao)
                            {
                                var valorMoraNaoLiquidada = _tituloContasAReceberService.CalculoRateio(newTituloContasAReceber.LiquidacaoDespesas, newTituloContasAReceber.LiquidacaoJuros, newTituloContasAReceber.LiquidacaoDesconto,
                                    newTituloContasAReceber.EmpresaCodigo, newTituloContasAReceber.DocumentoTipoCodigo, newTituloContasAReceber.DocumentoSerie, newTituloContasAReceber.DocumentoNumero, 
                                    int.Parse(newTituloContasAReceber.DocumentoSequencia), newTituloContasAReceber.BancoCodigoOficial, newTituloContasAReceber.BancoNumeroAgencia , (int)newTituloContasAReceber.BancoNumeroConta, newTituloContasAReceber.BancoDvConta,
                                     newTituloContasAReceber.Valor, newTituloContasAReceber.DataVencimento);

                                if (valorMoraNaoLiquidada > 0)
                                {
                                    _tituloContasAReceberService.GeraMoraNaoLiquidada(newTituloContasAReceber.EmpresaCodigo, newTituloContasAReceber.DocumentoTipoCodigo, newTituloContasAReceber.DocumentoSerie,
                                        (int)newTituloContasAReceber.DocumentoNumero,int.Parse(newTituloContasAReceber.DocumentoSequencia), newTituloContasAReceber.Desdobramento, valorMoraNaoLiquidada, (DateTime)request.DataLiquidacao, (long)newTituloContasAReceber.IntervenienteCodigo );
                                }
                            }

                            //CRIA VINCULO MOVIMENTO TITULO(Não irá criar caso seja um título de adiantamento)
                            if (tituloContasAReceber.Valor>0)
                                _tituloContasAReceberService.CriaEAtualizaVinculoMovimentoTitulo(newTituloContasAReceber.EmpresaCodigo, newTituloContasAReceber.DocumentoTipoCodigo, newTituloContasAReceber.DocumentoSerie, int.Parse(newTituloContasAReceber.DocumentoSequencia), newTituloContasAReceber.Desdobramento, newTituloContasAReceber.DocumentoNumero, newTituloContasAReceber.LiquidacaoValorRecebido, newTituloContasAReceber.IntervenienteCodigo, idMovimentoBancario);
                        }
                        else if (tipoLiquidacao == "PARCIAL")
                        {
                            TituloContasAReceber tituloLiquidacao = new TituloContasAReceber();
                            tituloLiquidacao = AutoMapper.Mapper.Map(newTituloContasAReceber, tituloLiquidacao);

                            //CRIA MOVIMENTO BANCARIO(Não irá criar caso seja um título de adiantamento)
                            long idMovimentoBancario = 0;
                            var parametrosMovimento =
                                _tituloContasAReceberService.RetornaParametrosMovimento(operacaoMovimentoBancario);
                            
                            if (tituloContasAReceber.Valor>0)
                            {
                                var movimentoBancarioLiquidacao = new MovimentoBanco(tituloLiquidacao,
                                    TpMovimentotpBaixa.Item1, operacaoMovimentoBancario, parametrosMovimento.Item1,
                                    "API ACCOUNTS RECEIVABLE FULL LIQ", "API ACCOUNTS RECEIVABLE");
                                 idMovimentoBancario =
                                    _tituloContasAReceberService.AdicionarMovimento(movimentoBancarioLiquidacao);
                            }
                            //LIQUIDA TITULO

                            var bancoLiquidacaoOperacao = _tituloContasAReceberService.RetornaBancoDeLiquidacao(tituloLiquidacao.OperacaoLiquidacao);
                            var desdobramento = _tituloContasAReceberService.RetornaDesdobramentoMaximo(newTituloContasAReceber.EmpresaCodigo, newTituloContasAReceber.DocumentoTipoCodigo, newTituloContasAReceber.DocumentoSerie, newTituloContasAReceber.DocumentoNumero, (newTituloContasAReceber).DocumentoSequencia.ToString());
                            var valorMoraLiquidada = tituloLiquidacao.LiquidacaoDespesas + tituloLiquidacao.LiquidacaoJuros - tituloLiquidacao.LiquidacaoDesconto;
                            if (tituloContasAReceber.Valor>0)
                                tituloLiquidacao.IdMovimentoBanco = idMovimentoBancario;
                           
                            tituloLiquidacao.LiquidacaoLoteBaixa = parametrosMovimento.Item2;
                            tituloLiquidacao.Desdobramento = (int)desdobramento;
                            tituloLiquidacao.ValorMoraNaoLiquidado = 0;
                            tituloLiquidacao.MultaMoraCalculado = 0;
                            tituloLiquidacao.DescontoMora = 0;
                            tituloLiquidacao.IdLiquidacao = "API AR -" + DateTime.Now.ToString("yyyy-MM-dd");
                            if (tituloContasAReceber.DocumentoTipoCodigo == 17 && tituloContasAReceber.Valor < 0)
                                tituloLiquidacao.AtualizaBanco = 9; 
                            else
                                tituloLiquidacao.AtualizaBanco = (int)bancoLiquidacaoOperacao; 

                            tituloLiquidacao.Valor = 0;
                            tituloLiquidacao.ValorBruto = 0;
                            tituloLiquidacao.Saldo = 0;
                            tituloLiquidacao.SomaRecebimentos = 0;
                            tituloLiquidacao.ValorRetencoes = 0;
                            tituloLiquidacao.Situacao = 0;
                            tituloLiquidacao.DataSituacao = null;
                            tituloLiquidacao.Observacao = "";
                            tituloLiquidacao.LiquidadoEmCheque = "N";
                            tituloLiquidacao.BancoPortador = 0;       
                            tituloLiquidacao.DataLiquidacaoCliente = request.DataLiquidacao;
                            tituloLiquidacao.DataLiquidacao = newTituloContasAReceber.DataLiquidacao;

                            _tituloContasAReceberService.Adicionar(tituloLiquidacao);

                            //ATUALIZA MORA E CORRIGE CAMPOS DO TÍTULO PRINCIPAL

                            newTituloContasAReceber.SomaRecebimentos += tituloLiquidacao.LiquidacaoValorRecebido - valorMoraLiquidada;
                            newTituloContasAReceber.MultaMoraCalculado = 0;
                            newTituloContasAReceber.JurosMoraCalculado = 0;
                            newTituloContasAReceber.LiquidacaoValorRecebido = 0;
                            newTituloContasAReceber.DataLiquidacao = null;
                            newTituloContasAReceber.DataLiquidacaoCliente = null;
                            newTituloContasAReceber.OperacaoLiquidacao = 0;
                            newTituloContasAReceber.BancoLiquidacao = 0;

                            _tituloContasAReceberService.Atualizar(newTituloContasAReceber);


                            //CALCULA RATEIO E GERA TITULO MORA
                            if (tituloLiquidacao.DataVencimento < request.DataLiquidacao)
                            {
                                var valorMoraNaoLiquidada = _tituloContasAReceberService.CalculoRateio(newTituloContasAReceber.LiquidacaoDespesas, newTituloContasAReceber.LiquidacaoJuros, newTituloContasAReceber.LiquidacaoDesconto,
                                    newTituloContasAReceber.EmpresaCodigo, newTituloContasAReceber.DocumentoTipoCodigo, newTituloContasAReceber.DocumentoSerie, newTituloContasAReceber.DocumentoNumero, 
                                    int.Parse(newTituloContasAReceber.DocumentoSequencia), newTituloContasAReceber.BancoCodigoOficial, newTituloContasAReceber.BancoNumeroAgencia , newTituloContasAReceber.BancoNumeroConta, newTituloContasAReceber.BancoDvConta,
                                    newTituloContasAReceber.Valor, newTituloContasAReceber.DataVencimento);

                                if (valorMoraNaoLiquidada > 0)
                                {
                                    _tituloContasAReceberService.GeraMoraNaoLiquidada(newTituloContasAReceber.EmpresaCodigo, newTituloContasAReceber.DocumentoTipoCodigo, newTituloContasAReceber.DocumentoSerie,
                                        (int)newTituloContasAReceber.DocumentoNumero, int.Parse(newTituloContasAReceber.DocumentoSequencia), newTituloContasAReceber.Desdobramento, valorMoraNaoLiquidada, (DateTime)request.DataLiquidacao, (long)request.IntervenienteCodigo);
                                }
                            }

                            //CRIA VINCULO MOVIMENTO TITULO(Não irá criar caso seja um título de adiantamento)
                            if (tituloContasAReceber.Valor>0)
                                _tituloContasAReceberService.CriaEAtualizaVinculoMovimentoTitulo(tituloLiquidacao.EmpresaCodigo, tituloLiquidacao.DocumentoTipoCodigo, tituloLiquidacao.DocumentoSerie, int.Parse(tituloLiquidacao.DocumentoSequencia), tituloLiquidacao.Desdobramento, tituloLiquidacao.DocumentoNumero, (float)tituloLiquidacao.LiquidacaoValorRecebido, tituloLiquidacao.IntervenienteCodigo, idMovimentoBancario);

                        }
                        else throw new Exception();
                    }
                    Commit();
                    scope.Complete();

                    return new ResultDTO<PublicoTituloContasAReceberResponse>(EResultDTOStatus.Success, "Sucessfully update", AutoMapper.Mapper.Map(tituloContasAReceber, newRetornoResponse));
                }
                catch
                {
                    scope.Dispose();
                    var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                    var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                    return new ResultDTO<PublicoTituloContasAReceberResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
                }
            }
        }
    }
}
