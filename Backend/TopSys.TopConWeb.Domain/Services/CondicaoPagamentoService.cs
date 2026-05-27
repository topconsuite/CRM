using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Domain.Interfaces.Services;

namespace TopSys.TopConWeb.Domain.Services
{
    public class CondicaoPagamentoService : ServiceBase<CondicaoPagamento>, ICondicaoPagamentoService
    {
        private readonly ICondicaoPagamentoRepository _condicaoPagamentoRepository;
        private readonly ICondicaoPagamentoParcelaRepository _condicaoPagamentoParcelaRepository;

        public CondicaoPagamentoService(
            ICondicaoPagamentoRepository condicaoPagamentoRepository,
			ICondicaoPagamentoParcelaRepository condicaoPagamentoParcelaRepository

			)
            : base(condicaoPagamentoRepository)
        {
            _condicaoPagamentoRepository = condicaoPagamentoRepository;
			_condicaoPagamentoParcelaRepository = condicaoPagamentoParcelaRepository;
        }

        public PagedList<CondicaoPagamento> ListarEmOrdemCrescente(int pagina, int porPagina, Expression<Func<CondicaoPagamento, bool>> filter)
        {
            var pagedList = _condicaoPagamentoRepository.ListaEmOrdemCrescente(pagina, porPagina, filter);

            return pagedList;
        }

        public IEnumerable<CondicaoPagamento> ListarPorUsinaDataParaAprovacaoPendente(int idUsina, DateTime data, string intervenienteTipo)
        {
            return _condicaoPagamentoRepository.ListarPorUsinaDataParaAprovacaoPendente(idUsina, data, intervenienteTipo);
        }

        public IEnumerable<CondicaoPagamento> ListarPorUsinaDataIntervenienteTipo(int idUsina, DateTime data, string intervenienteTipo, int segmentacao)
        {
            return _condicaoPagamentoRepository.ListarPorUsinaDataIntervenienteTipo(idUsina, data, intervenienteTipo, segmentacao).Where(t => t.Ativo ==  "S");
        }

        public CondicaoPagamento ObterPrincipal(IEnumerable<ObraPagamento> pagamentos)
        {
            if (pagamentos == null || pagamentos.Count() == 0) return null;

            if (pagamentos.Count() == 1) return _condicaoPagamentoRepository.ObterPorId(pagamentos.FirstOrDefault().CondicaoPagamentoCodigo);

            pagamentos = pagamentos.Where(t => t.Ativo).ToList();

            var condicoesComParcelas = _condicaoPagamentoRepository.ListarComParcelasPorCodigos(pagamentos.Select(t => t.CondicaoPagamentoCodigo).ToArray());;

            var listaComValoresRelativos = pagamentos.Select(t => new {
                condicaoPagamento = condicoesComParcelas.Where(c => c.Codigo == t.CondicaoPagamentoCodigo).FirstOrDefault(),
                valorRelativo = t.Valor * condicoesComParcelas.Where(c => c.Codigo == t.CondicaoPagamentoCodigo).Select(c => c.PrazoMedio).FirstOrDefault()
            }).ToList();

            var maiorValor = listaComValoresRelativos.Select(t => t.valorRelativo).DefaultIfEmpty().Max();

            return listaComValoresRelativos.Where(t => t.valorRelativo == maiorValor)
                .Select(t => t.condicaoPagamento).FirstOrDefault();
        }

        public float ObterValorAdicionalM3PorCondicaoPagamentoUsinaPrecoUnitarioTabela(int idCondicaoPagamento, int idUsina, float precoUnitarioTabela)
        {
            return _condicaoPagamentoRepository.ObterValorAdicionalM3PorCondicaoPagamentoUsinaPrecoUnitarioTabela(idCondicaoPagamento, idUsina, precoUnitarioTabela);
        }

        public bool PossuiObrasUtilizando(int idCondicaoPagamento)
        {
            return _condicaoPagamentoRepository.PossuiObrasUtilizando(idCondicaoPagamento);
        }

        public ICollection<CondicaoPagamento> Listar()
        {
            return _condicaoPagamentoRepository.ListarCondicaoPagamento();
        }

        public CondicaoPagamento ObterPeloId(int id, bool tracking = false)
        {
            return _condicaoPagamentoRepository.ObterPorIdCondicaoPagamento(id, tracking);
        }

        public CondicaoPagamento ObterPorExternalId(string externalId, bool tracking = false)
        {
            return _condicaoPagamentoRepository.ObterPorExternalIdCondicaoPagamento(externalId, tracking);
        }

        public bool CondicaoPagamentoPadraoUsinaTipoPessoa(int condicaoPagamentoCodigo, int idUsina, DateTime data, string intervenienteTipo)
        {
            return _condicaoPagamentoRepository.CondicaoPagamentoPadraoUsinaTipoPessoa(condicaoPagamentoCodigo, idUsina, data, intervenienteTipo);
        }

        public int ObterProximoCodigo()
        {
            return _condicaoPagamentoRepository.ObterProximoCodigo();
        }
    }
}
