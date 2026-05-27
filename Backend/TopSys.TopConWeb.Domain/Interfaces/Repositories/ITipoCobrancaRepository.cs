using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface ITipoCobrancaRepository : IRepositoryBase<TipoCobranca>
    {
        IEnumerable<TipoCobranca> ListarPorCondicaoPagamento(int idCondicaoPagamento);
        TipoCobranca GetByCode(int code);
    }
}
