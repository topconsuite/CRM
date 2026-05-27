using System.Collections.Generic;
using System.Transactions;
using TopSys.TopConWeb.Application.DTOS.Request.TipoDocumento;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Application.DTOS.Response.TipoDocumento;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Integrations;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;

namespace TopSys.TopConWeb.Application
{
    public class TipoDocumentoApplicationService : ApplicationServiceBase<TipoDocumento>,
        ITipoDocumentoApplicationService
    {
        private readonly ITipoDocumentoService _tipoDocumentoService;
        private readonly IHeaderProvider _headerProvider;

        public TipoDocumentoApplicationService(ITipoDocumentoService tipoDocumentoService, IUnitOfWork unityOfWork,
            IHeaderProvider headerProvider) : base(tipoDocumentoService, unityOfWork)
        {
            _tipoDocumentoService = tipoDocumentoService;
            _headerProvider = headerProvider;
        }

        public ResultDTO<TipoDocumentoAdicionarResponse> Adicionar(TipoDocumentoAdicionarRequest[] request)
        {
            var errors = new List<Error>();
            var idioma = _headerProvider.GetAcceptLanguage();

            // Verificar Erros
            for (int i = 0; i < request.Length; i++)
            {
                var codeExists = (_tipoDocumentoService.ObterPorId(request[i].Codigo) != null);
                if (codeExists)
                    errors.Add(new Error(
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetMessageCode(),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_ALREADY_EXIST_IN_OUR_SISTEM.GetResourceMessage(
                            idioma, "Code"),
                        i));
            }

            if (errors.Count > 0)
                return new ResultDTO<TipoDocumentoAdicionarResponse>(
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
                        var newTipoDocumento = new TipoDocumento();

                        newTipoDocumento = AutoMapper.Mapper.Map(cadastro, newTipoDocumento);

                        _tipoDocumentoService.Adicionar(newTipoDocumento);
                    }

                    Commit();
                    scope.Complete();

                    var result = new TipoDocumentoAdicionarResponse(request.Length);
                    return new ResultDTO<TipoDocumentoAdicionarResponse>(EResultDTOStatus.Success,
                        "Success when inserting records", result, "");
                }
                catch
                {
                    scope.Dispose();
                    var errorMessage =
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(
                            _headerProvider.GetAcceptLanguage());
                    var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                    return new ResultDTO<TipoDocumentoAdicionarResponse>(EResultDTOStatus.Error, errorMessage,
                        errorCode);
                }
            }
        }

        public ResultDTO<TipoDocumentoResponse> AtualizarPorId(int id, TipoDocumentoAtualizarRequest request)
        {
            var tipoDocumento = _tipoDocumentoService.ObterPorId(id, true);
            if (tipoDocumento == null)
                return new ResultDTO<TipoDocumentoResponse>(
                    EResultDTOStatus.Error,
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(
                        _headerProvider.GetAcceptLanguage(),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode()));

            return Atualizar(tipoDocumento, request);
        }

        public ResultDTO<TipoDocumentoResponse> Atualizar(TipoDocumento tipoDocumento,
            TipoDocumentoAtualizarRequest request)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    tipoDocumento = AutoMapper.Mapper.Map(request, tipoDocumento);

                    Commit();
                    scope.Complete();

                    return new ResultDTO<TipoDocumentoResponse>(EResultDTOStatus.Success, "Successfully updated",
                        AutoMapper.Mapper.Map(tipoDocumento, new TipoDocumentoResponse()));
                }
                catch
                {
                    scope.Dispose();
                    var errorMessage =
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(
                            _headerProvider.GetAcceptLanguage());
                    var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                    return new ResultDTO<TipoDocumentoResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
                }
            }
        }

        public ResultDTO<ICollection<TipoDocumentoResponse>> Listar()
        {
            var result = AutoMapper.Mapper.Map(_tipoDocumentoService.Listar(), new List<TipoDocumentoResponse>());

            if (result.Count == 0)
                return new ResultDTO<ICollection<TipoDocumentoResponse>>(EResultDTOStatus.Error,
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(
                        _headerProvider.GetAcceptLanguage()),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

            return new ResultDTO<ICollection<TipoDocumentoResponse>>(EResultDTOStatus.Success, "", result);
        }

        public ResultDTO<TipoDocumentoResponse> ObterPorId(int id)
        {
            try
            {
                var result = AutoMapper.Mapper.Map(_tipoDocumentoService.ObterPorId(id), new TipoDocumentoResponse());

                if (result == null)
                    return new ResultDTO<TipoDocumentoResponse>(
                        EResultDTOStatus.Error,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(
                            _headerProvider.GetAcceptLanguage(),
                            EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode()));

                return new ResultDTO<TipoDocumentoResponse>(EResultDTOStatus.Success, "", result);
            }
            catch
            {
                var errorMessage =
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(
                        _headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<TipoDocumentoResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }
    }
}