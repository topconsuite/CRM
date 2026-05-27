using System;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities.Oportunidades;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IOportunidadeTipoRepository : IRepositoryBase<OportunidadeTipo>
    {
        PagedList<OportunidadeTipo> ListarEmOrdemCrescente(int pagina, int porPagina, Expression<Func<OportunidadeTipo, bool>> filter);
    }
}
