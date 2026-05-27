using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface ITipoDeCobrancaService : IServiceBase<TipoDeCobranca>
    {
        ICollection<TipoDeCobranca> Listar();
        TipoDeCobranca ObterPorId(int id, bool tracking = false);
    }
}