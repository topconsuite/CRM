using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Request.SituacaoFinanceira;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Application.DTOS.Response.SituacaoFinanceira;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface ISituacaoFinanceiraApplicationService : IApplicationServiceBase<SituacaoFinanceira>
    {
        ResultDTO<SituacaoFinanceiraAdicionarResponse> Adicionar(SituacaoFinanceiraAdicionarRequest[] request);
        ResultDTO<SituacaoFinanceiraResponse> AtualizarPorId(int id, SituacaoFinanceiraAtualizarRequest request);
        ResultDTO<ICollection<SituacaoFinanceiraResponse>> Listar();
        ResultDTO<SituacaoFinanceiraResponse> ObterPorId(int id);
        ResultDTO<int> DeletarPorId(int id);
    }
}
