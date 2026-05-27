using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface ITributacaoPisCofinsRepository : IRepositoryBase<TributacaoPisCofins>
    {
        IEnumerable<TributacaoPisCofins> ListarTributacoesDeSaida();
    }
}