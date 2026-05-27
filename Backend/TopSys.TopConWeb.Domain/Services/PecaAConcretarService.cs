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
    public class PecaAConcretarService : ServiceBase<PecaAConcretar>, IPecaAConcretarService
    {
        private readonly IPecaAConcretarRepository _pecaAConcretarRepository;

        public PecaAConcretarService(IPecaAConcretarRepository pecaAConcretarRepository)
            : base(pecaAConcretarRepository)
        {
            _pecaAConcretarRepository = pecaAConcretarRepository;
        }
    }
}
