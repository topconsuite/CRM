using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using Topsys.TopConWeb.SharedKernel.QueryResults;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface INotaFiscalDigitalRepository : IRepositoryBase<NotaFiscalDigital>
    {
        NotaFiscalDigitalDetalhesFiscais ObterNFDetalhesFiscais(NotaFiscalDigital notaFiscalDigital);
        NotaFiscalDigitalDetalhesDistribuicao ObterNFDetalhesDistribuicao(NotaFiscalDigital notaFiscalDigital);
        NotaFiscalDigital ObterPorChaveNotaFiscalDigital(Expression<Func<NotaFiscalDigital, bool>> filter, bool tracking = false);
        ICollection<NotaFiscalDigital> ListarComPaginacao(Expression<Func<NotaFiscalDigital, bool>> filter, int page, int limit);
        PagedList<NotaFiscalDigital> ObterPorDataAtualizacao(DateTime dataInicio, DateTime? dataFim, int page, int limit);
    }
}
