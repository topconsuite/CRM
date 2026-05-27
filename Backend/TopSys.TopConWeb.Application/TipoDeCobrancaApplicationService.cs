using System;
using System.Collections.Generic;
using System.Transactions;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;
using TopSys.TopConWeb.Application.DTOS.Request.TipoDeCobranca;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Application.DTOS.Response.TipoDeCobranca;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Integrations;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Domain.Services;
using TopSys.TopConWeb.Infra.Data.Helpers;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;


namespace TopSys.TopConWeb.Application
{
    public class TipoDeCobrancaApplicationService : ApplicationServiceBase<TipoDeCobranca>,
        ITipoDeCobrancaApplicationService
    {
        private readonly ITipoDeCobrancaService _tipoDeCobrancaService;
        private readonly IPortadorService _portadorService;
        private readonly ISituacaoFinanceiraService _situacaoFinanceiraService;
        private readonly IHeaderProvider _headerProvider;

        public TipoDeCobrancaApplicationService(ITipoDeCobrancaService tipoDeCobrancaService,
            IPortadorService portadorService,
            ISituacaoFinanceiraService situacaoFinanceiraService, IUnitOfWork unityOfWork,
            IHeaderProvider headerProvider)
            : base(tipoDeCobrancaService, unityOfWork)
        {
            _tipoDeCobrancaService = tipoDeCobrancaService;
            _portadorService = portadorService;
            _situacaoFinanceiraService = situacaoFinanceiraService;
            _headerProvider = headerProvider;
        }

        public ResultDTO<TipoDeCobrancaAdicionarResponse> Adicionar(TipoDeCobrancaAdicionarRequest[] request)
        {
            var errors = new List<Error>();
            var idioma = _headerProvider.GetAcceptLanguage();

            for (int i = 0; i < request.Length; i++)
            {
                var codeExists = (_tipoDeCobrancaService.ObterPorId(request[i].Codigo) != null);
                if (codeExists)
                    errors.Add(new Error(
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetMessageCode(),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetResourceMessage(
                            idioma, "Code"),
                        i));

                if (request[i].Forma != "BE" && request[i].Forma != "BL" && request[i].Forma != "DN" &&
                    request[i].Forma != "DP" && request[i].Forma != "CC" && request[i].Forma != "CD" &&
                    request[i].Forma != "CH" && request[i].Forma != "CP" && request[i].Forma != "CT")
                    errors.Add(new Error(
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_TYPE_DOES_NOT_FOLLOW_CORRECT_PATTERN
                            .GetMessageCode(),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_TYPE_DOES_NOT_FOLLOW_CORRECT_PATTERN
                            .GetResourceMessage(idioma, "type_of_payment_form"),
                        i));

                if (_portadorService.ObterPorId(request[i].Portador) == null)
                    errors.Add(new Error(
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode(),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(
                            idioma, "Carrier"),
                        i));

                if (request[i].Situacao != 0 && _situacaoFinanceiraService.ObterPorId(request[i].Situacao) == null)
                    errors.Add(new Error(
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode(),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(
                            idioma, "Situation"),
                        i));
            }

            if (errors.Count > 0)
                return new ResultDTO<TipoDeCobrancaAdicionarResponse>(
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
                        var newTipoDeCobranca = new TipoDeCobranca();

                        newTipoDeCobranca = AutoMapper.Mapper.Map(cadastro, newTipoDeCobranca);

                        _tipoDeCobrancaService.Adicionar(newTipoDeCobranca);
                    }

                    Commit();
                    scope.Complete();

                    var result = new TipoDeCobrancaAdicionarResponse(request.Length);
                    return new ResultDTO<TipoDeCobrancaAdicionarResponse>(EResultDTOStatus.Success,
                        "Success when inserting records", result, "");
                }
                catch
                {
                    scope.Dispose();
                    var errorMessage =
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(
                            _headerProvider.GetAcceptLanguage());
                    var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                    return new ResultDTO<TipoDeCobrancaAdicionarResponse>(EResultDTOStatus.Error, errorMessage,
                        errorCode);
                }
            }
        }

        public ResultDTO<TipoDeCobrancaResponse> AtualizarPorId(int id, TipoDeCobrancaAtualizarRequest request)
        {
            var tipoDeCobranca = _tipoDeCobrancaService.ObterPorId(id, true);
            if (tipoDeCobranca == null)
                return new ResultDTO<TipoDeCobrancaResponse>(
                    EResultDTOStatus.Error,
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(
                        _headerProvider.GetAcceptLanguage(),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode()));

            return Atualizar(tipoDeCobranca, request);
        }

        public ResultDTO<TipoDeCobrancaResponse> Atualizar(TipoDeCobranca tipoDeCobranca,
            TipoDeCobrancaAtualizarRequest request)
        {
            // Verifica Erros
            var errors = new List<Error>();
            var idioma = _headerProvider.GetAcceptLanguage();

            if (request.Forma != "BE" && request.Forma != "BL" && request.Forma != "DN"
                && request.Forma != "DP" && request.Forma != "CC" && request.Forma != "CD"
                && request.Forma != "CH" && request.Forma != "CP" && request.Forma != "CT"
                && request.Forma != null)
                errors.Add(new Error(
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_TYPE_DOES_NOT_FOLLOW_CORRECT_PATTERN.GetMessageCode(),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_TYPE_DOES_NOT_FOLLOW_CORRECT_PATTERN.GetResourceMessage(
                        idioma, "type_of_payment_form")));

            if (_portadorService.ObterPorId(request.Portador ?? 0) == null && request.Portador != null)
                errors.Add(new Error(
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode(),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(idioma,
                        "Carrier")));
            ;

            if (request.Situacao != null && _situacaoFinanceiraService.ObterPorId(request.Situacao ?? 0) == null)
                errors.Add(new Error(
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode(),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(idioma,
                        "Situation")));
            ;

            if (errors.Count > 0)
                return new ResultDTO<TipoDeCobrancaResponse>(
                    EResultDTOStatus.Error,
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_ERROR_PROCESSING_RECORDS.GetResourceMessage(idioma),
                    errors);

            using (var scope = new TransactionScope())
            {
                try
                {
                    tipoDeCobranca = AutoMapper.Mapper.Map(request, tipoDeCobranca);

                    Commit();
                    scope.Complete();

                    return new ResultDTO<TipoDeCobrancaResponse>(EResultDTOStatus.Success, "Successfully updated",
                        AutoMapper.Mapper.Map(tipoDeCobranca, new TipoDeCobrancaResponse()));
                }
                catch
                {
                    scope.Dispose();
                    var errorMessage =
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(
                            _headerProvider.GetAcceptLanguage());
                    var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                    return new ResultDTO<TipoDeCobrancaResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
                }
            }
        }

        public ResultDTO<ICollection<TipoDeCobrancaResponse>> Listar()
        {
            var result = AutoMapper.Mapper.Map(_tipoDeCobrancaService.Listar(), new List<TipoDeCobrancaResponse>());

            if (result.Count == 0)
                return new ResultDTO<ICollection<TipoDeCobrancaResponse>>(EResultDTOStatus.Error,
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(
                        _headerProvider.GetAcceptLanguage()),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

            return new ResultDTO<ICollection<TipoDeCobrancaResponse>>(EResultDTOStatus.Success, "", result);
        }

        public ResultDTO<TipoDeCobrancaResponse> ObterPorId(int id)
        {
            try
            {
                var result = AutoMapper.Mapper.Map(_tipoDeCobrancaService.ObterPorId(id), new TipoDeCobrancaResponse());

                if (result == null)
                    return new ResultDTO<TipoDeCobrancaResponse>(
                        EResultDTOStatus.Error,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(
                            _headerProvider.GetAcceptLanguage(),
                            EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode()));

                return new ResultDTO<TipoDeCobrancaResponse>(EResultDTOStatus.Success, "", result);
            }
            catch
            {
                var errorMessage =
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(
                        _headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<TipoDeCobrancaResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }
    }
}