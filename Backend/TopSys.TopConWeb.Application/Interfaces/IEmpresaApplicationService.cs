using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Response.Empresa;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IEmpresaApplicationService : IApplicationServiceBase<Empresa>
    {
        ResultDTO<ICollection<EmpresaResponse>> Listar();
        ResultDTO<EmpresaResponse> ObterPorId(int id);
    }
}
