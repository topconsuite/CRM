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
    public class UsoApplicationService : ApplicationServiceBase<Uso>, IUsoApplicationService
    {
        private readonly IUsoService _usoService;

        public UsoApplicationService(IUsoService usoService, IUnitOfWork unityOfWork) : base(usoService, unityOfWork)
        {
            _usoService = usoService;
        }
    }
}
