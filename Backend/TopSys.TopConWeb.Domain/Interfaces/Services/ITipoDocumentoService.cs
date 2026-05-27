using System.Collections.Generic;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface ITipoDocumentoService : IServiceBase<TipoDocumento>
    {
        ICollection<TipoDocumento> Listar();
        TipoDocumento ObterPorId(int id, bool tracking = false);
    }
}