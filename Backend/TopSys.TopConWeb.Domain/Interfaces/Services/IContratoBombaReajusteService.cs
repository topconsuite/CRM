using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IContratoBombaReajusteService : IServiceBase<ContratoBombaReajuste>
    {
        IEnumerable<DateTime> ObterVigencias();
        PagedList<ContratoBombaReajuste> ListarContratoReajusteBombaPorPagina(int pagina, int porPagina, string filter);
        IEnumerable<ContratoBombaReajuste> ListarContratoReajusteBombaPorContrato(int usina, int anoContrato, int numContrato, DateTime dataVigencia);
        void AtualizarObraBombaReajustes(Obra obra, int sequencia, int numVersao, bool atualizaTaxa = false);
    }
}
