using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface ICartaoBandeiraService : IServiceBase<CartaoBandeira>
    {
        IEnumerable<CartaoBandeira> ListarTodosComAgregados();

        IEnumerable<CartaoBandeira> ListarTodosComIntegracao();
    }
}
