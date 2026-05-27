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
    public class PedraService : ServiceBase<Pedra>, IPedraService
    {
        private readonly IPedraRepository _pedraRepository;

        public PedraService(IPedraRepository pedraRepository) : base(pedraRepository)
        {
            _pedraRepository = pedraRepository;
        }
    }
}
