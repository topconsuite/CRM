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
    public class ResistenciaTipoRepository : RepositoryBase<ResistenciaTipo>, IResistenciaTipoRepository
    {
        public ResistenciaTipoRepository(AppDataContext context) : base(context)
        {
            _context = context;
        }
    }
}
