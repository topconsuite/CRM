using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IContratoBombaReajusteRepository : IRepositoryBase<ContratoBombaReajuste>
    {
        IEnumerable<DateTime> ObterVigencias();
        PagedList<ContratoBombaReajuste> ListarContratoReajusteBombaPorPagina(int pagina, int porPagina, string filter);
        IEnumerable<ContratoBombaReajuste> ListarContratoReajusteBombaPorContrato(int usina, int anoContrato, int numContrato, DateTime dataVigencia);
    }
}
