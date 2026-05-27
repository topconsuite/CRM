using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Repositories
{
    public interface ICondicaoPagamentoRepository : IRepositoryBase<CondicaoPagamento>
    {
        new void Adicionar(CondicaoPagamento condicaoPagamento);
        IEnumerable<CondicaoPagamento> ListarPorUsinaDataParaAprovacaoPendente(int idUsina, DateTime data, string intervenienteTipo);
        IEnumerable<CondicaoPagamento> ListarPorUsinaDataIntervenienteTipo(int idUsina, DateTime data, string intervenienteTipo, int segmentacao);
        float ObterValorAdicionalM3PorCondicaoPagamentoUsinaPrecoUnitarioTabela(int idCondicaoPagamento, int idUsina, float precoUnitarioTabela);
        IEnumerable<CondicaoPagamento> ListarComParcelasPorCodigos(params int[] codigos);
        PagedList<CondicaoPagamento> ListaEmOrdemCrescente(int pagina, int porPagina, Expression<Func<CondicaoPagamento, bool>> filter);
		bool PossuiObrasUtilizando(int idCondicaoPagamento);
        bool CondicaoPagamentoPadraoUsinaTipoPessoa(int condicaoPagamentoCodigo, int idUsina, DateTime data, string intervenienteTipo);


        ICollection<CondicaoPagamento> ListarCondicaoPagamento();
		CondicaoPagamento ObterPorIdCondicaoPagamento(int id, bool tracking = false);
		CondicaoPagamento ObterPorExternalIdCondicaoPagamento(string externalId, bool tracking = false);
        int ObterProximoCodigo();
	}
}
