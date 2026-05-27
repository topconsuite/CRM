using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities.AprovacaoComercialAlcada;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IAprovacaoComercialService : IServiceBase<AprovacaoComercialUsina>
    {
        AprovacaoComercialUsina ObterPorId(Guid id, bool tracking = false);
        AprovacaoComercialUsina ObterPorUsina(int usinaId, bool tracking = false);

        PagedList<AprovacaoComercialUsina> ListarAprovacaoComercialUsina(int pagina, int porPagina, Expression<Func<AprovacaoComercialUsina, bool>> filter);
        bool UtilizaAprovacaoComercialPorAlcada(int usinaId);
        void AdicionarLog(AprovacaoComercialLog log);

    }
}
