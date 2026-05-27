using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Request.CentroCusto;
using TopSys.TopConWeb.Application.DTOS.Response.CentroCusto;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface ICentroCustoApplicationService : IApplicationServiceBase<CentroCusto>
    {
        ResultDTO<CentroCustoAdicionarResponse> Adicionar(CentroCustoAdicionarRequest[] request);
        ResultDTO<CentroCustoResponse> AtualizarPorId(int id, CentroCustoAtualizarRequest request);
        ResultDTO<ICollection<CentroCustoResponse>> Listar();
        ResultDTO<CentroCustoResponse> ObterPorId(int id);
        ResultDTO<int> DeletarPorId(int id);
    }
}
