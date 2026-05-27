using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface ITipoDocumentoRepository : IRepositoryBase<TipoDocumento>
    {
        ICollection<TipoDocumento> ListarTipoDocumento();
        TipoDocumento ObterPorIdTipoDocumento(int id, bool tracking = false);
    }
}