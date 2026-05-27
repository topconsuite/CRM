using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface IFaturaRepository : IRepositoryBase<Fatura>
    {
        Fatura ObterPorChaveFatura(Expression<Func<Fatura, bool>> filter, bool tracking = false);
        ICollection<Fatura> ListarComPaginacao(Expression<Func<Fatura, bool>> filter, int page, int limit);
        PagedList<Fatura> ObterPorDataAtualizacao(DateTime dataInicio, DateTime? dataFim, int page, int limit);
    }
}
