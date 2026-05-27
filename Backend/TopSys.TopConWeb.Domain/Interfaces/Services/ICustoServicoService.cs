using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface ICustoServicoService : IServiceBase<CustoServico>
    {
        PagedList<CustoServico> ListarEmOrdemDecrescente(int pagina, int porPagina, Expression<Func<CustoServico, bool>> filter);
    }
}
