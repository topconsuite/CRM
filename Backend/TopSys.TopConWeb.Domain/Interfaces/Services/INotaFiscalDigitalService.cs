using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using Topsys.TopConWeb.SharedKernel.QueryResults;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface INotaFiscalDigitalService : IServiceBase<NotaFiscalDigital>
    {
        NotaFiscalDigital ObterPorChave(Expression<Func<NotaFiscalDigital, bool>> filter, bool tracking = false);
        ICollection<NotaFiscalDigital> Listar(Expression<Func<NotaFiscalDigital, bool>> filter, int page, int limit);
        Expression<Func<NotaFiscalDigital, bool>> CriarFiltroNotaFiscal(DateTime? dataNotaFiscal, int? filial, int? tipoDocumento, int? centroCusto, int? cliente);
        PagedList<NotaFiscalDigital> ObterPorDataAtualizacao(DateTime dataInicio, DateTime? dataFim, int page, int limit);
    }
}
