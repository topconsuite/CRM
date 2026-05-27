using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Response.TributacaoReforma;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface ITributacaoReformaApplicationService : IApplicationServiceBase<TributacaoReforma>
    {
        IEnumerable<TributacaoReformaResponse> ListarTodosProducao(string imposto);
    }
}
