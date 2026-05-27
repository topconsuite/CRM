using System;
using System.Collections.Generic;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;
using TopSys.TopConWeb.Application.DTOS.Response.Empresa;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Integrations;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;

namespace TopSys.TopConWeb.Application
{
    public class EmpresaApplicationService : ApplicationServiceBase<Empresa>, IEmpresaApplicationService
    {
        private readonly IEmpresaService _empresaService;
        private readonly IHeaderProvider _headerProvider;

        public EmpresaApplicationService(IEmpresaService empresaService, IUnitOfWork unityOfWork, IHeaderProvider headerProvider)
            : base(empresaService, unityOfWork)
        {
            _empresaService = empresaService;
            _headerProvider = headerProvider;
        }

        public ResultDTO<ICollection<EmpresaResponse>> Listar()
        {
            var result = AutoMapper.Mapper.Map(_empresaService.Listar(), new List<EmpresaResponse>());

            if (result.Count == 0)
                return new ResultDTO<ICollection<EmpresaResponse>>(
                    EResultDTOStatus.Error,
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

            return new ResultDTO<ICollection<EmpresaResponse>>(EResultDTOStatus.Success, "", result);
        }

        public ResultDTO<EmpresaResponse> ObterPorId(int id)
        {
            try
            {
                var result = AutoMapper.Mapper.Map(_empresaService.ObterPorId(id), new EmpresaResponse());

                if (result == null)
                    return new ResultDTO<EmpresaResponse>(
                        EResultDTOStatus.Error,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

                return new ResultDTO<EmpresaResponse>(EResultDTOStatus.Success, "", result);
            }
            catch (Exception)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<EmpresaResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }
    }
}
