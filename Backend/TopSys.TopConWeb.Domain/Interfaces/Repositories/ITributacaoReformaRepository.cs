using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface ITributacaoReformaRepository : IRepositoryBase<TributacaoReforma>
    {
        IEnumerable<TributacaoReforma> ListarTodosProducao(string imposto);
    }
}
