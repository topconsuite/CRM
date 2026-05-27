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
    public class ResistenciaTipoApplicationService : ApplicationServiceBase<ResistenciaTipo>, IResistenciaTipoApplicationService
    {
        private readonly IResistenciaTipoService _resistenciaTipoService;

        public ResistenciaTipoApplicationService(IResistenciaTipoService resistenciaTipoService, IUnitOfWork unityOfWork) : base(resistenciaTipoService, unityOfWork)
        {
            _resistenciaTipoService = resistenciaTipoService;
        }
    }
}
