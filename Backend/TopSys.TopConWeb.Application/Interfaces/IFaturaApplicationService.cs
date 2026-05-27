using System;
using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Request.Fatura;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Application.DTOS.Response.Fatura;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IFaturaApplicationService
    {
        ResultDTO<FaturaResponse> ObterPorChave(int filial, int cliente, int tipoDocumento, long numero, string serie, int subSerie);
        ResultDTO<List<FaturaResponse>> Listar(DateTime? dataFatura, int? filial, int?  centroCusto, int? tipoDocumento, int? cliente, int? page, int? limit);
        ResultDTO<PagedList<FaturaResponse>> ObterPorDataAtualizacao(DateTime dataInicio, DateTime? dataFim, int? page, int? limit);
        ResultDTO<FaturaResponse> FaturaAtualizarPorChave(int filial, int cliente, int tipoDocumento, long numero, string serie, int subSerie, FaturaAtualizarRequest request);
    }
}
