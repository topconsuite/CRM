using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface ITributacaoPisCofinsService : IServiceBase<TributacaoPisCofins>
    {
        IEnumerable<TributacaoPisCofins> ListarTributacoesDeSaida();
    }
}