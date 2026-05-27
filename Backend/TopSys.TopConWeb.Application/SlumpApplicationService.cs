using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application;
using TopSys.TopConWeb.Application.DTOS.Response.Slump;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;

namespace TopSys.TopConWeb.Application
{
    public class SlumpApplicationService : ApplicationServiceBase<Slump>, ISlumpApplicationService
    {
        private readonly ISlumpService _slumpService;

        public SlumpApplicationService(ISlumpService slumpService, IUnitOfWork unityOfWork) : base(slumpService, unityOfWork)
        {
            _slumpService = slumpService;
        }

        public IEnumerable<SlumpResponse> ListarPorSlumpReal(int slumpReal)
        {
            return AutoMapper.Mapper.Map(_slumpService.ListarPorSlumpReal(slumpReal), new List<SlumpResponse>());
        }
    }
}
