using System;
using System.Collections.Generic;
using System.Linq;
using Topsys.TopConWeb.SharedKernel.Common;
using Topsys.TopConWeb.SharedKernel.Resources.EResourceMessages;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Application.DTOS.Response.Remessa;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Integrations;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Application
{
    public class RemessaApplicationService : IRemessaApplicationService
    {
        private readonly INotaFiscalFisicaService _notaFiscalFisicaService;
        private readonly IHeaderProvider _headerProvider;

        public RemessaApplicationService(INotaFiscalFisicaService notaFiscalFisicaService, IHeaderProvider headerProvider)
        {
            _notaFiscalFisicaService = notaFiscalFisicaService;
            _headerProvider = headerProvider;
        }

        public ResultDTO<List<RemessaResponse>> ObterPorProgramacao(int contratoUsina, int contratoAno, int contratoNumero, int programacaoSequencia)
        {

            try
            {

                var remessas = _notaFiscalFisicaService.ListarFiltrados(
                    x => x.ContratoUsinaCodigo == contratoUsina 
                         && x.ContratoAno == contratoAno 
                         && x.ContratoNumero == contratoNumero 
                         && x.ProgramacaoSequencia == programacaoSequencia
                         , y => y.Itens
                         , y => y.DemaisServicos);

                foreach (var remessa in remessas)
                {
                    var reaproveitamentos = _notaFiscalFisicaService.ListarFiltrados<Reaproveitamento>(x => x.FilialNotaDestino == remessa.FilialCodigo && x.IntervenienteNotaDestino == remessa.IntervenienteCodigo && x.TipoDocumentoNotaDestino == remessa.TipoDocumentoCodigo
                           && x.SerieNotaDestino == remessa.Serie && x.NumeroNotaDestino == remessa.Numero && x.SequenciaNotaDestino == remessa.Sequencia && x.UsinaNotaDestino == remessa.UsinaPesagem).ToList();

                    remessa.Reaproveitamentos = reaproveitamentos;

                    remessa.Complemento = _notaFiscalFisicaService.ObterComplemento(remessa.FilialCodigo, remessa.IntervenienteCodigo, remessa.TipoDocumentoCodigo, remessa.Serie, remessa.Numero, remessa.Sequencia);

                    foreach (var remessaItem in remessa.Itens)
                    {
                        remessaItem.Mercadoria = _notaFiscalFisicaService.ObterMercadoria(remessaItem.MercadoriaCodigo);
                    }

                    foreach (var remessaServico in remessa.DemaisServicos)
                    {
                        remessaServico.Mercadoria = _notaFiscalFisicaService.ObterMercadoria(remessaServico.MercadoriaCodigo);
                    }
                }

                var result = AutoMapper.Mapper.Map(remessas, new List<RemessaResponse>());

                foreach(var remessa in result)
                {
                    var contrato = _notaFiscalFisicaService.ObterPorId<Contrato>(contratoUsina, contratoAno, contratoNumero);

                    if(contrato != null)
                    {
                        remessa.ContratoFinalidade = (int)contrato.ContratoFinalidade;
                        remessa.ObservacaoContrato = contrato.Observacao;
                    }

                    var obra = _notaFiscalFisicaService.ListarFiltrados<Obra>(t => t.UsinaCodigo == remessa.ContratoUsinaCodigo && t.NumContrato == remessa.ContratoNumero && t.AnoContrato == remessa.ContratoAno).FirstOrDefault();
                    if (obra != null)
                        remessa.Cei = obra.Cei;
                }

                if (result.Count == 0)
                    return new ResultDTO<List<RemessaResponse>>(
                        EResultDTOStatus.Error,
                        EResourcesRemessa.REMESSA_ERROR_TCON393731.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesRemessa.REMESSA_ERROR_TCON393731.GetMessageCode());

                return new ResultDTO<List<RemessaResponse>>(EResultDTOStatus.Success, "", result);

            }
            catch (Exception e)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<List<RemessaResponse>>(EResultDTOStatus.Error, errorMessage, errorCode);
            }

        }

        public ResultDTO<List<RemessaResponse>> ObterPorCentralEPeriodo(int filial, DateTime dataInicio, DateTime? dataFim, int page, int limit)
        {
            try
            {
                var remessas = dataFim is null
                    ? _notaFiscalFisicaService.ListarFiltradosPaginado(x => x.FilialCodigo == filial && x.DataRemessa >= dataInicio && x.ContratoUsinaCodigo != 0, page, limit, x => x.Itens, x => x.DemaisServicos)
                    : _notaFiscalFisicaService.ListarFiltradosPaginado(x => x.FilialCodigo == filial && x.DataRemessa >= dataInicio && x.DataRemessa <= dataFim && x.ContratoUsinaCodigo != 0, page, limit, x => x.Itens, x => x.DemaisServicos);

                foreach (var remessa in remessas)
                {
                    var reaproveitamentos = _notaFiscalFisicaService.ListarFiltrados<Reaproveitamento>(x => x.FilialNotaDestino == filial && x.IntervenienteNotaDestino == remessa.IntervenienteCodigo && x.TipoDocumentoNotaDestino == remessa.TipoDocumentoCodigo
                           && x.SerieNotaDestino == remessa.Serie && x.NumeroNotaDestino == remessa.Numero && x.SequenciaNotaDestino == remessa.Sequencia && x.UsinaNotaDestino == remessa.UsinaPesagem).ToList();

                    remessa.Reaproveitamentos = reaproveitamentos;

                    remessa.Complemento = _notaFiscalFisicaService.ObterComplemento(remessa.FilialCodigo, remessa.IntervenienteCodigo, remessa.TipoDocumentoCodigo, remessa.Serie, remessa.Numero, remessa.Sequencia);
                    
                    foreach (var remessaItem in remessa.Itens)
                    {
                        remessaItem.Mercadoria = _notaFiscalFisicaService.ObterMercadoria(remessaItem.MercadoriaCodigo);
                    }

                    foreach (var remessaServico in remessa.DemaisServicos)
                    {
                        remessaServico.Mercadoria = _notaFiscalFisicaService.ObterMercadoria(remessaServico.MercadoriaCodigo);
                    }
                }

                var result = AutoMapper.Mapper.Map(remessas, new List<RemessaResponse>());

                foreach (var remessa in result)
                {
                    var contrato = _notaFiscalFisicaService.ObterPorId<Contrato>(remessa.ContratoUsinaCodigo, remessa.ContratoAno, remessa.ContratoNumero);

                    if (contrato != null)
                    {
                        remessa.ContratoFinalidade = (int)contrato.ContratoFinalidade;
                        remessa.ObservacaoContrato = contrato.Observacao;
                    }

                    var obra = _notaFiscalFisicaService.ListarFiltrados<Obra>(t => t.UsinaCodigo == remessa.ContratoUsinaCodigo && t.NumContrato == remessa.ContratoNumero && t.AnoContrato == remessa.ContratoAno).FirstOrDefault();
                    if (obra != null)
                        remessa.Cei = obra.Cei;
                }

                if (result.Count == 0)
                    return new ResultDTO<List<RemessaResponse>>(
                        EResultDTOStatus.Error,
                        EResourcesRemessa.REMESSA_ERROR_TCON393731.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesRemessa.REMESSA_ERROR_TCON393731.GetMessageCode());

                return new ResultDTO<List<RemessaResponse>>(EResultDTOStatus.Success, "", result);
            }
            catch (Exception e)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<List<RemessaResponse>>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }

        public ResultDTO<RemessaResponse> ObterPorId(int filial, int interveniente, int tipoDocumento, string serie, long numero, int sequencia)
        {
            try {
                var remessa = _notaFiscalFisicaService.ListarFiltrados(x => x.FilialCodigo == filial && x.IntervenienteCodigo == interveniente
                    && x.TipoDocumentoCodigo == tipoDocumento && x.Serie == serie && x.Numero == numero && x.Sequencia == sequencia && x.ContratoUsinaCodigo != 0, x => x.Itens, x => x.DemaisServicos).FirstOrDefault();
                
                if (remessa == null)
                    return new ResultDTO<RemessaResponse>(
                        EResultDTOStatus.Error,
                        EResourcesRemessa.REMESSA_ERROR_TCON393731.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesRemessa.REMESSA_ERROR_TCON393731.GetMessageCode());

                var reaproveitamentos = _notaFiscalFisicaService.ListarFiltrados<Reaproveitamento>(x => x.FilialNotaDestino == filial && x.IntervenienteNotaDestino == interveniente && x.TipoDocumentoNotaDestino == tipoDocumento
                                           && x.SerieNotaDestino == serie && x.NumeroNotaDestino == numero && x.SequenciaNotaDestino == sequencia && x.UsinaNotaDestino == remessa.UsinaPesagem).ToList();

                remessa.Reaproveitamentos = reaproveitamentos;

                var complemento = _notaFiscalFisicaService.ObterComplemento(filial, interveniente, tipoDocumento, serie, numero, sequencia);

                if (complemento == null)
                    return new ResultDTO<RemessaResponse>(
                        EResultDTOStatus.Error,
                        EResourcesRemessa.REMESSA_ERROR_TCON393731.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesRemessa.REMESSA_ERROR_TCON393731.GetMessageCode());
                
                foreach (var remessaItem in remessa.Itens)
                {
                    remessaItem.Mercadoria = _notaFiscalFisicaService.ObterMercadoria(remessaItem.MercadoriaCodigo);
                }

                foreach (var remessaServico in remessa.DemaisServicos)
                {
                    remessaServico.Mercadoria = _notaFiscalFisicaService.ObterMercadoria(remessaServico.MercadoriaCodigo);
                }

                remessa.Complemento = complemento;

                var result = AutoMapper.Mapper.Map(remessa, new RemessaResponse());

                var contrato = _notaFiscalFisicaService.ObterPorId<Contrato>(remessa.ContratoUsinaCodigo, remessa.ContratoAno, remessa.ContratoNumero);

                if (contrato != null)
                {
                    result.ContratoFinalidade = (int)contrato.ContratoFinalidade;
                    result.ObservacaoContrato = contrato.Observacao;
                }

                var obra = _notaFiscalFisicaService.ListarFiltrados<Obra>(t => t.UsinaCodigo == remessa.ContratoUsinaCodigo && t.NumContrato == remessa.ContratoNumero && t.AnoContrato == remessa.ContratoAno).FirstOrDefault();
                if (obra != null)
                    result.Cei = obra.Cei;

                if (result == null)
                    return new ResultDTO<RemessaResponse>(
                        EResultDTOStatus.Error,
                        EResourcesRemessa.REMESSA_ERROR_TCON393731.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesRemessa.REMESSA_ERROR_TCON393731.GetMessageCode());

                return new ResultDTO<RemessaResponse>(EResultDTOStatus.Success, "", result);
            }
            catch (Exception e)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<RemessaResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }

        public ResultDTO<RemessaResponse> ObterPorIdSemInterveniente(int filial, int tipoDocumento, string serie, long numero, int sequencia)
        {
            try
            {
                var remessa = _notaFiscalFisicaService.ListarFiltrados(x => x.FilialCodigo == filial
                    && x.TipoDocumentoCodigo == tipoDocumento && x.Serie == serie && x.Numero == numero && x.Sequencia == sequencia && x.ContratoUsinaCodigo != 0, x => x.Itens, x => x.DemaisServicos).FirstOrDefault();

                if (remessa == null)
                    return new ResultDTO<RemessaResponse>(
                        EResultDTOStatus.Error,
                        EResourcesRemessa.REMESSA_ERROR_TCON393731.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesRemessa.REMESSA_ERROR_TCON393731.GetMessageCode());

                var reaproveitamentos = _notaFiscalFisicaService.ListarFiltrados<Reaproveitamento>(x => x.FilialNotaDestino == filial && x.TipoDocumentoNotaDestino == tipoDocumento
                           && x.SerieNotaDestino == serie && x.NumeroNotaDestino == numero && x.SequenciaNotaDestino == sequencia && x.UsinaNotaDestino == remessa.UsinaPesagem).ToList();

                remessa.Reaproveitamentos = reaproveitamentos;

                var complemento = _notaFiscalFisicaService.ObterComplemento(filial, remessa.IntervenienteCodigo, tipoDocumento, serie, numero, sequencia);

                if (complemento == null)
                    return new ResultDTO<RemessaResponse>(
                        EResultDTOStatus.Error,
                        EResourcesRemessa.REMESSA_ERROR_TCON393731.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesRemessa.REMESSA_ERROR_TCON393731.GetMessageCode());
                
                foreach (var remessaItem in remessa.Itens)
                {
                    remessaItem.Mercadoria = _notaFiscalFisicaService.ObterMercadoria(remessaItem.MercadoriaCodigo);
                }

                foreach (var remessaServico in remessa.DemaisServicos)
                {
                    remessaServico.Mercadoria = _notaFiscalFisicaService.ObterMercadoria(remessaServico.MercadoriaCodigo);
                }

                remessa.Complemento = complemento;

                var result = AutoMapper.Mapper.Map(remessa, new RemessaResponse());

                var contrato = _notaFiscalFisicaService.ObterPorId<Contrato>(remessa.ContratoUsinaCodigo, remessa.ContratoAno, remessa.ContratoNumero);

                if (contrato != null)
                {
                    result.ContratoFinalidade = (int)contrato.ContratoFinalidade;
                    result.ObservacaoContrato = contrato.Observacao;
                }

                var obra = _notaFiscalFisicaService.ListarFiltrados<Obra>(t => t.UsinaCodigo == remessa.ContratoUsinaCodigo && t.NumContrato == remessa.ContratoNumero && t.AnoContrato == remessa.ContratoAno).FirstOrDefault();
                if (obra != null)
                    result.Cei = obra.Cei;

                if (result == null)
                    return new ResultDTO<RemessaResponse>(
                        EResultDTOStatus.Error,
                        EResourcesRemessa.REMESSA_ERROR_TCON393731.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesRemessa.REMESSA_ERROR_TCON393731.GetMessageCode());

                return new ResultDTO<RemessaResponse>(EResultDTOStatus.Success, "", result);
            }
            catch (Exception e)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<RemessaResponse>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }

        public ResultDTO<PagedList<RemessaResponse>> ObterPorDataAtualizacao(DateTime dataInicio, DateTime? dataFim, int page, int limit)
        {
            try
            {
                if (limit > 100) limit = 10;

                var result = AutoMapper.Mapper.Map(_notaFiscalFisicaService.ObterPorDataAtualizacao(dataInicio, dataFim, page, limit), new PagedList<RemessaResponse>());

                if (result.RecordCount == 0)
                    return new ResultDTO<PagedList<RemessaResponse>>(
                        EResultDTOStatus.Error,
                        EResourcesRemessa.REMESSA_ERROR_TCON393731.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesRemessa.REMESSA_ERROR_TCON393731.GetMessageCode());

                foreach (var remessa in result.Records)
                {
                    var contrato = _notaFiscalFisicaService.ObterPorId<Contrato>(remessa.ContratoUsinaCodigo, remessa.ContratoAno, remessa.ContratoNumero);

                    if (contrato != null)
                    {
                        remessa.ContratoFinalidade = (int)contrato.ContratoFinalidade;
                        remessa.ObservacaoContrato = contrato.Observacao;
                    }

                    var obra = _notaFiscalFisicaService.ListarFiltrados<Obra>(t => t.UsinaCodigo == remessa.ContratoUsinaCodigo && t.NumContrato == remessa.ContratoNumero && t.AnoContrato == remessa.ContratoAno).FirstOrDefault();
                    if (obra != null)
                        remessa.Cei = obra.Cei;
                }

                return new ResultDTO<PagedList<RemessaResponse>>(EResultDTOStatus.Success, "", result);

            }
            catch (Exception)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<PagedList<RemessaResponse>>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }

        public ResultDTO<PagedList<RemessaResponse>> ObterPorDataRetornoAutomacao(DateTime dataInicio, DateTime? dataFim, int page, int limit)
        {
            try
            {
                if (limit > 100) limit = 10;

                var result = AutoMapper.Mapper.Map(_notaFiscalFisicaService.ObterPorDataRetornoAutomacao(dataInicio, dataFim, page, limit), new PagedList<RemessaResponse>());

                if (result.RecordCount == 0)
                    return new ResultDTO<PagedList<RemessaResponse>>(
                        EResultDTOStatus.Error,
                        EResourcesRemessa.REMESSA_ERROR_TCON393731.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesRemessa.REMESSA_ERROR_TCON393731.GetMessageCode());

                foreach (var remessa in result.Records)
                {
                    var contrato = _notaFiscalFisicaService.ObterPorId<Contrato>(remessa.ContratoUsinaCodigo, remessa.ContratoAno, remessa.ContratoNumero);

                    if (contrato != null)
                    {
                        remessa.ContratoFinalidade = (int)contrato.ContratoFinalidade;
                        remessa.ObservacaoContrato = contrato.Observacao;
                    }

                    var obra = _notaFiscalFisicaService.ListarFiltrados<Obra>(t => t.UsinaCodigo == remessa.ContratoUsinaCodigo && t.NumContrato == remessa.ContratoNumero && t.AnoContrato == remessa.ContratoAno).FirstOrDefault();
                    if (obra != null)
                        remessa.Cei = obra.Cei;
                }

                return new ResultDTO<PagedList<RemessaResponse>>(EResultDTOStatus.Success, "", result);

            }
            catch (Exception)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<PagedList<RemessaResponse>>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }

        public ResultDTO<PagedList<RemessaPontosResponse>> ObterIndicadorPontos(DateTime? dataInicio, DateTime? dataFim, int vendedor, string indicadorNome, int page, int limit)
        {
            try
            {
                if (limit > 100) limit = 10;

                var result = AutoMapper.Mapper.Map(_notaFiscalFisicaService.ObterIndicadorPontos(dataInicio, dataFim, vendedor, indicadorNome, page, limit), new PagedList<RemessaPontosResponse>());

                if (result.RecordCount == 0)
                    return new ResultDTO<PagedList<RemessaPontosResponse>>(
                        EResultDTOStatus.Error,
                        EResourcesRemessa.REMESSA_ERROR_TCON393731.GetResourceMessage(_headerProvider.GetAcceptLanguage()),
                        EResourcesRemessa.REMESSA_ERROR_TCON393731.GetMessageCode());

                return new ResultDTO<PagedList<RemessaPontosResponse>>(EResultDTOStatus.Success, "", result);

            }
            catch (Exception e)
            {
                var errorMessage = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetResourceMessage(_headerProvider.GetAcceptLanguage());
                var errorCode = EResourcesDefaultMessages.DEFAULT_MESSAGES_INTERNAL_ENDPOINT_ERROR.GetMessageCode();
                return new ResultDTO<PagedList<RemessaPontosResponse>>(EResultDTOStatus.Error, errorMessage, errorCode);
            }
        }
    }
}
