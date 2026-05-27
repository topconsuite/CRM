using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Enums;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.Data.Persistence.Context;

namespace TopSys.TopConWeb.Infra.Data.Repositories
{
    public class SlumpRepository : RepositoryBase<Slump>, ISlumpRepository
    {
        public SlumpRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Slump> ListarPorSlumpReal(int slumpReal)
        {
            var slumps = _context.Slumps.Where(s => s.AmplitudeDe==slumpReal).AsNoTracking().ToList();

            return slumps;
        }
    }
}
