using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application.DTOS.Response.CartaoBandeira;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface ICartaoBandeiraApplicationService : IApplicationServiceBase<CartaoBandeira>
    {
        IEnumerable<CartaoBandeiraResponse> ListarTodosComAgregados();

        IEnumerable<CartaoBandeiraResponse> ListarTodosComIntegracao();
    }
}
