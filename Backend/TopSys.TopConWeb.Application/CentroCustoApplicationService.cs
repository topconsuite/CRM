using System;
using System.Collections.Generic;
using System.Transactions;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;
using TopSys.TopConWeb.Application.DTOS.Request.CentroCusto;
using TopSys.TopConWeb.Application.DTOS.Response.CentroCusto;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Integrations;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;

namespace TopSys.TopConWeb.Application
{
    public class CentroCustoApplicationService : ApplicationServiceBase<CentroCusto>, ICentroCustoApplicationService
    {
        private readonly ICentroCustoService _centroCustoService;
        private readonly IHeaderProvider _headerProvider;

        public CentroCustoApplicationService(ICentroCustoService centroCustoService, IUnitOfWork unityOfWork, IHeaderProvider headerProvider)
            : base(centroCustoService, unityOfWork)
        {
            _centroCustoService = centroCustoService;
            _headerProvider = headerProvider;
        }

        public ResultDTO<CentroCustoAdicionarResponse> Adicionar(CentroCustoAdicionarRequest[] request)
        {
            var errors = new List<Error>();
            var idioma = _headerProvider.GetAcceptLanguage();

            // Verificar Erros
            for (int i = 0; i < request.Length; i++)
            {
                var codeExists = (_centroCustoService.ObterPorId(request[i].Codigo) != null);
                if (codeExists)
                    errors.Add(new Error(
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetMessageCode(),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetResourceMessage(idioma, "Code"),
                        i));
            }

            if (errors.Count > 0)
                return new ResultDTO<CentroCustoAdicionarResponse>(
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
                        var newCentroCusto = new CentroCusto();

                        newCentroCusto = AutoMapper.Mapper.Map(cadastro, newCentroCusto);

                        _centroCustoService.Adicionar(newCentroCusto);
                    }

                    Commit();
                    scope.Complete();

                    var result = new CentroCustoAdicionarResponse(request.Length);

                    return new ResultDTO<CentroCustoAdicionarResponse>(EResultDTOStatus.Success, "Success when inserting records", result, "");
                }
                catch
                {
                    scope.Dispose();
                    var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                    var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                    return new ResultDTO<CentroCustoAdicionarResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
                }
            }
        }

        public ResultDTO<CentroCustoResponse> AtualizarPorId(int id, CentroCustoAtualizarRequest request)
        {
            var centroCusto = _centroCustoService.ObterPorId(id, true);
            if (centroCusto == null)
                return new ResultDTO<CentroCustoResponse>(
                    EResultDTOStatus.Error,
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

            return Atualizar(centroCusto, request);
        }

        public ResultDTO<CentroCustoResponse> Atualizar(CentroCusto centroCusto, CentroCustoAtualizarRequest request)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    centroCusto = AutoMapper.Mapper.Map(request, centroCusto);

                    Commit();   
                    scope.Complete();

                    return new ResultDTO<CentroCustoResponse>(EResultDTOStatus.Success, "Successfully updated", AutoMapper.Mapper.Map(centroCusto, new CentroCustoResponse()));
                }
                catch
                {
                    scope.Dispose();
                    var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                    var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                    return new ResultDTO<CentroCustoResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
                }
            }
        }

        public ResultDTO<ICollection<CentroCustoResponse>> Listar()
        {
            var result = AutoMapper.Mapper.Map(_centroCustoService.Listar(), new List<CentroCustoResponse>());

            if (result.Count == 0)
                return new ResultDTO<ICollection<CentroCustoResponse>>(
                    EResultDTOStatus.Error,
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

            return new ResultDTO<ICollection<CentroCustoResponse>>(EResultDTOStatus.Success, "", result);
        }

        public ResultDTO<CentroCustoResponse> ObterPorId(int id)
        {
            try
            {
                var result = AutoMapper.Mapper.Map(_centroCustoService.ObterPorId(id), new CentroCustoResponse());

                if (result == null)
                    return new ResultDTO<CentroCustoResponse>(
                        EResultDTOStatus.Error,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

                return new ResultDTO<CentroCustoResponse>(EResultDTOStatus.Success, "", result);
            }
            catch (Exception)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<CentroCustoResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }

        public ResultDTO<int> DeletarPorId(int id)
        {
            var centroCusto = _centroCustoService.ObterPorId(id, true);
            if (centroCusto == null)
                return new ResultDTO<int>(
                    EResultDTOStatus.Error,
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

            return Deletar(centroCusto);
        }

        public ResultDTO<int> Deletar(CentroCusto centroCusto)
        {
            try
            {
                var estaEmUso = _centroCustoService.EstaEmUsoCentroCusto(centroCusto.Codigo);
                if (estaEmUso)
                    return new ResultDTO<int>(
                        EResultDTOStatus.Error,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_CANT_BE_DELETED_SINCE_ITS_IN_USE.GetResourceMessage(_headerProvider.GetAcceptLanguage(), "cost_center"),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_CANT_BE_DELETED_SINCE_ITS_IN_USE.GetMessageCode());

                _centroCustoService.Remover(centroCusto);

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
