using System;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities.MotivoPerdas;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IMotivoPerdaService : IServiceBase<MotivoPerda>
    {
        PagedList<MotivoPerda> ListarEmOrdemCrescente(int pagina, int porPagina, Expression<Func<MotivoPerda, bool>> filter);
    }
}
