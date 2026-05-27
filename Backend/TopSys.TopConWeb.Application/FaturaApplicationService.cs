using System;
using System.Collections.Generic;
using System.Transactions;
using TopSys.TopConWeb.Application.DTOS.Request.Fatura;
using Topsys.TopConWeb.SharedKernel.Common;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;
using TopSys.TopConWeb.Application.DTOS.Response.Fatura;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Application.DTOS.Response.NotaFiscalDigital;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Integrations;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Domain.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;

namespace TopSys.TopConWeb.Application
{
    public class FaturaApplicationService : ApplicationServiceBase<Fatura>,
        IFaturaApplicationService
    {
        private readonly IFaturaService _faturaService;
        private readonly IHeaderProvider _headerProvider;

        public FaturaApplicationService(IFaturaService faturaService, IUnitOfWork unityOfWork,
            IHeaderProvider headerProvider) : base(faturaService, unityOfWork)
        {
            _faturaService = faturaService;
            _headerProvider = headerProvider;
        }

        public ResultDTO<FaturaResponse> ObterPorChave(int filial, int cliente, int tipoDocumento, long numero, string serie, int subSerie)
        {
            try
            {
                var fatura = _faturaService.ObterPorChave(x => x.Filial == filial && x.Cliente == cliente 
                    && x.TipoDocumento == tipoDocumento && x.Numero == numero && x.Serie == serie && x.SubSerie == subSerie);

                var result = AutoMapper.Mapper.Map(fatura, new FaturaResponse());

                if (result == null)
                    return new ResultDTO<FaturaResponse>(
                        EResultDTOStatus.Error,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

                return new ResultDTO<FaturaResponse>(EResultDTOStatus.Success, "", result);
            }
            catch (Exception e)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<FaturaResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }

        public ResultDTO<List<FaturaResponse>> Listar(DateTime? dataFatura, int? filial, int? centroCusto, int? tipoDocumento, int? cliente, int? page, int? limit)
        {
            try
            {
                if (limit > 100) limit = 10;

                var faturas = _faturaService.Listar(_faturaService.CriarFiltroFatura(dataFatura, filial, centroCusto, tipoDocumento, cliente), page ?? 1, limit ?? 10);

                var result = AutoMapper.Mapper.Map(faturas, new List<FaturaResponse>());

                if (result.Count == 0)
                    return new ResultDTO<List<FaturaResponse>>(
                        EResultDTOStatus.Error,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

                return new ResultDTO<List<FaturaResponse>>(EResultDTOStatus.Success, "", result);
            }
            catch (Exception e)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<List<FaturaResponse>>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }

        public ResultDTO<PagedList<FaturaResponse>> ObterPorDataAtualizacao(DateTime dataInicio, DateTime? dataFim, int? page, int? limit)
        {
            try
            {
                if (limit > 100) limit = 10;

                var result = AutoMapper.Mapper.Map(_faturaService.ObterPorDataAtualizacao(dataInicio, dataFim, page ?? 1, limit ?? 10), new PagedList<FaturaResponse>());

                if (result.RecordCount == 0)
                    return new ResultDTO<PagedList<FaturaResponse>>(
                        EResultDTOStatus.Error,
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode());

                return new ResultDTO<PagedList<FaturaResponse>>(EResultDTOStatus.Success, "", result);

            }
            catch (Exception)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<PagedList<FaturaResponse>>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }

        public ResultDTO<FaturaResponse> FaturaAtualizarPorChave(int filial, int cliente, int tipoDocumento, long numero, string serie, int subSerie, FaturaAtualizarRequest request)
        {
            var fatura = _faturaService.ObterPorChave(x => x.Filial == filial && x.Cliente == cliente && x.TipoDocumento == tipoDocumento && x.Numero == numero && x.Serie == serie && x.SubSerie == subSerie, true);
            if (fatura == null)
                return new ResultDTO<FaturaResponse>(
                    EResultDTOStatus.Error,
                    EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetResourceMessage(
                        _headerProvider.GetAcceptLanguage(),
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_NO_RECORDS_FOUND.GetMessageCode()));

            return Atualizar(fatura, request);
        }

        private ResultDTO<FaturaResponse> Atualizar(Fatura fatura, FaturaAtualizarRequest request)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    fatura = AutoMapper.Mapper.Map(request, fatura);
                    
                    foreach (var item in fatura.Itens)
                    {
                        item.NumeroRps = fatura.NumeroRps;
                        item.NumeroNfse = fatura.NumeroNfse;
                    }

                    Commit();
                    scope.Complete();

                    return new ResultDTO<FaturaResponse>(EResultDTOStatus.Success, "Successfully updated",
                        AutoMapper.Mapper.Map(fatura, new FaturaResponse()));
                }
                catch
                {
                    scope.Dispose();
                    var errorMessage =
                        EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(
                            _headerProvider.GetAcceptLanguage());
                    var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                    return new ResultDTO<FaturaResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
                }
            }
        }
    }
}
