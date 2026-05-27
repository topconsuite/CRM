using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface INotaFiscalFisicaService : IServiceBase<NotaFiscalFisica>
    {
        bool Emitida(Programacao programacao);
        PagedList<NotaFiscalFisica> ObterPorDataAtualizacao(DateTime dataInicio, DateTime? dataFim, int page, int limit);
        PagedList<NotaFiscalFisica> ObterPorDataRetornoAutomacao(DateTime dataInicio, DateTime? dataFim, int page, int limit);
        NotaFiscalFisicaComplemento ObterComplemento(int filial, int interveniente, int tipoDocumento, string serie, long numero, int sequencia);
        Mercadoria ObterMercadoria(string mercadoriaCodigo);
        PagedList<NotaFiscalFisicaIndicadorPontos> ObterIndicadorPontos(DateTime? dataInicio, DateTime? dataFim, int vendedor, string indicadorNome, int page, int limit);
    }
}
