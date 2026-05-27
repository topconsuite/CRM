using System;
using System.Collections.Generic;
using System.Transactions;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;
using TopSys.TopConWeb.Application.DTOS.Request.MunicipioTributacao;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Application.DTOS.Response.MunicipioTributacao;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Integrations;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;

namespace TopSys.TopConWeb.Application
{
    public class MunicipioTributacaoApplicationService : ApplicationServiceBase<Municipio>, IMunicipioTributacaoApplicationService
	{
		public readonly IMunicipioTributacaoService _municipioTributacaoService;
        private readonly IHeaderProvider _headerProvider;

        public MunicipioTributacaoApplicationService(IMunicipioTributacaoService municipioTributacaoService, IUnitOfWork unityOfWork, IHeaderProvider headerProvider)
            : base(municipioTributacaoService, unityOfWork)
        {
            _municipioTributacaoService = municipioTributacaoService;
            _headerProvider = headerProvider;
        }


        public ResultDTO<MunicipioTributacaoAdicionarResponse> Adicionar(MunicipioTributacaoAdicionarRequest[] request)
        {
            var errors = new List<Error>();
            var idioma = _headerProvider.GetAcceptLanguage();

            // Verificar Erros
            for (int i = 0; i < request.Length; i++)
            {
                var ExternalIdExists = (_municipioTributacaoService.ObterPorExternalId(request[i].IdExterno) != null);
                if (ExternalIdExists)
                    errors.Add(new Error(
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetMessageCode(),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetResourceMessage(idioma, "external_id"),
                        i));

                var IbgeCodeExists = (_municipioTributacaoService.ObterPorIbgeCode(request[i].IbgeCodigo) != null);
                if (IbgeCodeExists)
                    errors.Add(new Error(
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetMessageCode(),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetResourceMessage(idioma, "ibge_code"),
                        i));

                var MunUfExists = (_municipioTributacaoService.ObterPorMunicipioUf(request[i].Nome, request[i].Uf) != null);
                if (MunUfExists)
                    errors.Add(new Error(
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetMessageCode(),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetResourceMessage(idioma, "municipality_name + uf"),
                        i));

                if (request[i].IntervPrefeituraRetencao != 0)
                {
                    if (!_municipioTributacaoService.VerificaSeExisteInterveniente(request[i].IntervPrefeituraRetencao))
                        errors.Add(new Error(
                            EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetMessageCode(),
                            EResourcesDefaultMessages.DEFAULT_MESSAGES_DOES_NOT_EXIST_IN_OUR_SYSTEM.GetResourceMessage(idioma, "municipal_tax_retention_intervener_code"),
                            i));
                }
            }

            if (errors.Count > 0)
                return new ResultDTO<MunicipioTributacaoAdicionarResponse>(
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
                        var newMunicipioTributacao = new Municipio()
                        {
                            Codigo = _municipioTributacaoService.ObterProximoCodigo()
                        };

                        newMunicipioTributacao = AutoMapper.Mapper.Map(cadastro, newMunicipioTributacao);

                        _municipioTributacaoService.Adicionar(newMunicipioTributacao);

                        Commit();
                    }

                    Commit();
                    scope.Complete();

                    var result = new MunicipioTributacaoAdicionarResponse(request.Length);

                    return new ResultDTO<MunicipioTributacaoAdicionarResponse>(EResultDTOStatus.Success, "Success when inserting records", result, "");
                }
                catch
                {
                    scope.Dispose();
                    var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                    var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                    return new ResultDTO<MunicipioTributacaoAdicionarResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
                }
            }
        }

        public ResultDTO<MunicipioTributacaoResponse> AtualizarPorId(int id, MunicipioTributacaoAtualizarRequest request)
        {
            var idioma = _headerProvider.GetAcceptLanguage();

            var municipioTributacao = _municipioTributacaoService.ObterPorId(id, true);
            if (municipioTributacao == null)
                return new ResultDTO<MunicipioTributacaoResponse>(
                        EResultDTOStatus.Error,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(idioma),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

            if (municipioTributacao.IdExterno != request.IdExterno && request.IdExterno != null)
            {
                var ExternalIdExists = (_municipioTributacaoService.ObterPorExternalId(request.IdExterno) != null);
                if (ExternalIdExists)
                    return new ResultDTO<MunicipioTributacaoResponse>(
                        EResultDTOStatus.Error,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetResourceMessage(idioma, "external_id"),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetMessageCode());
            }

            if (municipioTributacao.IbgeCodigo != request.IbgeCodigo && request.IbgeCodigo != null)
            {
                var IbgeCodeExists = (_municipioTributacaoService.ObterPorIbgeCode(request?.IbgeCodigo ?? 0) != null);
                if (IbgeCodeExists)
                    return new ResultDTO<MunicipioTributacaoResponse>(
                        EResultDTOStatus.Error,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetResourceMessage(idioma, "ibge_code"),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetMessageCode());
            }

            return Atualizar(municipioTributacao, request);
        }

        public ResultDTO<MunicipioTributacaoResponse> AtualizarPorExternalId(string externalId, MunicipioTributacaoAtualizarRequest request)
        {
            var idioma = _headerProvider.GetAcceptLanguage();

            var municipioTributacao = _municipioTributacaoService.ObterPorExternalId(externalId, true);
            if (municipioTributacao == null)
                return new ResultDTO<MunicipioTributacaoResponse>(
                    EResultDTOStatus.Error,
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(idioma),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

            if (municipioTributacao.IbgeCodigo != request.IbgeCodigo && request.IbgeCodigo != null)
            {
                var IbgeCodeExists = (_municipioTributacaoService.ObterPorIbgeCode(request?.IbgeCodigo ?? 0) != null);
                if (IbgeCodeExists)
                    return new ResultDTO<MunicipioTributacaoResponse>(
                         EResultDTOStatus.Error,
                         EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetResourceMessage(idioma, "ibge_code"),
                         EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetMessageCode());
            }

            return Atualizar(municipioTributacao, request);
        }

        public ResultDTO<MunicipioTributacaoResponse> AtualizarPorIbgeCode(int ibgeCode, MunicipioTributacaoAtualizarRequest request)
        {
            var idioma = _headerProvider.GetAcceptLanguage();

            var municipioTributacao = _municipioTributacaoService.ObterPorIbgeCode(ibgeCode, true);
            if (municipioTributacao == null)
                return new ResultDTO<MunicipioTributacaoResponse>(
                        EResultDTOStatus.Error,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(idioma),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

            if (municipioTributacao.IdExterno != request.IdExterno && request.IdExterno != null)
            {
                var ExternalIdExists = (_municipioTributacaoService.ObterPorExternalId(request.IdExterno) != null);
                if (ExternalIdExists)
                    return new ResultDTO<MunicipioTributacaoResponse>(
                       EResultDTOStatus.Error,
                       EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(idioma),
                       EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());
            }

            return Atualizar(municipioTributacao, request);
        }

        public ResultDTO<MunicipioTributacaoResponse> Atualizar(Municipio municipioTributacao, MunicipioTributacaoAtualizarRequest request)
        {
            var idioma = _headerProvider.GetAcceptLanguage();

            if ((municipioTributacao.Nome != request.Nome && request.Nome != null) || (municipioTributacao.Uf != request.Uf && request.Uf != null))
            {
                var MunUfExists = (_municipioTributacaoService.ObterPorMunicipioUf(request.Nome, request.Uf) != null);
                if (MunUfExists)
                    return new ResultDTO<MunicipioTributacaoResponse>(
                        EResultDTOStatus.Error,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetResourceMessage(idioma, "municipality_name + uf"),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetMessageCode());
            }

            if (request.IntervPrefeituraRetencao != 0 && request.IntervPrefeituraRetencao != null)
            {
                if (!_municipioTributacaoService.VerificaSeExisteInterveniente(request?.IntervPrefeituraRetencao ?? 0))
                    return new ResultDTO<MunicipioTributacaoResponse>(
                        EResultDTOStatus.Error,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(idioma),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());
            }

            using (var scope = new TransactionScope())
            {
                try
                {
                    municipioTributacao = AutoMapper.Mapper.Map(request, municipioTributacao);

                    Commit();
                    scope.Complete();

                    return new ResultDTO<MunicipioTributacaoResponse>(EResultDTOStatus.Success, "Successfully updated", AutoMapper.Mapper.Map(municipioTributacao, new MunicipioTributacaoResponse()));
                }
                catch
                {
                    scope.Dispose();
                    var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                    var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                    return new ResultDTO<MunicipioTributacaoResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
                }
            }
        }

        public ResultDTO<ICollection<MunicipioTributacaoResponse>> Listar(string uf)
        {
            var result = AutoMapper.Mapper.Map(_municipioTributacaoService.Listar(uf), new List<MunicipioTributacaoResponse>());

            if (result.Count == 0)
                return new ResultDTO<ICollection<MunicipioTributacaoResponse>>(
                    EResultDTOStatus.Error,
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

            return new ResultDTO<ICollection<MunicipioTributacaoResponse>>(EResultDTOStatus.Success, "", result);
        }

        public ResultDTO<MunicipioTributacaoResponse> ObterPorId(int id)
        {
            try
            {
                var result = AutoMapper.Mapper.Map(_municipioTributacaoService.ObterPorId(id), new MunicipioTributacaoResponse());

                if (result == null)
                    return new ResultDTO<MunicipioTributacaoResponse>(
                        EResultDTOStatus.Error,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

                return new ResultDTO<MunicipioTributacaoResponse>(EResultDTOStatus.Success, "", result);
            }
            catch (Exception)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<MunicipioTributacaoResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }

        public ResultDTO<MunicipioTributacaoResponse> ObterPorExternalId(string externalId)
        {
            try
            {
                var result = AutoMapper.Mapper.Map(_municipioTributacaoService.ObterPorExternalId(externalId), new MunicipioTributacaoResponse());

                if (result == null)
                    return new ResultDTO<MunicipioTributacaoResponse>(
                        EResultDTOStatus.Error,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

                return new ResultDTO<MunicipioTributacaoResponse>(EResultDTOStatus.Success, "", result);
            }
            catch (Exception)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<MunicipioTributacaoResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }

        public ResultDTO<MunicipioTributacaoResponse> ObterPorIbgeCode(int ibgeCode)
        {
            try
            {
                var result = AutoMapper.Mapper.Map(_municipioTributacaoService.ObterPorIbgeCode(ibgeCode), new MunicipioTributacaoResponse());

                if (result == null)
                    return new ResultDTO<MunicipioTributacaoResponse>(
                        EResultDTOStatus.Error,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

                return new ResultDTO<MunicipioTributacaoResponse>(EResultDTOStatus.Success, "", result);
            }
            catch (Exception)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<MunicipioTributacaoResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }
    }
}
