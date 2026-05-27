using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Application.DTOS.Generic;
using TopSys.TopConWeb.Application.DTOS.Request.CondicaoPagamento;
using TopSys.TopConWeb.Application.DTOS.Request.CondicaoPagamento.Alteracao;
using TopSys.TopConWeb.Application.DTOS.Request.CondicaoPagamento.Inclusao;
using TopSys.TopConWeb.Application.DTOS.Response.CondicaoPagamento;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Domain.Entities;


namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface ICondicaoPagamentoApplicationService : IApplicationServiceBase<CondicaoPagamento>
    {
        IEnumerable<CondicaoPagamento> ListarPorUsinaDataParaAprovacaoPendente(int idUsina, DateTime data, string intervenienteTipo);
        IEnumerable<CondicaoPagamentoResponse> ListarPorUsinaDataIntervenienteTipo(int idUsina, DateTime data, string intervenienteTipo, int segmentacao);
        float ObterValorAdicionalM3PorCondicaoPagamentoUsinaPrecoUnitarioTabela(int idCondicaoPagamento, int idUsina, float precoUnitarioTabela);
        PagedList<CondicaoPagamentoResponse> Listar(int pagina, int porPagina, Expression<Func<CondicaoPagamento, bool>> filter);
		void Deletar(int idCondicaoPagamento);
        void Adicionar(CondicaoPagamentoInclusaoRequest condicaoPagamentoRequest);
        void Atualizar(CondicaoPagamentoAlteracaoRequest condicaoPagamentoRequest);
        bool PossuiObrasUtilizando(int idCondicaoPagamento);

        ResultDTO<CondicaoDePagamentoAdicionarResponse> AdicionarIntegration(CondicoesPagamentoAdicionarRequest[] request);
        ResultDTO<CondicaoDePagamentoResponse> AtualizaPorIdIntegration(int id, CondicoesPagamentoAtualizarRequest request);
        ResultDTO<CondicaoDePagamentoResponse> AtualizarPorExternalIdIntegration(string externalId, CondicoesPagamentoAtualizarRequest request);
        ResultDTO<ICollection<CondicaoDePagamentoResponse>> ListarIntegration();
        ResultDTO<CondicaoDePagamentoResponse> ObterPorIdIntegration(int id);
        ResultDTO<CondicaoDePagamentoResponse> ObterPorExternalIdIntegration(string externalId);
    }
}
