using System;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface ICustoServicoRepository:  IRepositoryBase<CustoServico>
    {
        CustoServico ObterPorUsina(int idUsina);
        PagedList<CustoServico> ListaEmOrdemDecrescente(int pagina, int porPagina, Expression<Func<CustoServico, bool>> filter);
    }
}
