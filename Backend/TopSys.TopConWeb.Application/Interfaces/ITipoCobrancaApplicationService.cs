using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application.DTOS.Response.TipoCobranca;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface ITipoCobrancaApplicationService : IApplicationServiceBase<TipoCobranca>
    {
        IEnumerable<TipoCobrancaResponse> ListarPorCondicaoPagamento(int idCondicaoPagamento);
    }
}
