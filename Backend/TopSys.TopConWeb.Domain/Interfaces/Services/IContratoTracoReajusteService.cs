using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IContratoTracoReajusteService : IServiceBase<ContratoTracoReajuste>
    {
        IEnumerable<DateTime> ObterVigencias();
        PagedList<ContratoTracoReajuste> ListarContratoReajusteTracoPorPagina(int pagina, int porPagina, string filter);
        IEnumerable<ContratoTracoReajuste> ListarContratoReajusteTracoPorContrato(int usina, int anoContrato, int numContrato, DateTime dataVigencia);
        void AtualizarObraTracoReajustes(Obra obra, int sequencia, int numVersao, bool atualizaPrecoProposto = false);
    }
}
