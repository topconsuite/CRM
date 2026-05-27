using System;
using System.Collections.Generic;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;
using TopSys.TopConWeb.Application.DTOS.Response.Filial;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;
using TopSys.TopConWeb.Domain.Interfaces.Integrations;

namespace TopSys.TopConWeb.Application
{
    public class FilialApplicationService : ApplicationServiceBase<Filial>, IFilialApplicationService
    {
        private readonly IFilialService _filialService;
        private readonly IHeaderProvider _headerProvider;

        public FilialApplicationService(IFilialService filialService, IUnitOfWork unitOfWork, IHeaderProvider headerProvider)
           : base(filialService, unitOfWork)
        {
            _filialService = filialService;
            _headerProvider = headerProvider;
        }

        public FilialResponse ObterPorId(int idFilial)
        {
            var filial = AutoMapper.Mapper.Map(_filialService.ObterPorId(idFilial), new FilialResponse());

            return filial;
        }

        public ICollection<FilialResponse> Listar()
        {
            var filiais = AutoMapper.Mapper.Map(_filialService.Listar(), new List<FilialResponse>());

            return filiais;
        }

        public ResultDTO<ICollection<FilialFiscalResponse>> FilialFiscalListar()
        {
            var result = AutoMapper.Mapper.Map(_filialService.Listar(), new List<FilialFiscalResponse>());

            if (result.Count == 0)
                return new ResultDTO<ICollection<FilialFiscalResponse>>(
                    EResultDTOStatus.Error,
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

            return new ResultDTO<ICollection<FilialFiscalResponse>>(EResultDTOStatus.Success, "", result);
        }

        public ResultDTO<FilialFiscalResponse> FilialFiscalObterPorId(int id)
        {
            try
            {
                var result = AutoMapper.Mapper.Map(_filialService.ObterPorId(id), new FilialFiscalResponse());

                if (result == null)
                    return new ResultDTO<FilialFiscalResponse>(
                        EResultDTOStatus.Error,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

                return new ResultDTO<FilialFiscalResponse>(EResultDTOStatus.Success, "", result);
            }
            catch (Exception)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<FilialFiscalResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }

        public ResultDTO<FilialFiscalResponse> FilialFiscalObterPorCentroCusto(int centroCusto)
        {
            try
            {
                var result = AutoMapper.Mapper.Map(_filialService.ObterPorCentroCusto(centroCusto), new FilialFiscalResponse());

                if (result == null)
                    return new ResultDTO<FilialFiscalResponse>(
                        EResultDTOStatus.Error,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

                return new ResultDTO<FilialFiscalResponse>(EResultDTOStatus.Success, "", result);
            }
            catch (Exception)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<FilialFiscalResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }
    }
}
