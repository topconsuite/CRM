using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IFaturaService : IServiceBase<Fatura>
    {
        Fatura ObterPorChave(Expression<Func<Fatura, bool>> filter, bool tracking = false);
        ICollection<Fatura> Listar(Expression<Func<Fatura, bool>> filter, int page, int limit);
        PagedList<Fatura> ObterPorDataAtualizacao(DateTime dataInicio, DateTime? dataFim, int page, int limit);
        Expression<Func<Fatura, bool>> CriarFiltroFatura(DateTime? dataFatura, int? filial,int? centroCusto, int? tipoDocumento, int? cliente);
    }
}
