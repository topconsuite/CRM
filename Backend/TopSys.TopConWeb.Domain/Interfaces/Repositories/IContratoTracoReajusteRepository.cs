using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IContratoTracoReajusteRepository : IRepositoryBase<ContratoTracoReajuste>
    {
        IEnumerable<DateTime> ObterVigencias();
        PagedList<ContratoTracoReajuste> ListarContratoReajusteTracoPorPagina(int pagina, int porPagina, string filter);
        IEnumerable<ContratoTracoReajuste> ListarContratoReajusteTracoPorContrato(int usina, int anoContrato, int numContrato, DateTime dataVigencia);
    }
}
