using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class FilialRepository : RepositoryBase<Filial>, IFilialRepository
    {
        public FilialRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }

        public Filial ObterPorId(int idFilial)
        {
            return _context.Filial
                .Where(x => x.Codigo == idFilial)
                .FirstOrDefault();
        }

        public ICollection<Filial> Listar()
        {
            return _context.Filial
                .OrderBy(x => x.Codigo).ToList();
        }

        public Filial ObterPorCentroCusto(int centroCusto)
        {
            return _context.Filial
                .Where(x => x.CentroCusto == centroCusto)
                .FirstOrDefault();
        }
    }

}
