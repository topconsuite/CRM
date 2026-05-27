using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Response.Filial;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IFilialApplicationService : IApplicationServiceBase<Filial>
    {
        FilialResponse ObterPorId(int idFilial);
        ICollection<FilialResponse> Listar();
        ResultDTO<ICollection<FilialFiscalResponse>> FilialFiscalListar();
        ResultDTO<FilialFiscalResponse> FilialFiscalObterPorId(int id);
        ResultDTO<FilialFiscalResponse> FilialFiscalObterPorCentroCusto(int centroCusto);
    }
}
