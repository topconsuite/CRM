using TopSys.TopConWeb.Application.DTOS.Request.Prensa;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Application.DTOS.Response.Prensa;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IPrensaApplicationService : IApplicationServiceBase<Prensa>
    {
        ResultDTO<PrensaResponse> PrensaAdicionar(PrensaRequest request);
    }
}
