using System;
using System.Collections.Generic;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Application.DTOS.Request.Remessa;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Application.DTOS.Response.Remessa;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IRemessaApplicationService
    {
        ResultDTO<RemessaResponse> ObterPorId(int filial, int interveniente, int tipoDocumento, string serie, long numero, int sequencia);
        ResultDTO<RemessaResponse> ObterPorIdSemInterveniente(int filial, int tipoDocumento, string serie, long numero, int sequencia);
        ResultDTO<List<RemessaResponse>> ObterPorCentralEPeriodo(int filial, DateTime dataInicio, DateTime? dataFim, int page, int limit);
        ResultDTO<List<RemessaResponse>> ObterPorProgramacao(int contratoUsina, int contratoAno, int contratoNumero, int programacaoSequencia);
        ResultDTO<PagedList<RemessaResponse>> ObterPorDataAtualizacao(DateTime dataInicio, DateTime? dataFim, int page, int limit);
        ResultDTO<PagedList<RemessaResponse>> ObterPorDataRetornoAutomacao(DateTime dataInicio, DateTime? dataFim, int page, int limit);
        ResultDTO<PagedList<RemessaPontosResponse>> ObterIndicadorPontos(DateTime? dataInicio, DateTime? dataFim, int vendedor, string indicadorNome, int page, int limit);
    }
}
