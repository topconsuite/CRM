using DocsBr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using Topsys.TopConWeb.SharedKernel.Common;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;
using TopSys.TopConWeb.Domain.Constants;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Helpers;
using TopSys.TopConWeb.Domain.Interfaces.Integrations;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Domain.Scopes;
using TopSys.TopConWeb.SharedKernel;

namespace TopSys.TopConWeb.Domain.Services
{
    public class TituloContasAReceberService : ServiceBase<TituloContasAReceber>, ITituloContasAReceberService
    {
        private readonly IParametroFinanceiroRepository _parametroFinanceiroRepository;
        private readonly IParametroRepository _parametroRepository;
        private readonly ITituloContasAReceberRepository _tituloContasAReceberRepository;
        private readonly ICartaoTransacaoRepository _cartaoTransacaoRepository;
        private readonly ITituloContasAPagarRepository _tituloContasAPagarRepository;
        private readonly IMovimentoBancoRepository _movimentoBancoRepository;
        private readonly IBancoRepository _bancoRepository;
        private readonly ITipoDeCobrancaRepository _tipoDeCobrancaRepository;
        private readonly IHeaderProvider _headerProvider;
        private readonly IFaturaRepository _faturaRepository;

        public TituloContasAReceberService(ITituloContasAReceberRepository tituloContasAReceberRepository,
                                           ITituloContasAPagarRepository tituloContasAPagarRepository,
                                           IParametroRepository parametroRepository,
                                           IMovimentoBancoRepository movimentoBancoRepository,
                                           ITipoDeCobrancaRepository tipoDeCobrancaRepository,
                                           IHeaderProvider headerProvider,
                                           ICartaoTransacaoRepository cartaoTransacaoRepository,
                                           IParametroFinanceiroRepository parametroFinanceiroRepository,
                                           IFaturaRepository faturaRepository)
            : base(tituloContasAReceberRepository)
        {
            _tituloContasAReceberRepository = tituloContasAReceberRepository;
            _tituloContasAPagarRepository = tituloContasAPagarRepository;
            _parametroRepository = parametroRepository;
            _movimentoBancoRepository = movimentoBancoRepository;
            _tipoDeCobrancaRepository = tipoDeCobrancaRepository;
            _headerProvider = headerProvider;
            _cartaoTransacaoRepository = cartaoTransacaoRepository;
            _parametroFinanceiroRepository = parametroFinanceiroRepository;
            _faturaRepository = faturaRepository;
        }

        public IEnumerable<TituloContasAReceber> ListarPorNumeroCartaoAutorizacao(int numeroCartao, string autorizacao)
        {
            return _tituloContasAReceberRepository.ListarPorNumeroCartaoAutorizacao(numeroCartao, autorizacao);
        }

        public IEnumerable<TituloContasAReceber> ListarPorNumeroCartaoAutorizacaoDuplicado(int idUsina, int contratoAno, int contratoNumero, int numeroCartao, string autorizacao)
        {
            return _tituloContasAReceberRepository.ListarPorNumeroCartaoAutorizacaoDuplicado(idUsina, contratoAno, contratoNumero, numeroCartao, autorizacao);
        }

        // FOR PUBLIC INTEGRATIONS

        public TituloContasAReceber ObterPorParametros(int empresa, int tipoDocumento, string serieDocumento, int numeroDocumento, int sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv, int desdobramento)
        {
            return _tituloContasAReceberRepository.ObterPorParametros(empresa, tipoDocumento, serieDocumento, numeroDocumento, sequencia, codBancoBand, numAgencia,  numConta, numContaDv, desdobramento);
        }

        public ICollection<TituloContasAReceber> ObterPorParametros(int empresa, int tipoDocumento, string serieDocumento, int numeroDocumento,
            int codBancoBand, int numAgencia, int numConta, int numContaDv)
        {
            return _tituloContasAReceberRepository.ObterPorParametros(empresa, tipoDocumento, serieDocumento, numeroDocumento, codBancoBand, numAgencia,  numConta, numContaDv);

        }

        public TituloContasAReceber ObterPorOriginalIdReguaDeCobranca(string id)
        {
            var (empresa, tipoDocumento, serieDocumento, numeroDocumento, sequenciaDocumento, bandeiraCodigo, agenciaNumero, contaNumero, contaDigito, desdobramento) = TituloContasAReceberHelper.ObtemChaveDoOriginalIdReguaDeCobranca(id);

            return _tituloContasAReceberRepository.ObterPorParametros(empresa, tipoDocumento, serieDocumento, numeroDocumento, sequenciaDocumento, bandeiraCodigo, agenciaNumero, contaNumero, contaDigito, desdobramento, dapper: true);
        }

        public ICollection<TituloContasAReceber> Listar(DateTime? dataEmissao, DateTime? dataOperacao, int tipoDocumento, int? centroCusto, string serieDocumento, long? numeroDocumento, int cliente, int pagina = 0, int limite = 0)
        {
            return _tituloContasAReceberRepository.ListarComPaginacao(dataEmissao, dataOperacao, tipoDocumento, centroCusto, serieDocumento, numeroDocumento, cliente, pagina, limite);
        }

        public PagedList<TituloContasAReceber> ObterPorDataAtualizacao(DateTime dataInicio, DateTime? dataFim, int page, int limit)
        {
            return _tituloContasAReceberRepository.ObterPorDataAtualizacao(dataInicio, dataFim, page, limit);
        }

        public List<string[]> ValidaCamposRequestAdicionarTituloContasAReceber(int? empresa, int? documentoTipo, int? cliente, int? operacaoCodigo, int? centroCusto, int? situacao, int? BancoPortador, int? operacaoLiquidacao, int? BancoLiquidacao, DateTime? dataLiquidacao)
        {

            var errors = new List<string[]>();
            var idioma = _headerProvider.GetAcceptLanguage();

            if (!_tituloContasAReceberRepository.VerificaSeExisteEmTabelasRelacionadas(empresa.ToString(), "emp", "ger_empresa"))
            {
                errors.Add(new string[] {
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(idioma, "company"),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode()
                });
            }

            if (!_tituloContasAReceberRepository.VerificaSeExisteEmTabelasRelacionadas(documentoTipo.ToString(), "cod", "ger_tp_doc"))
            {
                errors.Add(new string[] {
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(idioma, "document_type"),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode()
                });
            }

            if (!_tituloContasAReceberRepository.VerificaSeExisteEmTabelasRelacionadas(cliente.ToString(), "cod", "ger_interv"))
            {
                errors.Add(new string[] {
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(idioma, "client"),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode()
                });
            }

            if (!_tituloContasAReceberRepository.VerificaSeExisteEmTabelasRelacionadas(operacaoCodigo.ToString(), "cod", "fin_operacao"))
            {
                errors.Add(new string[] {
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(idioma, "operation_code"),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode()
                });
            }

            if (!_tituloContasAReceberRepository.VerificaSeExisteEmTabelasRelacionadas(centroCusto.ToString(), "cod", "fin_ccusto"))
            {
                errors.Add(new string[] {
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(idioma, "cost_center"),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode()
                });
            }
            
            if (situacao != null && situacao != 0 && !_tituloContasAReceberRepository.VerificaSeExisteEmTabelasRelacionadas(situacao.ToString(), "cod", "fin_situacao"))
            {
                errors.Add(new string[] {
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(idioma, "situation"),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode()
                });
            }

            if (BancoPortador > 0 && !_tituloContasAReceberRepository.VerificaSeExisteEmTabelasRelacionadas(BancoPortador.ToString(), "cod", "fin_portador"))
            {
                errors.Add(new string[] {
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(idioma, "bill_collector"),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode()
                });

                if (dataLiquidacao != null)
                {
                    if (!_tituloContasAReceberRepository.VerificaSeExisteEmTabelasRelacionadas(operacaoLiquidacao.ToString(), "cod", "fin_operacao") || operacaoLiquidacao == null)
                    {
                        errors.Add(new string[] {
                            EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(idioma, "operation_liquidate"),
                            EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode()
                        });
                    }

                    if (!_tituloContasAReceberRepository.VerificaSeExisteEmTabelasRelacionadas(BancoLiquidacao.ToString(), "cod", "fin_portador"))
                    {
                        errors.Add(new string[] {
                            EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(idioma, "bank_liquidation"),
                            EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode()
                        });
                    }
                }

                if (dataLiquidacao == null)
                {
                    if (BancoLiquidacao != null || operacaoLiquidacao != null)
                    {
                        errors.Add(new string[] {
                            EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f32.GetResourceMessage(idioma),
                            EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f32.GetMessageCode()
                        });
                    }
                }
            }
            return errors;
        }

        public List<string[]> ValidaCamposRequestAtualizarTituloContasAReceber(int? empresa, int? documentoTipo, int? cliente, int? operacaoCodigo, int? centroCusto, int? situacao, int? BancoPortador, int? operacaoLiquidacao, int? BancoLiquidacao, DateTime? dataLiquidacao)
        {

            var errors = new List<string[]>();
            var idioma = _headerProvider.GetAcceptLanguage();

            if (empresa !=null && !_tituloContasAReceberRepository.VerificaSeExisteEmTabelasRelacionadas(empresa.ToString(), "emp", "ger_empresa"))
            {
                errors.Add(new string[] {
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(idioma, "company"),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode()
                });
            }

            if (documentoTipo != null && !_tituloContasAReceberRepository.VerificaSeExisteEmTabelasRelacionadas(documentoTipo.ToString(), "cod", "ger_tp_doc"))
            {
                errors.Add(new string[] {
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(idioma, "document_type"),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode()
                });
            }

            if (cliente != null && !_tituloContasAReceberRepository.VerificaSeExisteEmTabelasRelacionadas(cliente.ToString(), "cod", "ger_interv"))
            {
                errors.Add(new string[] {
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(idioma, "client"),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode()
                });
            }

            if (operacaoCodigo != null && !_tituloContasAReceberRepository.VerificaSeExisteEmTabelasRelacionadas(operacaoCodigo.ToString(), "cod", "fin_operacao"))
            {
                errors.Add(new string[] {
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(idioma, "operation_code"),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode()
                });
            }

            if (centroCusto != null && !_tituloContasAReceberRepository.VerificaSeExisteEmTabelasRelacionadas(centroCusto.ToString(), "cod", "fin_ccusto"))
            {
                errors.Add(new string[] {
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(idioma, "cost_center"),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode()
                });
            }

            if (situacao != null && situacao != 0 && !_tituloContasAReceberRepository.VerificaSeExisteEmTabelasRelacionadas(situacao.ToString(), "cod", "fin_situacao"))
            {
                errors.Add(new string[] {
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(idioma, "situation"),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode()
                });
            }

            if (BancoPortador > 0 && !_tituloContasAReceberRepository.VerificaSeExisteEmTabelasRelacionadas(BancoPortador.ToString(), "cod", "fin_portador"))
            {
                errors.Add(new string[] {
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(idioma, "bill_collector"),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode()
                });
            }

            if (dataLiquidacao != null)
            {
                if (!_tituloContasAReceberRepository.VerificaSeExisteEmTabelasRelacionadas(operacaoLiquidacao.ToString(), "cod", "fin_operacao") || operacaoLiquidacao == null)
                {
                    errors.Add(new string[] {
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(idioma, "operation_liquidate"),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode()
                    });
                }

                if (!_tituloContasAReceberRepository.VerificaSeExisteEmTabelasRelacionadas(BancoLiquidacao.ToString(), "cod", "fin_portador"))
                {
                    errors.Add(new string[] {
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(idioma, "bank_liquidation"),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode()
                    });
                }
            }

            if (dataLiquidacao == null)
            {
                if (BancoLiquidacao != null || operacaoLiquidacao != null)
                {
                    errors.Add(new string[] {
                        EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f32.GetResourceMessage(idioma),
                        EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f32.GetMessageCode()
                    });
                }
            }

            return errors;
        }

        public List<string[]> ValidaSaldoAdicionarRequest(float? valor, float? recebimentos)
        {
            var errors = new List<string[]>();
            var idioma = _headerProvider.GetAcceptLanguage();

            if (valor > 0 && valor < recebimentos)
            {
                errors.Add(new string[] { EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f33.GetResourceMessage(idioma),
                    EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f33.GetMessageCode()});
            }

            if (valor < 0 && valor > recebimentos)
            {
                errors.Add(new string[] { EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f34.GetResourceMessage(idioma),
                    EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f34.GetMessageCode()});
            }

            return errors;
        }

        public List<string[]> ValidaSaldoAtualizarRequest(float? valor, float? recebimentos, float? valorOriginal, float? recebimentosOriginal)
        {
            var errors = new List<string[]>();
            var idioma = _headerProvider.GetAcceptLanguage();

            if (valor == null && recebimentos !=null ) valor = valorOriginal;
            if (recebimentos == null && valor != null) recebimentos = recebimentosOriginal;
            if (valor == null && recebimentos == null) return errors;

            if (valor != valorOriginal && recebimentos != recebimentosOriginal)
            {
                if (valor > 0 && valor < recebimentos)
                {
                    errors.Add(new string[] { EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f33.GetResourceMessage(idioma),
                        EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f33.GetMessageCode()});
                }

                if (valor < 0 && valor > recebimentos)
                {
                    errors.Add(new string[] { EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f34.GetResourceMessage(idioma),
                        EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f34.GetMessageCode() });
                }
            }

            if (valor != valorOriginal && recebimentos == recebimentosOriginal)
            {
                if (valor > 0 && valor < recebimentosOriginal)
                {
                    errors.Add(new string[] { EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f33.GetResourceMessage(idioma),
                        EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f33.GetMessageCode()});
                }

                if (valor < 0 && valor > recebimentosOriginal)
                {
                    errors.Add(new string[] { EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f34.GetResourceMessage(idioma),
                        EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f34.GetMessageCode() });
                }
            }

            if (valor == valorOriginal && recebimentos != recebimentosOriginal)
            {
                if (valorOriginal > 0 && valorOriginal < recebimentos)
                {
                    errors.Add(new string[] { EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f33.GetResourceMessage(idioma),
                        EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f33.GetMessageCode()});
                }

                if (valorOriginal < 0 && valorOriginal > recebimentos)
                {
                    errors.Add(new string[] { EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f34.GetResourceMessage(idioma),
                        EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f34.GetMessageCode() });
                }
            }

            return errors;
        }

        public int? RetornaOperacaoMovimentoBancario(int? operacaoLiquidacao)
        {
            var validaOperacao = _tituloContasAReceberRepository.ValidaOperacaoMovimentoBancario(operacaoLiquidacao);

            if (validaOperacao > 0)
                return validaOperacao;
            
            return null;
            
        }
        
        public int? RetornaOperacaoBaixa(int? operacaoBaixa)
        {
            var validaOperacao = _tituloContasAReceberRepository.ValidaOperacaoBaixa(operacaoBaixa);

            if (validaOperacao > 0)
                return validaOperacao;
            
            return null;
            
        }

        public List<string[]> ValidaBancoLiquidacao(int? empresa, int? operacaoLiquidacao, int? bancoLiquidacao)
        {
            var errors = new List<string[]>();

            var result = _tituloContasAReceberRepository.ValidaBancoLiquidacao(empresa, operacaoLiquidacao, bancoLiquidacao);

            if (result != true)
            {
                errors.Add(new string[]
                {
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(
                        _headerProvider.GetAcceptLanguage(), "bank_liquidation"),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode()
                });
            }

            return errors;
        }

        public string ObterLinkSegundaViaBoleto(int codigoBanco)
        {
            return _tituloContasAReceberRepository.ObterLinkSegundaViaBoleto(codigoBanco);
        }

        public List<string[]> ValidaValoresLiquidacao(float? valor, float? liquidacaoValorRecebido, float? liquidacaoJuros, float? liquidacaoDesconto, float? liquidacaoDespesas)
        {

            var errors = new List<string[]>();
            var idioma = _headerProvider.GetAcceptLanguage();
            
            if (Math.Abs(liquidacaoValorRecebido??0) > Math.Abs(valor??0) )
            {
                errors.Add(new string[]
                {
                    EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f31.GetResourceMessage(idioma,
                        "amount_received_liquidation"),
                    EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f31.GetMessageCode()
                });
            }

            if (Math.Abs(liquidacaoValorRecebido + liquidacaoDesconto??0) > Math.Abs(valor??0))
            {
                errors.Add(new string[]
                {
                    EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f31.GetResourceMessage(idioma,
                        "discount_liquidation"),
                    EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f31.GetMessageCode()
                });

            }

            if (liquidacaoValorRecebido == 0)
            {
                errors.Add(new string[]
                {
                    EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f31.GetResourceMessage(idioma,
                        "amount_received_liquidation"),
                    EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f31.GetMessageCode()
                });
            }
            
            if ((liquidacaoValorRecebido > 0 && valor<0)||(liquidacaoValorRecebido < 0 && valor>0))
            {
                errors.Add(new string[]
                {
                    EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f31.GetResourceMessage(idioma,
                        "amount_received_liquidation"),
                    EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f31.GetMessageCode()
                });
            }

            return errors;
        }

        public List<string[]> ValidaValoresRecebimento(float? somaRecebimentos, float? valor, float? liquidacaoTotalRecebido)
        {

            var errors = new List<string[]>();

            if (Math.Abs(somaRecebimentos + liquidacaoTotalRecebido??0) > Math.Abs(valor??0))
            {
                errors.Add(new string[] { EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f35.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f35.GetMessageCode()});            }
            
            return errors;
        }

        public Tuple<int, int> RetornaTipoMovimentoEBaixa(int empresa)
        {
            var result = _tituloContasAReceberRepository.RetornaTipoMovimentoEBaixa(empresa);
     
            return Tuple.Create(int.Parse(result.Item1.ToString()), int.Parse(result.Item2.ToString()));
        }

        public string DefineTipoLiquidacao(float? valor, float? valorRecebidoLiquidacao, float? somaRecebimentos)
        {
            if (Math.Abs(valorRecebidoLiquidacao + somaRecebimentos??0) == Math.Abs(valor??0)) return "PARCIAL TOTAL";
            if (Math.Abs(valorRecebidoLiquidacao??0) < Math.Abs(valor??0)) return "PARCIAL";
            else if (valorRecebidoLiquidacao == valor) return "TOTAL";
            else return "";

        }

        public void CriaEAtualizaVinculoMovimentoTitulo(int empresaCodigo, int documentoTipoCodigo, string documentoSerie, int? documentoSequencia, int desdobramento, long documentoNumero, float valor, long interveniente, long idMovimentoBancario)
        {
            _tituloContasAReceberRepository.CriaEAtualizaVinculoMovimentoTitulo(empresaCodigo, documentoTipoCodigo, documentoSerie, documentoSequencia, desdobramento, documentoNumero, valor, interveniente, idMovimentoBancario);
        }

        public float CalculoRateio(float liquidacaoDespesas, float liquidacaoJuros, float liquidacaoDesconto, int empresaCodigo, int documentoTipoCodigo, string documentoSerie, long documentoNumero, int documentoSequencia, int codBancoBand, int numAgencia, long numConta, byte numContaDv, float valor, DateTime? dataVencimento)
        {
            return _tituloContasAReceberRepository.CalculoRateio(liquidacaoDespesas, liquidacaoJuros, liquidacaoDesconto, empresaCodigo, documentoTipoCodigo,
                documentoSerie, documentoNumero, documentoSequencia, codBancoBand, numAgencia, numConta, numContaDv, valor, dataVencimento);
        }

        public void GeraMoraNaoLiquidada(int empresaCodigo, int documentoTipoCodigo, string documentoSerie, int numeroDocumento, int? documentoSequencia, int desdobramento, float valorMoraNaoLiquidada, DateTime dataEmissao, long interveniente)
        {
            _tituloContasAReceberRepository.GeraMoraNaoLiquidada(empresaCodigo, documentoTipoCodigo, documentoSerie, numeroDocumento, documentoSequencia, desdobramento, valorMoraNaoLiquidada, dataEmissao, interveniente);
        }

        public Tuple<long, long> RetornaParametrosMovimento(int? operacaoMovimentoBancario)
        {
            return _tituloContasAReceberRepository.RetornaParametrosDeMovimentoBancario(operacaoMovimentoBancario);
        }

        public int? RetornaBancoDeLiquidacao(int? operacaoLiquidacao)
        {
            return _tituloContasAReceberRepository.RetornaBancoDeLiquidacao(operacaoLiquidacao);
        }

        public int? RetornaDesdobramentoMaximo(int? empresaCodigo, int? documentoTipoCodigo, string documentoSerie, long? documentoNumero, string documentoSequencia)
        {
            return _tituloContasAReceberRepository.RetornaDesdobramentoMaximo(empresaCodigo, documentoTipoCodigo, documentoSerie, documentoNumero, documentoSequencia);
        }

        public long AdicionarMovimento(MovimentoBanco movimentoBanco)
        {
            return _movimentoBancoRepository.AdicionarERetornaId(movimentoBanco);
        }

        public TipoDeCobranca RetornaTipoDeCobrancaComDescricao(int codTipoCobranca)
        {
            return _tipoDeCobrancaRepository.ObterTipoDeCobrancaComDescricao(codTipoCobranca);
        }

        public bool DentroDoMesFechamento(int empresa, DateTime? dataLiquidacao)
        {
            var mesAberto = _parametroFinanceiroRepository.ObterMesAbertoPorEmpresa(empresa);

            return mesAberto >= dataLiquidacao && dataLiquidacao != null;
        }

        public bool CancelamentoParcialDeRecebimentoEmCartao(long lote)
        {
            if (_tituloContasAReceberRepository.ExisteTituloRecebimentoEmCartao(lote))
            {
                return _tituloContasAReceberRepository.QuantidadeTitulosDeCredito(lote) > 1;
            }
                
            return false;
        }

        public bool CancelamentoParcialDeRecebimentoEmCheque(long lote)
        {
            if (_tituloContasAReceberRepository.ExisteTituloRecebimentoEmCheque(lote))
            {
                return _tituloContasAReceberRepository.QuantidadeTitulosDeCredito(lote) > 1;
            }

            return false;
        }

        public bool ExisteTituloDevolucaoEmCheque(int tipoDocumento, string serieDocumento, int numeroDocumento, int sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv, int desdobramento)
        {
            return _tituloContasAReceberRepository.ExisteTituloDevolucaoEmCheque(tipoDocumento, serieDocumento, numeroDocumento, sequencia, codBancoBand, numAgencia, numConta, numContaDv, desdobramento);
        }

        public bool ExisteLiquidacaoGeradoNoContasAPagar(long lote, int empresa, int clienteDocumento, string serieDocumento, int numeroDocumento, double liquidacaoValorRecebidoDocumento)
        {
            return _tituloContasAPagarRepository.ExisteTituloCreditoFornecedor(lote, empresa, clienteDocumento, serieDocumento, numeroDocumento, liquidacaoValorRecebidoDocumento);
        }

        public bool ExisteMovimentoDeBancoConciliado(int empresa, int tipoDocumento, string serieDocumento, int numeroDocumento, int sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv, int desdobramento, long idMovimentoBanco)
        {
            return _tituloContasAReceberRepository.ExisteMovimentoDeBancoConciliado(empresa, tipoDocumento, serieDocumento, numeroDocumento, sequencia, codBancoBand, numAgencia, numConta, numContaDv, desdobramento, idMovimentoBanco);
        }

        public bool ExisteBaixasDoContasAPagar(long lote)
        {
            return _tituloContasAPagarRepository.ExisteTitulo(lote);
        }

        public bool ExisteChequeLiquidadoRelacionadoABaixaDoLote(long lote)
        {
            return _tituloContasAReceberRepository.ExisteChequeLiquidadoRelacionadoABaixaDoLote(lote, Titulo.DESDOBRAMENTO_TITULO_PRINCIPAL);
        }

        public bool ExisteCreditoCompensadoNaGeracaoDaLiquidacao(long lote)
        {
            return _tituloContasAReceberRepository.ExisteCreditoCompensadoNaGeracaoDaLiquidacao(lote);
        }

        public void AjustaDescontoDeMora(long loteBaixa, int tipoDocumento, long clienteDocumento, int numeroDocumento, string serieDocumento)
        {
            _tituloContasAReceberRepository.AjustaDescontoDeMora(loteBaixa, tipoDocumento, clienteDocumento, numeroDocumento, serieDocumento);
        }

        public void RemovePendenciaCobranca(long loteBaixa)
        {
            _tituloContasAReceberRepository.RemovePendenciaDeCobranca(loteBaixa);
        }

        public void RemoveDesdobramentos(long loteBaixa, int sequencia, int tipoDocumento, long clienteDocumento, int numeroDocumento, string serieDocumento)
        {
            _tituloContasAReceberRepository.RemoveDesdobramentos(loteBaixa, sequencia, tipoDocumento, clienteDocumento, numeroDocumento, serieDocumento);
        }

        public void RemoveMovimentoDeBancoNaoConciliadoDeTituloDeCredito(int empresa, long loteBaixa, long clienteDocumento)
        {
            var titulosDeCredito = _tituloContasAReceberRepository.ObterTitulosDeCredito(empresa, loteBaixa, clienteDocumento);

            foreach (var tituloDeCredito in titulosDeCredito)
            {
                var movimentoBanco = _tituloContasAReceberRepository.ObterVinculosMovimentosDeBanco(tituloDeCredito.EmpresaCodigo,
                                                                                                      tituloDeCredito.DocumentoTipoCodigo,
                                                                                                      tituloDeCredito.DocumentoSerie,
                                                                                                      (int)tituloDeCredito.DocumentoNumero,
                                                                                                      int.Parse(tituloDeCredito.DocumentoSequencia),
                                                                                                      tituloDeCredito.BancoCodigoOficial,
                                                                                                      tituloDeCredito.BancoNumeroAgencia,
                                                                                                      (int)tituloDeCredito.BancoNumeroConta,
                                                                                                      tituloDeCredito.BancoDvConta,
                                                                                                      tituloDeCredito.Desdobramento);

                _tituloContasAReceberRepository.RemoveVinculoMovimentosDeBanco(movimentoBanco.idMovimento);
                _movimentoBancoRepository.RemoveVinculoMovimentosDeBanco(movimentoBanco.idMovimento);
            }
        }

        public void AjustaMovimentosComCartaoDeCredito(long loteBaixa)
        {
            var titulosComCartaoDeCredito = _tituloContasAReceberRepository.ObterTituloRecebimentoEmCartao(loteBaixa);

            foreach (var tituloComCartaoDeCredito in titulosComCartaoDeCredito)
            {

                var transacoesCartoes = _cartaoTransacaoRepository.ObterPorNumeroCartaoENumeroAutorizacaoEDataHoraTransacao(tituloComCartaoDeCredito.CartaoNumero,
                                                                                                                            tituloComCartaoDeCredito.CartaoAutorizacao,
                                                                                                                            (DateTime)tituloComCartaoDeCredito.DataEmissao);

                foreach (var transacaoCartao in transacoesCartoes)
                {

                    if (transacaoCartao.Origem == "manual")
                    {
                        _cartaoTransacaoRepository.RemoverPorId(transacaoCartao.Id, "API CR");
                    }
                    else
                    {
                        _tituloContasAReceberRepository.DesvinculaTituloCartaoDeCredito(transacaoCartao.CartaoNumero,
                                                                                        transacaoCartao.AutorizacaoNumero,
                                                                                        transacaoCartao.TransacaoDataHora);
                    }
                }
            }   
        }

        public void RemoveTitulo(long loteBaixa)
        {
            _tituloContasAReceberRepository.RemoveTitulo(loteBaixa);
        }

        public void RemoveTituloDeMora(int empresa, int tipoDocumento, string serieDocumento, int cliente, int numeroDocumento, int sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv)
        {
            _tituloContasAReceberRepository.RemoveTituloDeMora(empresa, tipoDocumento, serieDocumento, cliente, numeroDocumento, sequencia, codBancoBand, numAgencia, numConta, numContaDv);
        }

        public void RemoveTituloDeCredito(long loteBaixa, int empresa, long clienteDocumento)
        {
            _tituloContasAReceberRepository.RemoveTituloDeCredito(loteBaixa, empresa, clienteDocumento);
        }

        public void RemoveTituloDeCreditoContasAPagar(long loteBaixa, int empresa, int tipoDocumento, string serieDocumento, int clienteDocumento, int numeroDocumento, double valorRecebido)
        {
            _tituloContasAPagarRepository.RemoveTituloDeCredito(loteBaixa, empresa, clienteDocumento, serieDocumento, numeroDocumento, valorRecebido);
        }

        public void RemoveMovimentoDeBancoNaoConciliado(int idMovimentoDeBanco, int empresa, int tipoDocumento, string serieDocumento, int numeroDocumento, int sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv, int desdobramento)
        {
            var movimentoBanco = _tituloContasAReceberRepository.ObterVinculosMovimentosDeBanco(empresa, tipoDocumento, serieDocumento, numeroDocumento, sequencia, codBancoBand, numAgencia, numConta, numContaDv, desdobramento, idMovimentoDeBanco);

            var valorMovimento = _movimentoBancoRepository.ValorMovimentoDeBanco(movimentoBanco.idMovimento);

            if (valorMovimento != null )
            {
                if (valorMovimento <= movimentoBanco.valor)
                {
                    _movimentoBancoRepository.RemoverNaoConciliadoPorControle(idMovimentoDeBanco, "API CR");
                }
                else
                {
                    _movimentoBancoRepository.DescontaValorMovimentosDeBancoNaoConciliado(movimentoBanco.Item2, idMovimentoDeBanco);
                }

                _tituloContasAReceberRepository.RemoveVinculoMovimentosDeBanco(idMovimentoDeBanco, empresa, tipoDocumento, serieDocumento, numeroDocumento, sequencia, codBancoBand, numAgencia, numConta, numContaDv, desdobramento);
                
                _tituloContasAReceberRepository.RemoveVinculoIdMovimentosDeBanco(empresa, tipoDocumento, serieDocumento, numeroDocumento, sequencia, codBancoBand, numAgencia, numConta, numContaDv, desdobramento);
            }
        }

        public void AjustaSaldoDoTituloPrincipal(int empresa, int tipoDocumento, string serieDocumento, int clienteDocumento, int numeroDocumento, string sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv)
        {
            var dataMaximaDesdobramentos =_tituloContasAReceberRepository.ObterDataMaximaDesdobramentos(empresa, tipoDocumento, serieDocumento, clienteDocumento, numeroDocumento, sequencia, codBancoBand, numAgencia, numConta, numContaDv);
            
            var somatorioRecebimentos = _tituloContasAReceberRepository.ObterSomatorioRecebimentos(empresa, tipoDocumento, serieDocumento, clienteDocumento, numeroDocumento, sequencia, codBancoBand, numAgencia, numConta, numContaDv);
                
            _tituloContasAReceberRepository.AtualizarRecebimentosTituloPrincipal(somatorioRecebimentos, dataMaximaDesdobramentos, empresa, tipoDocumento, serieDocumento, clienteDocumento, numeroDocumento, sequencia, codBancoBand, numAgencia, numConta, numContaDv);

        }

        public void AjustaDescontoDeMoraDoTituloPrincipal(int empresa, int tipoDocumento, string serieDocumento, int clienteDocumento, int numeroDocumento, string sequencia, int codBancoBand, int numAgencia, int numConta, int numContaDv)
        {
            var ObterValorOcorrenciaDeDescontoMora = _tituloContasAReceberRepository.ObterValorOcorrenciaDeDescontoMora(empresa, tipoDocumento, serieDocumento, clienteDocumento, numeroDocumento, sequencia, codBancoBand, numAgencia, numConta, numContaDv);

            _tituloContasAReceberRepository.AtualizarDescontoMora(ObterValorOcorrenciaDeDescontoMora, empresa, tipoDocumento, serieDocumento, clienteDocumento, numeroDocumento, sequencia, codBancoBand, numAgencia, numConta, numContaDv);
        }

        public List<string[]> ValidaCancelamentoDeTitulo(TituloContasAReceber tituloContasAReceber)
        {
            var errors = new List<string[]>();
            var idioma = _headerProvider.GetAcceptLanguage();

            var tituloPrincipal = _tituloContasAReceberRepository.ObterPorParametros(tituloContasAReceber.EmpresaCodigo,
                                             tituloContasAReceber.DocumentoTipoCodigo,
                                             tituloContasAReceber.DocumentoSerie,
                                             (int)tituloContasAReceber.DocumentoNumero,
                                             int.Parse(tituloContasAReceber.DocumentoSequencia),
                                             tituloContasAReceber.BancoCodigoOficial,
                                             tituloContasAReceber.BancoNumeroAgencia,
                                             (int)tituloContasAReceber.BancoNumeroConta,
                                             tituloContasAReceber.BancoDvConta,
                                             Titulo.DESDOBRAMENTO_TITULO_PRINCIPAL);


            if (tituloContasAReceber.Desdobramento != Titulo.DESDOBRAMENTO_TITULO_PRINCIPAL && tituloPrincipal.EstaLiquidado)
            {
                errors.Add(new string[] {
                   EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f39.GetResourceMessage(idioma),
                   EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f39.GetMessageCode()});
            }

            return errors;

            // NOTE: As validações abaixos serão habilitadas posteriormente, caso necessário futuramente.

            if (CancelamentoParcialDeRecebimentoEmCartao(tituloContasAReceber.LiquidacaoLoteBaixa))
            {
                errors.Add(new string[] {
                   EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f40.GetResourceMessage(idioma),
                   EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f40.GetMessageCode()});

            }

            if (CancelamentoParcialDeRecebimentoEmCheque(tituloContasAReceber.LiquidacaoLoteBaixa))
            {
                errors.Add(new string[] {
                   EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f41.GetResourceMessage(idioma),
                   EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f41.GetMessageCode()});

            }

            if (tituloContasAReceber.Desdobramento != Titulo.DESDOBRAMENTO_TITULO_PRINCIPAL && 
                ExisteTituloDevolucaoEmCheque(tituloContasAReceber.DocumentoTipoCodigo,
                                              tituloContasAReceber.DocumentoSerie,
                                              (int)tituloContasAReceber.DocumentoNumero,
                                              int.Parse(tituloContasAReceber.DocumentoSequencia),
                                              tituloContasAReceber.BancoCodigoOficial,
                                              tituloContasAReceber.BancoNumeroAgencia,
                                              (int)tituloContasAReceber.BancoNumeroConta,
                                              tituloContasAReceber.BancoDvConta,
                                              tituloContasAReceber.Desdobramento))
            {
                errors.Add(new string[] {
                   EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f42.GetResourceMessage(idioma),
                   EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f42.GetMessageCode()});
            }

            if (ExisteLiquidacaoGeradoNoContasAPagar(tituloContasAReceber.LiquidacaoLoteBaixa,
                                                     tituloContasAReceber.EmpresaCodigo,
                                                     tituloContasAReceber.IntervenienteCodigo,
                                                     tituloContasAReceber.DocumentoSerie,
                                                     (int)tituloContasAReceber.DocumentoNumero,
                                                     tituloContasAReceber.LiquidacaoValorRecebido))
            {
                errors.Add(new string[] {
                   EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f43.GetResourceMessage(idioma),
                   EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f43.GetMessageCode()});
            }

            if (tituloContasAReceber.DocumentoTipoCodigo != (int)EDocumentoTipo.ContasAReceberCliente &&
                tituloContasAReceber.IdMovimentoBanco != 0 &&
                 ExisteMovimentoDeBancoConciliado(tituloContasAReceber.EmpresaCodigo,
                                                  tituloContasAReceber.DocumentoTipoCodigo,
                                                  tituloContasAReceber.DocumentoSerie,
                                                  (int)tituloContasAReceber.DocumentoNumero,
                                                  int.Parse(tituloContasAReceber.DocumentoSequencia),
                                                  tituloContasAReceber.BancoCodigoOficial,
                                                  tituloContasAReceber.BancoNumeroAgencia,
                                                  (int)tituloContasAReceber.BancoNumeroConta,
                                                  tituloContasAReceber.BancoDvConta,
                                                  tituloContasAReceber.Desdobramento,
                                                  tituloContasAReceber.IdMovimentoBanco)

                )
            {
                errors.Add(new string[] {
                   EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f44.GetResourceMessage(idioma),
                   EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f44.GetMessageCode()});
            }

            if (ExisteBaixasDoContasAPagar(tituloContasAReceber.LiquidacaoLoteBaixa))
            {
                errors.Add(new string[] {
                   EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f45.GetResourceMessage(idioma),
                   EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f45.GetMessageCode()});
            }

            if (ExisteChequeLiquidadoRelacionadoABaixaDoLote(tituloContasAReceber.LiquidacaoLoteBaixa))
            {
                errors.Add(new string[] {
                   EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f46.GetResourceMessage(idioma),
                   EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f46.GetMessageCode()});
            }

            if (ExisteCreditoCompensadoNaGeracaoDaLiquidacao(tituloContasAReceber.LiquidacaoLoteBaixa))
            {
                errors.Add(new string[] {
                   EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f47.GetResourceMessage(idioma),
                   EResourcesTituloContasAReceber.CONTAS_A_RECEBER_ERROR_TCON375f335f47.GetMessageCode()});
            }

            return errors;
        }

        public void CancelarRecebimentoDeTitulo(TituloContasAReceber tituloContasAReceber)
        {
            AjustaDescontoDeMora(tituloContasAReceber.LiquidacaoLoteBaixa,
                                 tituloContasAReceber.DocumentoTipoCodigo,
                                 tituloContasAReceber.IntervenienteCodigo,
                                 (int)tituloContasAReceber.DocumentoNumero,
                                 tituloContasAReceber.DocumentoSerie);

            RemovePendenciaCobranca(tituloContasAReceber.LiquidacaoLoteBaixa);

            RemoveDesdobramentos(tituloContasAReceber.LiquidacaoLoteBaixa,
                                 int.Parse(tituloContasAReceber.DocumentoSequencia),
                                 tituloContasAReceber.DocumentoTipoCodigo,
                                 tituloContasAReceber.IntervenienteCodigo,
                                 (int)tituloContasAReceber.DocumentoNumero,
                                 tituloContasAReceber.DocumentoSerie);


            RemoveMovimentoDeBancoNaoConciliadoDeTituloDeCredito(tituloContasAReceber.EmpresaCodigo,
                                                                 tituloContasAReceber.LiquidacaoLoteBaixa,
                                                                 tituloContasAReceber.IntervenienteCodigo);


            AjustaMovimentosComCartaoDeCredito(tituloContasAReceber.LiquidacaoLoteBaixa);

            RemoveTitulo(tituloContasAReceber.LiquidacaoLoteBaixa);

            RemoveTituloDeMora(tituloContasAReceber.EmpresaCodigo,
                               tituloContasAReceber.DocumentoTipoCodigo,
                               tituloContasAReceber.DocumentoSerie,
                               tituloContasAReceber.IntervenienteCodigo,
                               (int)tituloContasAReceber.DocumentoNumero,
                               int.Parse(tituloContasAReceber.DocumentoSequencia),
                               tituloContasAReceber.BancoCodigoOficial,
                               tituloContasAReceber.BancoNumeroAgencia,
                               (int)tituloContasAReceber.BancoNumeroConta,
                               tituloContasAReceber.BancoDvConta);
         
            RemoveTituloDeCredito(tituloContasAReceber.LiquidacaoLoteBaixa,
                                  tituloContasAReceber.EmpresaCodigo,
                                  tituloContasAReceber.IntervenienteCodigo);

           
            RemoveTituloDeCreditoContasAPagar(tituloContasAReceber.LiquidacaoLoteBaixa,
                                        tituloContasAReceber.EmpresaCodigo,
                                        tituloContasAReceber.DocumentoTipoCodigo,
                                        tituloContasAReceber.DocumentoSerie,
                                        tituloContasAReceber.IntervenienteCodigo,
                                        (int)tituloContasAReceber.DocumentoNumero,
                                        tituloContasAReceber.LiquidacaoValorRecebido);

            if (tituloContasAReceber.DocumentoTipoCodigo != (int)EDocumentoTipo.ContasAReceberCliente && 
                tituloContasAReceber.IdMovimentoBanco != 0)
            {
                RemoveMovimentoDeBancoNaoConciliado((int)tituloContasAReceber.IdMovimentoBanco,
                                                    tituloContasAReceber.EmpresaCodigo,
                                                    tituloContasAReceber.DocumentoTipoCodigo,
                                                    tituloContasAReceber.DocumentoSerie,
                                                    (int)tituloContasAReceber.DocumentoNumero,
                                                    int.Parse(tituloContasAReceber.DocumentoSequencia),
                                                    tituloContasAReceber.BancoCodigoOficial,
                                                    tituloContasAReceber.BancoNumeroAgencia,
                                                    (int)tituloContasAReceber.BancoNumeroConta,
                                                    tituloContasAReceber.BancoDvConta,
                                                    tituloContasAReceber.Desdobramento);     
            }

            AjustaSaldoDoTituloPrincipal(tituloContasAReceber.EmpresaCodigo,
                                         tituloContasAReceber.DocumentoTipoCodigo,
                                         tituloContasAReceber.DocumentoSerie,
                                         tituloContasAReceber.IntervenienteCodigo,
                                         (int)tituloContasAReceber.DocumentoNumero,
                                         tituloContasAReceber.DocumentoSequencia,
                                         tituloContasAReceber.BancoCodigoOficial,
                                         tituloContasAReceber.BancoNumeroAgencia,
                                         (int)tituloContasAReceber.BancoNumeroConta,
                                         tituloContasAReceber.BancoDvConta);


            if (tituloContasAReceber.Desdobramento != Titulo.DESDOBRAMENTO_TITULO_PRINCIPAL)
            {
                AjustaDescontoDeMoraDoTituloPrincipal(tituloContasAReceber.EmpresaCodigo,
                                                      tituloContasAReceber.DocumentoTipoCodigo,
                                                      tituloContasAReceber.DocumentoSerie,
                                                      tituloContasAReceber.IntervenienteCodigo,
                                                      (int)tituloContasAReceber.DocumentoNumero,
                                                      tituloContasAReceber.DocumentoSequencia,
                                                      tituloContasAReceber.BancoCodigoOficial,
                                                      tituloContasAReceber.BancoNumeroAgencia,
                                                      (int)tituloContasAReceber.BancoNumeroConta,
                                                      tituloContasAReceber.BancoDvConta);
            }


        }

        public Segmentacao RetornaSegmentacao(int empresa, int tipoDocumento, string serieDocumento, int clienteDocumento, long numeroDocumento)
        {
           var fatura = _faturaRepository.ObterPorChaveFatura(x => Math.Floor((double)(x.Filial/1000)) == empresa && x.Cliente == clienteDocumento
                && x.TipoDocumento == tipoDocumento && x.Numero == numeroDocumento && x.Serie == serieDocumento);
           
           return fatura == null ? new Segmentacao() : fatura.SegmentacaoContrato;
        }
    }
}
