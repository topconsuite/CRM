using System;
using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Response.NotaFiscalDigital;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using Topsys.TopConWeb.SharedKernel.Common;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface INotaFiscalDigitalApplicationService
    {
        ResultDTO<NotaFiscalDigitalResponse> ObterPorChave(int filial, int cliente, int tipoDocumento, string serie, long numero, int sequencia);
        ResultDTO<NotaFiscalDigitalResponse> ObterPorChaveSemInterveniente(int filial, int tipoDocumento, string serie, long numero, int sequencia);
        ResultDTO<List<NotaFiscalDigitalResponse>> Listar(DateTime? dataNotaFiscal, int? filial, int? tipoDocumento, int? centroCusto, int? cliente, int? page, int? limit);
        ResultDTO<PagedList<NotaFiscalDigitalResponse>> ObterPorDataAtualizacao(DateTime dataInicio, DateTime? dataFim, int? page, int? limit);
    }
}
