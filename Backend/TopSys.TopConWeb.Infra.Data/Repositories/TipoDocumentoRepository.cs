using System.Collections.Generic;
using System.Linq;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class TipoDocumentoRepository : RepositoryBase<TipoDocumento>, ITipoDocumentoRepository
    {
        public TipoDocumentoRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }

        public ICollection<TipoDocumento> ListarTipoDocumento()
        {
            return _context
                .TipoDocumento
                .AsNoTracking()
                .OrderBy(t => t.Codigo)
                .ToList();
        }

        public TipoDocumento ObterPorIdTipoDocumento(int id, bool tracking = false)
        {
            return _context
                .TipoDocumento
                .Where(t => t.Codigo == id)
                .Tracking(tracking)
                .FirstOrDefault();
        }
    }
}