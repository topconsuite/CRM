using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using Topsys.TopConWeb.SharedKernel.Common;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;
using TopSys.TopConWeb.Application.DTOS.Request.CondicaoPagamento;
using TopSys.TopConWeb.Application.DTOS.Request.CondicaoPagamento.Alteracao;
using TopSys.TopConWeb.Application.DTOS.Request.CondicaoPagamento.Inclusao;
using TopSys.TopConWeb.Application.DTOS.Response.CondicaoPagamento;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Integrations;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Domain.Scopes;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;

namespace TopSys.TopConWeb.Application
{
    public class CondicaoPagamentoApplicationService : ApplicationServiceBase<CondicaoPagamento>, ICondicaoPagamentoApplicationService
    {
        public readonly ICondicaoPagamentoService _condicaoPagamentoService;
        private readonly ITipoDeCobrancaService _tipoDeCobrancaService;
        private readonly IHeaderProvider _headerProvider;

        public CondicaoPagamentoApplicationService(ICondicaoPagamentoService condicaoPagamentoService, ITipoDeCobrancaService tipoDeCobrancaService, IUnitOfWork unityOfWork, IHeaderProvider headerProvider)
            : base(condicaoPagamentoService, unityOfWork)
        {
            _condicaoPagamentoService = condicaoPagamentoService;
            _tipoDeCobrancaService = tipoDeCobrancaService;
            _headerProvider = headerProvider;
        }

        public PagedList<CondicaoPagamentoResponse> Listar(int pagina, int porPagina, Expression<Func<CondicaoPagamento, bool>> filter)
        {
            return AutoMapper.Mapper.Map(_condicaoPagamentoService.ListarEmOrdemCrescente(pagina, porPagina, filter), new PagedList<CondicaoPagamentoResponse>());
        }

        public IEnumerable<CondicaoPagamento> ListarPorUsinaDataParaAprovacaoPendente(int idUsina, DateTime data, string intervenienteTipo)
        {
            return _condicaoPagamentoService.ListarPorUsinaDataParaAprovacaoPendente(idUsina, data, intervenienteTipo);
        }
        public IEnumerable<CondicaoPagamentoResponse> ListarPorUsinaDataIntervenienteTipo(int idUsina, DateTime data, string intervenienteTipo, int segmentacao)
        {
            return AutoMapper.Mapper.Map(_condicaoPagamentoService.ListarPorUsinaDataIntervenienteTipo(idUsina, data, intervenienteTipo, segmentacao), new List<CondicaoPagamentoResponse>());
        }

        public float ObterValorAdicionalM3PorCondicaoPagamentoUsinaPrecoUnitarioTabela(int idCondicaoPagamento, int idUsina, float precoUnitarioTabela)
        {
            return _condicaoPagamentoService.ObterValorAdicionalM3PorCondicaoPagamentoUsinaPrecoUnitarioTabela(idCondicaoPagamento, idUsina, precoUnitarioTabela);
        }

        public void Deletar(int idCondicaoPagamento)
        {
            var condicaoPagamento = _condicaoPagamentoService.ObterPorId(idCondicaoPagamento);
            _condicaoPagamentoService.Remover(condicaoPagamento);

            var parcelas = _condicaoPagamentoService.ListarFiltradosTracking<CondicaoPagamentoParcela>
                    (t => t.CondicaoPagamentoCodigo == idCondicaoPagamento);

            foreach (var parcela in parcelas)
            {
                _condicaoPagamentoService.Remover<CondicaoPagamentoParcela>(parcela);
            }

            Commit();
        }

        public void Adicionar(CondicaoPagamentoInclusaoRequest condicaoPagamentoRequest)
        {
            var condicoesPagamento = _condicaoPagamentoService.ListarTodos();
            var newCondicaoPagamentoCodigo = condicoesPagamento.Max(t => t.Codigo) + 1;

            condicaoPagamentoRequest.Codigo = newCondicaoPagamentoCodigo;
            foreach (var parcela in condicaoPagamentoRequest.Parcelas)
                parcela.CondicaoPagamentoCodigo = newCondicaoPagamentoCodigo;

            var newCondicaoPagamento = AutoMapper.Mapper.Map(condicaoPagamentoRequest, new CondicaoPagamento());

            newCondicaoPagamento.DescricaoCompleta = newCondicaoPagamento.Descricao;

            if (!newCondicaoPagamento.CondicaoPagamentoToAddIsValid(condicoesPagamento))
                return;

            _condicaoPagamentoService.Adicionar(newCondicaoPagamento);

            foreach (var parcela in newCondicaoPagamento.Parcelas)
                _condicaoPagamentoService.Adicionar<CondicaoPagamentoParcela>(parcela);

            Commit();
        }

        public void Atualizar(CondicaoPagamentoAlteracaoRequest condicaoPagamentoRequest)
        {
            var condicaoPagamentoAntiga = _condicaoPagamentoService.ObterPorId(condicaoPagamentoRequest.Codigo);
            var condicaoPagamentoAtualizado = AutoMapper.Mapper.Map(condicaoPagamentoRequest, condicaoPagamentoAntiga);

            condicaoPagamentoAtualizado.DescricaoCompleta = condicaoPagamentoAtualizado.Descricao;

            var condicaoPagamentoParcelasAntiga = _condicaoPagamentoService.ListarFiltradosTracking<CondicaoPagamentoParcela>
                (t => t.CondicaoPagamentoCodigo == condicaoPagamentoRequest.Codigo);
            condicaoPagamentoAtualizado.Parcelas = AutoMapper.Mapper.Map(condicaoPagamentoRequest.Parcelas, condicaoPagamentoParcelasAntiga).ToList();

            if (!condicaoPagamentoAtualizado.CondicaoPagamentoToUpdateIsValid())
                return;

            Commit();
        }

        public bool PossuiObrasUtilizando(int idCondicaoPagamento)
        {
            return _condicaoPagamentoService.PossuiObrasUtilizando(idCondicaoPagamento);
        }

        public ResultDTO<CondicaoDePagamentoAdicionarResponse> AdicionarIntegration(CondicoesPagamentoAdicionarRequest[] request)
        {
            var errors = new List<Error>();
            var idioma = _headerProvider.GetAcceptLanguage();
            var daysUntilThePayments = new List<List<int>>();
            
            for (int i = 0; i < request.Length; i++)
            {
                if (request[i].CondicaoDaCobrancaCod!=null && request[i].CondicaoDaCobrancaCod!="E" && request[i].CondicaoDaCobrancaCod!="D" && 
                    request[i].CondicaoDaCobrancaCod!="M" && request[i].CondicaoDaCobrancaCod!="Q" && request[i].CondicaoDaCobrancaCod!="F" && 
                    request[i].CondicaoDaCobrancaCod!="S")
                    errors.Add(new Error(
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_TYPE_DOES_NOT_FOLLOW_CORRECT_PATTERN.GetMessageCode(),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_TYPE_DOES_NOT_FOLLOW_CORRECT_PATTERN.GetResourceMessage(idioma, "type_of_condition"),
                        i));

                if (request[i].TiposDeCobrancaCodigos != null)
                {
                    errors.AddRange(from tipoCobranca in request[i].TiposDeCobrancaCodigos where _tipoDeCobrancaService.ObterPorId(tipoCobranca ?? 0) == null
                                    select new Error(
                                        EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode(),
                                        EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(idioma, "billing_type"),
                                        i));
                    
                    if (request[i].TiposDeCobrancaCodigos.GroupBy(x => x).Any(g => g.Count() > 1))
                        errors.Add(new Error(
                            EResourcesDefaultMessages.DEFAULT_MESSAGES_DUPLICATE_ELEMENT_IN_THE_LIST.GetMessageCode(),
                            EResourcesDefaultMessages.DEFAULT_MESSAGES_DUPLICATE_ELEMENT_IN_THE_LIST.GetResourceMessage(idioma, "Billing Type", "Billing_type"),
                            i));
                }
                
                if (request[i].VencimentoFixoSemana && (request[i].DiaVencimentoFixoSemana<2 || request[i].DiaVencimentoFixoSemana>6))
                    errors.Add(new Error(
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_GREATER_THAN_LESS_THAN.GetMessageCode(),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_GREATER_THAN_LESS_THAN.GetResourceMessage(idioma, "day_of_the_week", "2", "6"),
                        i));

                if (!request[i].VencimentoFixoSemana && request[i].DiaVencimentoFixoSemana!=0)
                    errors.Add(new Error(
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_TYPE_DOES_NOT_FOLLOW_CORRECT_PATTERN.GetMessageCode(),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_TYPE_DOES_NOT_FOLLOW_CORRECT_PATTERN.GetResourceMessage(idioma, "day_of_the_week"),
                        i));
                
                if(request[i].PercentualParcelas.Sum()!=100)
                    errors.Add(new Error(
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_SUM_OF_FIELD.GetMessageCode(),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_SUM_OF_FIELD.GetResourceMessage(idioma, "percentage_per_payment", "100"),
                        i));
                
                if(request[i].PercentualParcelas.Length != request[i].QuantidadeParcelas)
                    errors.Add(new Error(EResourcesDefaultMessages.DEFAULT_MESSAGES_THE_LENGTH_OF_FIELD_MUST_BE_EQUAL_TO.GetMessageCode(),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_THE_LENGTH_OF_FIELD_MUST_BE_EQUAL_TO.GetResourceMessage(idioma, "percentage_per_payment", "number_of_payments"),
                        i));

                daysUntilThePayments.Add( new List<int>
                {
                    request[i].Vencimento1Parcela, request[i].Vencimento2Parcela ?? 0, request[i].Vencimento3Parcela ?? 0,
                    request[i].Vencimento4Parcela ?? 0, request[i].Vencimento5Parcela ?? 0, request[i].Vencimento6Parcela ?? 0,
                    request[i].Vencimento7Parcela ?? 0, request[i].Vencimento8Parcela ?? 0, request[i].Vencimento9Parcela ?? 0,
                    request[i].Vencimento10Parcela ?? 0, request[i].Vencimento11Parcela ?? 0, request[i].Vencimento12Parcela ?? 0
                });

                if(request[i].QuantidadeParcelas!=daysUntilThePayments[i].GetRange(0, request[i].QuantidadeParcelas).Count(src => src != 0) 
                   || daysUntilThePayments[i].GetRange(request[i].QuantidadeParcelas, 12-request[i].QuantidadeParcelas).Count(src => src != 0)>0)
                    errors.Add(new Error(EResourcesCondicaoDePagamento.CONDICAO_DE_PAGAMENTO_ERROR_TCON375f315f31.GetMessageCode(),
                        EResourcesCondicaoDePagamento.CONDICAO_DE_PAGAMENTO_ERROR_TCON375f315f31.GetResourceMessage(idioma),
                        i));
                
            }
            if (errors.Count > 0)
                return new ResultDTO<CondicaoDePagamentoAdicionarResponse>(
                            EResultDTOStatus.Error,
                            EResourcesDefaultMessages.DEFAULT_MESSAGES_ERROR_PROCESSING_RECORDS.GetResourceMessage(idioma),
                            errors);
            
            // Cadastrar
            using (var scope = new TransactionScope())
            {
                try
                {
                    for(int i = 0; i < request.Length; i++)
                    {
                        var newCondicaoPagamento = new CondicaoPagamento();

                        newCondicaoPagamento.Codigo = _condicaoPagamentoService.ObterProximoCodigo();
                        newCondicaoPagamento = AutoMapper.Mapper.Map(request[i], newCondicaoPagamento);
                        newCondicaoPagamento.AnalisaFraude = "N";
                        newCondicaoPagamento.Ativo = "S";
                        newCondicaoPagamento.MesFixo30Dias = "N";
                        newCondicaoPagamento.RetencaoPrimeiraParcela = "N";
                        
                        for (int j = 0; j < request[i].PercentualParcelas.Length; j++)
                        {
                            _condicaoPagamentoService.Adicionar(new CondicaoPagamentoParcela
                            {
                                CondicaoPagamentoCodigo = newCondicaoPagamento.Codigo,
                                Percentual = request[i].PercentualParcelas[j] ?? 0,
                                Dias = daysUntilThePayments[i][j]
                            });
                        }

                        _condicaoPagamentoService.Adicionar(newCondicaoPagamento);
                        Commit();
                    }
                    Commit();
                    scope.Complete();

                    var result = new CondicaoDePagamentoAdicionarResponse(request.Length);
                    return new ResultDTO<CondicaoDePagamentoAdicionarResponse>(EResultDTOStatus.Success, "Success when inserting records", result, "");                
                }
                catch
                {
                    scope.Dispose();
                    var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                    var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                    return new ResultDTO<CondicaoDePagamentoAdicionarResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
                }
            }
        }

        public ResultDTO<CondicaoDePagamentoResponse> AtualizaPorIdIntegration(int id, CondicoesPagamentoAtualizarRequest request)
        {
            var condicaoPagamento = _condicaoPagamentoService.ObterPeloId(id, true);
            if (condicaoPagamento == null)
                return new ResultDTO<CondicaoDePagamentoResponse>(
                    EResultDTOStatus.Error,
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

            return AtualizarIntegration(condicaoPagamento, request);
        }

        public ResultDTO<CondicaoDePagamentoResponse> AtualizarPorExternalIdIntegration(string externalId, CondicoesPagamentoAtualizarRequest request)
        {
            var condicaoPagamento = _condicaoPagamentoService.ObterPorExternalId(externalId, true);
            if (condicaoPagamento == null)
                return new ResultDTO<CondicaoDePagamentoResponse>(
                    EResultDTOStatus.Error,
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

            return AtualizarIntegration(condicaoPagamento, request);
        }

        public ResultDTO<CondicaoDePagamentoResponse> AtualizarIntegration(CondicaoPagamento condicaoPagamento,
            CondicoesPagamentoAtualizarRequest request)
        {
            var errors = new List<Error>();
            var idioma = _headerProvider.GetAcceptLanguage();

            var codeExists = (_condicaoPagamentoService.ObterPorId(request.Codigo) != null);
            if (codeExists)
                errors.Add(new Error(
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetMessageCode(),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetResourceMessage(idioma, "Code")));

            if (request.CondicaoDaCobrancaCod!=null && request.CondicaoDaCobrancaCod!="E" && request.CondicaoDaCobrancaCod!="D" && 
                request.CondicaoDaCobrancaCod!="M" && request.CondicaoDaCobrancaCod!="Q" && request.CondicaoDaCobrancaCod!="F" && 
                request.CondicaoDaCobrancaCod!="S")
                errors.Add(new Error(
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_TYPE_DOES_NOT_FOLLOW_CORRECT_PATTERN.GetMessageCode(),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_TYPE_DOES_NOT_FOLLOW_CORRECT_PATTERN.GetResourceMessage(idioma, "type_of_condition")));

            if ((request.VencimentoFixoSemana ?? false) && (request.DiaVencimentoFixoSemana<2 || request.DiaVencimentoFixoSemana>6 || request.DiaVencimentoFixoSemana==null))
                errors.Add(new Error(
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_GREATER_THAN_LESS_THAN.GetMessageCode(), 
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_GREATER_THAN_LESS_THAN.GetResourceMessage(idioma,"day_of_the_week", "2", "6")));

            if (!(request.VencimentoFixoSemana ?? false) && request.DiaVencimentoFixoSemana!=0 && request.DiaVencimentoFixoSemana!=null)
                errors.Add(new Error(
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_TYPE_DOES_NOT_FOLLOW_CORRECT_PATTERN.GetMessageCode(), 
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_TYPE_DOES_NOT_FOLLOW_CORRECT_PATTERN.GetResourceMessage(idioma, "day_of_the_week")));
                
            if (request.VencimentoFixoSemana==null && request.DiaVencimentoFixoSemana!=null) 
                errors.Add(new Error(
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY.GetMessageCode(), 
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY.GetResourceMessage(idioma, "fixed_due_date_in_the_week")));
                
            if(request.PercentualParcelas?.Sum()!=100 && request.PercentualParcelas!=null)
                errors.Add(new Error(
                     EResourcesDefaultMessages.DEFAULT_MESSAGES_SUM_OF_FIELD.GetMessageCode(),
                     EResourcesDefaultMessages.DEFAULT_MESSAGES_SUM_OF_FIELD.GetResourceMessage(idioma, "percentage_per_payment", "100"), 0));
                
            if(request.TiposDeCobrancaCodigos!=null)
            {
                errors.AddRange(from tipoCobranca in request.TiposDeCobrancaCodigos
                    where _tipoDeCobrancaService.ObterPorId(tipoCobranca ?? 0) == null
                    select new Error(
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode(),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(idioma, "billing_type")));
                    
                if (request.TiposDeCobrancaCodigos.GroupBy(x => x).Any(g => g.Count() > 1))
                    errors.Add(new Error(
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_DUPLICATE_ELEMENT_IN_THE_LIST.GetMessageCode(),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_DUPLICATE_ELEMENT_IN_THE_LIST.GetResourceMessage(idioma, "BillingType", "Billing_type")));
            }
                
            if(request.PercentualParcelas == null && request.QuantidadeParcelas != null)
                errors.Add(new Error(
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY.GetMessageCode(),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY.GetResourceMessage(idioma, "percentage_per_payment")));
                
            if(request.QuantidadeParcelas == null && request.PercentualParcelas != null)
                errors.Add(new Error(
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY.GetMessageCode(),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_CANNOT_BE_EMPTY.GetResourceMessage(idioma, "number_of_payments")));

            if (request.PercentualParcelas?.Length != request.QuantidadeParcelas && request.PercentualParcelas != null && request.QuantidadeParcelas != null)
                errors.Add(new Error(
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_THE_LENGTH_OF_FIELD_MUST_BE_EQUAL_TO.GetMessageCode(),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_THE_LENGTH_OF_FIELD_MUST_BE_EQUAL_TO.GetResourceMessage(idioma, "percentage_per_payment", "number_of_payments")));
                
            if(request.QuantidadeParcelas != null)
            {

                request.Vencimento1Parcela = request.Vencimento1Parcela ?? 0;
                request.Vencimento2Parcela = request.Vencimento2Parcela ?? 0;
                request.Vencimento3Parcela = request.Vencimento3Parcela ?? 0;
                request.Vencimento4Parcela = request.Vencimento4Parcela ?? 0;
                request.Vencimento5Parcela = request.Vencimento5Parcela ?? 0;
                request.Vencimento6Parcela = request.Vencimento6Parcela ?? 0;
                request.Vencimento7Parcela = request.Vencimento7Parcela ?? 0;
                request.Vencimento8Parcela = request.Vencimento8Parcela ?? 0;
                request.Vencimento9Parcela = request.Vencimento9Parcela ?? 0;
                request.Vencimento10Parcela = request.Vencimento10Parcela ?? 0; 
                request.Vencimento11Parcela = request.Vencimento11Parcela ?? 0;
                request.Vencimento12Parcela = request.Vencimento12Parcela ?? 0;
                    
                var daysUntilThePayments = new List<int>
                {
                    request.Vencimento1Parcela ?? 0, request.Vencimento2Parcela ?? 0,
                    request.Vencimento3Parcela ?? 0,
                    request.Vencimento4Parcela ?? 0, request.Vencimento5Parcela ?? 0,
                    request.Vencimento6Parcela ?? 0,
                    request.Vencimento7Parcela ?? 0, request.Vencimento8Parcela ?? 0,
                    request.Vencimento9Parcela ?? 0,
                    request.Vencimento10Parcela ?? 0, request.Vencimento11Parcela ?? 0,
                    request.Vencimento12Parcela ?? 0
                };

                if(request.QuantidadeParcelas!=daysUntilThePayments.GetRange(0, request.QuantidadeParcelas ?? 0).Count(src => src != 0) 
                   || daysUntilThePayments.GetRange(request.QuantidadeParcelas ?? 0, 12-request.QuantidadeParcelas ?? 0).Count(src => src != 0)>0)
                    errors.Add(new Error(EResourcesCondicaoDePagamento.CONDICAO_DE_PAGAMENTO_ERROR_TCON375f315f31.GetMessageCode(),
                        EResourcesCondicaoDePagamento.CONDICAO_DE_PAGAMENTO_ERROR_TCON375f315f31.GetResourceMessage(idioma),
                        0));
            }
                
                
            if (errors.Count > 0)
                return new ResultDTO<CondicaoDePagamentoResponse>(
                            EResultDTOStatus.Error,
                            EResourcesDefaultMessages.DEFAULT_MESSAGES_ERROR_PROCESSING_RECORDS.GetResourceMessage(idioma),
                            errors);

            // Atualizar
            using (var scope = new TransactionScope())
            {
                try
                {
                    condicaoPagamento = AutoMapper.Mapper.Map(request, condicaoPagamento);

                    if (request.PercentualParcelas != null)
                    {
                        var daysUntilThePayments = new List<int>
                        {
                            condicaoPagamento.Vencimento1Parcela, condicaoPagamento.Vencimento2Parcela,
                            condicaoPagamento.Vencimento3Parcela, condicaoPagamento.Vencimento4Parcela, 
                            condicaoPagamento.Vencimento5Parcela, condicaoPagamento.Vencimento6Parcela,
                            condicaoPagamento.Vencimento7Parcela, condicaoPagamento.Vencimento8Parcela,
                            condicaoPagamento.Vencimento9Parcela, condicaoPagamento.Vencimento10Parcela, 
                            condicaoPagamento.Vencimento11Parcela, condicaoPagamento.Vencimento12Parcela
                        };
                            
                        var condicaoPagamentoParcelasAntiga = _condicaoPagamentoService.ListarFiltradosTracking<CondicaoPagamentoParcela>
                            (t => t.CondicaoPagamentoCodigo == condicaoPagamento.Codigo);

                        var parcelasResquest = new List<CondicaoPagamentoParcela>(request.PercentualParcelas.Select((percentual, index) => new CondicaoPagamentoParcela
                        {
                            CondicaoPagamentoCodigo = condicaoPagamento.Codigo,
                            Percentual = request.PercentualParcelas[index] ?? 0,
                            Dias = daysUntilThePayments[index]
                        }));
                        condicaoPagamento.Parcelas = AutoMapper.Mapper.Map(parcelasResquest, condicaoPagamentoParcelasAntiga).ToList();
                    }

                    Commit();
                    scope.Complete();

                    return new ResultDTO<CondicaoDePagamentoResponse>(EResultDTOStatus.Success, "Successfully updated", AutoMapper.Mapper.Map(condicaoPagamento, new CondicaoDePagamentoResponse()));
                }
                catch
                {
                    scope.Dispose();
                    var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                    var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                    return new ResultDTO<CondicaoDePagamentoResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
                }
            }
        }

        public ResultDTO<ICollection<CondicaoDePagamentoResponse>> ListarIntegration()
        {
            var result = AutoMapper.Mapper.Map(_condicaoPagamentoService.Listar(), new List<CondicaoDePagamentoResponse>());
    
            if (result.Count == 0)
                return new ResultDTO<ICollection<CondicaoDePagamentoResponse>>(
                    EResultDTOStatus.Error,
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

            return new ResultDTO<ICollection<CondicaoDePagamentoResponse>>(EResultDTOStatus.Success, "", result);
        }

        public ResultDTO<CondicaoDePagamentoResponse> ObterPorIdIntegration(int id)
        {
            try
            {
                var result = AutoMapper.Mapper.Map(_condicaoPagamentoService.ObterPeloId(id), new CondicaoDePagamentoResponse());

                if (result == null)
                    return new ResultDTO<CondicaoDePagamentoResponse>(
                        EResultDTOStatus.Error, 
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

                return new ResultDTO<CondicaoDePagamentoResponse>(EResultDTOStatus.Success, "", result);
            }
            catch
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<CondicaoDePagamentoResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }

        public ResultDTO<CondicaoDePagamentoResponse> ObterPorExternalIdIntegration(string externalId)
        {
            try
            {
                var result = AutoMapper.Mapper.Map(_condicaoPagamentoService.ObterPorExternalId(externalId), new CondicaoDePagamentoResponse());

                if (result == null)
                    return new ResultDTO<CondicaoDePagamentoResponse>(
                        EResultDTOStatus.Error,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

                return new ResultDTO<CondicaoDePagamentoResponse>(EResultDTOStatus.Success, "", result);
            }
            catch
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<CondicaoDePagamentoResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }
    }
}