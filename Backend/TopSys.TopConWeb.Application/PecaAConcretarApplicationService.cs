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
    public class PecaAConcretarApplicationService : ApplicationServiceBase<PecaAConcretar>, IPecaAConcretarApplicationService
    {
        private readonly IPecaAConcretarService _pecaAConcretarService;

        public PecaAConcretarApplicationService(IPecaAConcretarService pecaAConcretarService, IUnitOfWork unityOfWork)
            : base(pecaAConcretarService, unityOfWork)
        {
            _pecaAConcretarService = pecaAConcretarService;
        }
    }
}
