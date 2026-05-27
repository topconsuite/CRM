using System;
using System.Collections.Generic;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;
using TopSys.TopConWeb.Application.DTOS.Response.NotaFiscalDigital;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Interfaces.Integrations;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using Topsys.TopConWeb.SharedKernel.Common;

namespace TopSys.TopConWeb.Application
{
    public class NotaFiscalDigitalApplicationService : INotaFiscalDigitalApplicationService
    {
        private readonly INotaFiscalDigitalService _notaFiscalDigitalService;
        private readonly IHeaderProvider _headerProvider;

        public NotaFiscalDigitalApplicationService(INotaFiscalDigitalService notaFiscalDigitalService, IHeaderProvider headerProvider)
        {
            _notaFiscalDigitalService = notaFiscalDigitalService;
            _headerProvider = headerProvider;
        }

        public ResultDTO<NotaFiscalDigitalResponse> ObterPorChave(int filial, int cliente, int tipoDocumento, string serie, long numero, int sequence)
        {
            try
            {
                var notaFiscalDigital = _notaFiscalDigitalService.ObterPorChave(x => x.Filial == filial && x.Cliente == cliente 
                    && x.TipoDocumento == tipoDocumento && x.Serie == serie && x.Numero == numero && x.Sequencia == sequence);

                var result = AutoMapper.Mapper.Map(notaFiscalDigital, new NotaFiscalDigitalResponse());

                if (result == null)
                    return new ResultDTO<NotaFiscalDigitalResponse>(
                        EResultDTOStatus.Error,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

                return new ResultDTO<NotaFiscalDigitalResponse>(EResultDTOStatus.Success, "", result);
            }
            catch (Exception e)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<NotaFiscalDigitalResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }

        public ResultDTO<NotaFiscalDigitalResponse> ObterPorChaveSemInterveniente(int filial, int tipoDocumento, string serie, long numero, int sequence)
        {
            try
            {
                var notaFiscalDigital = _notaFiscalDigitalService.ObterPorChave(x => x.Filial == filial
                    && x.TipoDocumento == tipoDocumento && x.Serie == serie && x.Numero == numero);

                var result = AutoMapper.Mapper.Map(notaFiscalDigital, new NotaFiscalDigitalResponse());

                if (result == null)
                    return new ResultDTO<NotaFiscalDigitalResponse>(
                        EResultDTOStatus.Error,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

                return new ResultDTO<NotaFiscalDigitalResponse>(EResultDTOStatus.Success, "", result);
            }
            catch (Exception e)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<NotaFiscalDigitalResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }

        public ResultDTO<List<NotaFiscalDigitalResponse>> Listar(DateTime? dataNotaFiscal, int? filial, int? tipoDocumento, int? centroCusto, int? cliente, int? page, int? limit)
        {
            try
            {
                if (limit > 100) limit = 10;

                var notaFiscalDigital = _notaFiscalDigitalService.Listar(_notaFiscalDigitalService.CriarFiltroNotaFiscal(dataNotaFiscal, filial, tipoDocumento, centroCusto,  cliente), page ?? 1, limit ?? 10);

                var result = AutoMapper.Mapper.Map(notaFiscalDigital, new List<NotaFiscalDigitalResponse>());

                if (result.Count == 0)
                    return new ResultDTO<List<NotaFiscalDigitalResponse>>(
                        EResultDTOStatus.Error,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

                return new ResultDTO<List<NotaFiscalDigitalResponse>>(EResultDTOStatus.Success, "", result);
            }
            catch (Exception e)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<List<NotaFiscalDigitalResponse>>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }

        public ResultDTO<PagedList<NotaFiscalDigitalResponse>> ObterPorDataAtualizacao(DateTime dataInicio, DateTime? dataFim, int? page, int? limit)
        {
            try
            {
                if (limit > 100) limit = 10;

                var result = AutoMapper.Mapper.Map(_notaFiscalDigitalService.ObterPorDataAtualizacao(dataInicio, dataFim, page ?? 1, limit ?? 10), new PagedList<NotaFiscalDigitalResponse>());

                if (result.RecordCount == 0)
                    return new ResultDTO<PagedList<NotaFiscalDigitalResponse>>(
                        EResultDTOStatus.Error,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

                return new ResultDTO<PagedList<NotaFiscalDigitalResponse>>(EResultDTOStatus.Success, "", result);

            }
            catch (Exception e)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<PagedList<NotaFiscalDigitalResponse>>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }
    }
}
