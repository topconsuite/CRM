using System;
using System.Collections.Generic;
using System.Transactions;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;
using TopSys.TopConWeb.Application.DTOS.Request.SituacaoFinanceira;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Application.DTOS.Response.SituacaoFinanceira;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Integrations;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;

namespace TopSys.TopConWeb.Application
{
    public class SituacaoFinanceiraApplicationService : ApplicationServiceBase<SituacaoFinanceira>, ISituacaoFinanceiraApplicationService
    {
        private readonly ISituacaoFinanceiraService _situacaoFinanceiraService;
        private readonly IHeaderProvider _headerProvider;

        public SituacaoFinanceiraApplicationService(ISituacaoFinanceiraService situacaoFinanceiraService, IUnitOfWork unityOfWork, IHeaderProvider headerProvider)
            : base(situacaoFinanceiraService, unityOfWork)
        {
            _situacaoFinanceiraService = situacaoFinanceiraService;
            _headerProvider = headerProvider;
        }

        public ResultDTO<SituacaoFinanceiraAdicionarResponse> Adicionar(SituacaoFinanceiraAdicionarRequest[] request)
        {
            var errors = new List<Error>();
            var idioma = _headerProvider.GetAcceptLanguage();

            // Verificar Erros
            for (int i = 0; i < request.Length; i++)
            {
                var codeExists = (_situacaoFinanceiraService.ObterPorId(request[i].Codigo) != null);
                if (codeExists)
                    errors.Add(new Error(
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetMessageCode(),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetResourceMessage(idioma, "Code"),
                        i));

                if (request[i].OperacaoBaixa != 0)
                {
                    if (!_situacaoFinanceiraService.VerificaSeExisteOperacao(request[i].OperacaoBaixa))
                        errors.Add(new Error(
                            EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode(),
                            EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(idioma, "sell_operation"),
                            i));
                    else
                    {
                        if (!_situacaoFinanceiraService.ValidaOperacaoBaixa(request[i].OperacaoBaixa))
                            errors.Add(new Error(
                                EResourcesDefaultMessages.DEFAULT_MESSAGES_TYPE_DOES_NOT_FOLLOW_CORRECT_PATTERN.GetMessageCode(),
                                EResourcesDefaultMessages.DEFAULT_MESSAGES_TYPE_DOES_NOT_FOLLOW_CORRECT_PATTERN.GetResourceMessage(idioma, "type_of_sell_operation"),
                                i));
                    }
                }
            }

            if (errors.Count > 0)
                return new ResultDTO<SituacaoFinanceiraAdicionarResponse>(
                    EResultDTOStatus.Error,
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_ERROR_PROCESSING_RECORDS.GetResourceMessage(idioma),
                    errors);

            // Cadastrar
            using (var scope = new TransactionScope())
            {
                try
                {
                    foreach (var cadastro in request)
                    {
                        var newSituacaoFinanceira = new SituacaoFinanceira();

                        newSituacaoFinanceira = AutoMapper.Mapper.Map(cadastro, newSituacaoFinanceira);

                        _situacaoFinanceiraService.Adicionar(newSituacaoFinanceira);
                    }

                    Commit();
                    scope.Complete();

                    var result = new SituacaoFinanceiraAdicionarResponse(request.Length);

                    return new ResultDTO<SituacaoFinanceiraAdicionarResponse>(EResultDTOStatus.Success, "Success when inserting records", result, "");
                }
                catch
                {
                    scope.Dispose();
                    var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                    var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                    return new ResultDTO<SituacaoFinanceiraAdicionarResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
                }
            }
        }

        public ResultDTO<SituacaoFinanceiraResponse> AtualizarPorId(int id, SituacaoFinanceiraAtualizarRequest request)
        {
            var situacaoFinanceira = _situacaoFinanceiraService.ObterPorId(id, true);
            if (situacaoFinanceira == null)
                return new ResultDTO<SituacaoFinanceiraResponse>(
                    EResultDTOStatus.Error,
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

            return Atualizar(situacaoFinanceira, request);
        }

        public ResultDTO<SituacaoFinanceiraResponse> Atualizar(SituacaoFinanceira situacaoFinanceira, SituacaoFinanceiraAtualizarRequest request)
        {
            var idioma = _headerProvider.GetAcceptLanguage();

            if (request.OperacaoBaixa != 0 && request.OperacaoBaixa != null)
            {
                if (!_situacaoFinanceiraService.VerificaSeExisteOperacao(request?.OperacaoBaixa ?? 0))
                    return new ResultDTO<SituacaoFinanceiraResponse>(
                        EResultDTOStatus.Error,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(idioma),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());
                else
                {
                    if (!_situacaoFinanceiraService.ValidaOperacaoBaixa(request?.OperacaoBaixa ?? 0))
                        return new ResultDTO<SituacaoFinanceiraResponse>(
                            EResultDTOStatus.Error,
                            EResourcesDefaultMessages.DEFAULT_MESSAGES_TYPE_DOES_NOT_FOLLOW_CORRECT_PATTERN.GetResourceMessage(idioma, "type_of_sell_operation"),
                            EResourcesDefaultMessages.DEFAULT_MESSAGES_TYPE_DOES_NOT_FOLLOW_CORRECT_PATTERN.GetMessageCode());
                }
            }

            using (var scope = new TransactionScope())
            {
                try
                {
                    situacaoFinanceira = AutoMapper.Mapper.Map(request, situacaoFinanceira);

                    Commit();
                    scope.Complete();

                    return new ResultDTO<SituacaoFinanceiraResponse>(EResultDTOStatus.Success, "Successfully updated", AutoMapper.Mapper.Map(situacaoFinanceira, new SituacaoFinanceiraResponse()));
                }
                catch
                {
                    scope.Dispose();
                    var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                    var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                    return new ResultDTO<SituacaoFinanceiraResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
                }
            }
        }

        public ResultDTO<ICollection<SituacaoFinanceiraResponse>> Listar()
        {
            var result = AutoMapper.Mapper.Map(_situacaoFinanceiraService.Listar(), new List<SituacaoFinanceiraResponse>());

            if (result.Count == 0)
                return new ResultDTO<ICollection<SituacaoFinanceiraResponse>>(
                    EResultDTOStatus.Error,
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

            return new ResultDTO<ICollection<SituacaoFinanceiraResponse>>(EResultDTOStatus.Success, "", result);
        }

        public ResultDTO<SituacaoFinanceiraResponse> ObterPorId(int id)
        {
            try
            {
                var result = AutoMapper.Mapper.Map(_situacaoFinanceiraService.ObterPorId(id), new SituacaoFinanceiraResponse());

                if (result == null)
                    return new ResultDTO<SituacaoFinanceiraResponse>(
                        EResultDTOStatus.Error,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetResourceMessage(_headerProvider.GetAcceptLanguage(), "Id"),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetMessageCode());

                return new ResultDTO<SituacaoFinanceiraResponse>(EResultDTOStatus.Success, "", result);
            }
            catch (Exception)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<SituacaoFinanceiraResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }

        public ResultDTO<int> DeletarPorId(int id)
        {
            var situacaoFinanceira = _situacaoFinanceiraService.ObterPorId(id, true);
            if (situacaoFinanceira == null)
                return new ResultDTO<int>(
                    EResultDTOStatus.Error,
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetResourceMessage(_headerProvider.GetAcceptLanguage(), "Id"),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetMessageCode());

            return Deletar(situacaoFinanceira);
        }

        public ResultDTO<int> Deletar(SituacaoFinanceira situacaoFinanceira)
        {
            try
            {
                var estaEmUso = _situacaoFinanceiraService.EstaEmUsoSituacaoFinanceira(situacaoFinanceira.Codigo);
                if (estaEmUso)
                    return new ResultDTO<int>(
                        EResultDTOStatus.Alert,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_CANT_BE_DELETED_SINCE_ITS_IN_USE.GetResourceMessage(_headerProvider.GetAcceptLanguage(), "Finance_situation",
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_CANT_BE_DELETED_SINCE_ITS_IN_USE.GetMessageCode()));

                _situacaoFinanceiraService.Remover(situacaoFinanceira);

                Commit();

                return new ResultDTO<int>(EResultDTOStatus.Success, "Successfully deleted");
            }
            catch (Exception)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<int>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }
    }
}
