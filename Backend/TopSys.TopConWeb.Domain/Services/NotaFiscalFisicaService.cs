using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class NotaFiscalFisicaService : ServiceBase<NotaFiscalFisica>, INotaFiscalFisicaService
    {
        INotaFiscalFisicaRepository _notaFiscalFisicaRepository;

        public NotaFiscalFisicaService(INotaFiscalFisicaRepository notaFiscalFisicaRepository)
            : base(notaFiscalFisicaRepository)
        {
            _notaFiscalFisicaRepository = notaFiscalFisicaRepository;
        }

        public bool Emitida(Programacao programacao)
        {
            return _notaFiscalFisicaRepository.Emitida(programacao);
        }

        public PagedList<NotaFiscalFisica> ObterPorDataAtualizacao(DateTime dataInicio, DateTime? dataFim, int page, int limit)
        {
            return _notaFiscalFisicaRepository.ObterPorDataAtualizacao(dataInicio, dataFim, page, limit);
        }

        public NotaFiscalFisicaComplemento ObterComplemento(int filial, int interveniente, int tipoDocumento, string serie, long numero, int sequencia)
        {
            return _notaFiscalFisicaRepository.ObterComplemento(filial, interveniente, tipoDocumento, serie, numero, sequencia);
        }

        public Mercadoria ObterMercadoria(string mercadoriaCodigo)
        {
            return _notaFiscalFisicaRepository.ObterMercadoria(mercadoriaCodigo);
        }

        public PagedList<NotaFiscalFisica> ObterPorDataRetornoAutomacao(DateTime dataInicio, DateTime? dataFim, int page, int limit)
        {
            return _notaFiscalFisicaRepository.ObterPorDataRetornoAutomacao(dataInicio, dataFim, page, limit);
        }

        public PagedList<NotaFiscalFisicaIndicadorPontos> ObterIndicadorPontos(DateTime? dataInicio, DateTime? dataFim, int vendedor, string indicadorNome, int page, int limit)
        {
            return _notaFiscalFisicaRepository.ObterIndicadorPontos(dataInicio, dataFim, vendedor, indicadorNome, page, limit);
        }
    }
}
