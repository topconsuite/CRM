using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface ITipoCobrancaService : IServiceBase<TipoCobranca>
    {
        IEnumerable<TipoCobranca> ListarPorCondicaoPagamento(int idCondicaoPagamento);
        bool ChecarSeExistePeloCodigo(int code);
    }
}
