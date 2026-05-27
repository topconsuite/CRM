using System;
using System.Transactions;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;
using TopSys.TopConWeb.Application.DTOS.Request.Prensa;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Application.DTOS.Response.Prensa;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Integrations;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;

namespace TopSys.TopConWeb.Application
{
    public class PrensaApplicationService : ApplicationServiceBase<Prensa>, IPrensaApplicationService
    {
        private readonly IPrensaService _prensaService;
        private readonly IHeaderProvider _headerProvider;

        public PrensaApplicationService(IPrensaService prensaService, IUnitOfWork unityOfWork, IHeaderProvider headerProvider)
            : base(prensaService, unityOfWork)
        {
            _prensaService = prensaService;
            _headerProvider = headerProvider;
        }

        public ResultDTO<PrensaResponse> PrensaAdicionar(PrensaRequest request)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    var newPrensa = new Prensa();
                    newPrensa = AutoMapper.Mapper.Map(request, newPrensa);

                    var prensa = _prensaService.ObterPorId(newPrensa.PrensaNome);
                    if (prensa != null)
                        _prensaService.Remover(prensa);

                    _prensaService.Adicionar(newPrensa);

                    Commit();
                    scope.Complete();

                    var result = new PrensaResponse();
                    return new ResultDTO<PrensaResponse>(EResultDTOStatus.Success, "Success when inserting record", result, "");
                }
                catch
                {
                    scope.Dispose();
                    var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                    var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                    return new ResultDTO<PrensaResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
                }
            }
        }
    }
}
