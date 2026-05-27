using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface ITributacaoReformaService : IServiceBase<TributacaoReforma>
    {
        IEnumerable<TributacaoReforma> ListarTodosProducao(string imposto);
    }
}
