using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;

namespace TopSys.TopConWeb.Application
{
    public class PedraApplicationService : ApplicationServiceBase<Pedra>, IPedraApplicationService
    {

        private readonly IPedraService _pedraService;

        public PedraApplicationService(IPedraService pedraService, IUnitOfWork unityOfWork) : base(pedraService, unityOfWork)
        {
            _pedraService = pedraService;
        }
    }
}
