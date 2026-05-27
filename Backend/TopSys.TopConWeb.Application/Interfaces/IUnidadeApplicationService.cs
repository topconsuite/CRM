using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Response.Unidade;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IUnidadeApplicationService: IApplicationServiceBase<Unidade>
    {
        IEnumerable<UnidadeResponse> ListarTodos();
    }
}
