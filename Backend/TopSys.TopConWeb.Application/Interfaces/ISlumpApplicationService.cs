using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application.DTOS.Response.Slump;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Services;
using TopSys.TopConWeb.Infra.Data.Persistence.Interface;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface ISlumpApplicationService : IApplicationServiceBase<Slump>
    {
        IEnumerable<SlumpResponse> ListarPorSlumpReal(int slumpReal);
    }
}
