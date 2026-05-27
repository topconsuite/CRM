using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Response.TributacaoPisCofins;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface ITributacaoPisCofinsApplicationService : IApplicationServiceBase<TributacaoPisCofins>
    {
        IEnumerable<TributacaoPisCofinsResponse> ListarTodosDeSaida();
    }
}