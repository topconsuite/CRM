using System;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities.MotivoPerdas;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IMotivoPerdaRepository : IRepositoryBase<MotivoPerda>
    {
        PagedList<MotivoPerda> ListarEmOrdemCrescente(int pagina, int porPagina, Expression<Func<MotivoPerda, bool>> filter);
    }
}
