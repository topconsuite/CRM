using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class ResistenciaTipoService : ServiceBase<ResistenciaTipo>, IResistenciaTipoService
    {
        private readonly IResistenciaTipoRepository _resistenciaTipoRepository;

        public ResistenciaTipoService(IResistenciaTipoRepository resistenciaTipoRepository) : base(resistenciaTipoRepository)
        {
            _resistenciaTipoRepository = resistenciaTipoRepository;
        }
    }
}
