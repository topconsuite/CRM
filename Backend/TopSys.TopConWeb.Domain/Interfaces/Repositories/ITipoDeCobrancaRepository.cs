using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface ITipoDeCobrancaRepository : IRepositoryBase<TipoDeCobranca>
    {
        ICollection<TipoDeCobranca> ListarTipoDeCobranca();
        TipoDeCobranca ObterPorIdTipoDeCobranca(int id, bool tracking = false);
        TipoDeCobranca ObterTipoDeCobrancaComDescricao(int codigoTipoCobranca);
    }
}