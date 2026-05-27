using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface ICondicaoPagamentoService : IServiceBase<CondicaoPagamento>
    {
        IEnumerable<CondicaoPagamento> ListarPorUsinaDataParaAprovacaoPendente(int idUsina, DateTime data, string intervenienteTipo);
        IEnumerable<CondicaoPagamento> ListarPorUsinaDataIntervenienteTipo(int idUsina, DateTime data, string intervenienteTipo, int segmentacao);
        float ObterValorAdicionalM3PorCondicaoPagamentoUsinaPrecoUnitarioTabela(int idCondicaoPagamento, int idUsina, float precoUnitarioTabela);
        CondicaoPagamento ObterPrincipal(IEnumerable<ObraPagamento> pagamentos);
        PagedList<CondicaoPagamento> ListarEmOrdemCrescente(int pagina, int porPagina, Expression<Func<CondicaoPagamento, bool>> filter);
        bool PossuiObrasUtilizando(int idCondicaoPagamento);
        bool CondicaoPagamentoPadraoUsinaTipoPessoa(int condicaoPagamentoCodigo, int idUsina, DateTime data, string intervenienteTipo);


        ICollection<CondicaoPagamento> Listar();
		CondicaoPagamento ObterPeloId(int id, bool tracking = false);
        CondicaoPagamento ObterPorExternalId(string externalId, bool tracking = false);
        int ObterProximoCodigo();
	}
}
