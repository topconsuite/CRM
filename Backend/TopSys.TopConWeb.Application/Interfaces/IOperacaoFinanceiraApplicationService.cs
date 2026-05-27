using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Request.OperacaoFinanceira;
using TopSys.TopConWeb.Application.DTOS.Response.OperacaoFinanceira;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IOperacaoFinanceiraApplicationService
    {
        ResultDTO<OperacaoFinanceiraAdicionarResponse> Adicionar(OperacaoFinanceiraAdicionarRequest[] request);
        ResultDTO<OperacaoFinanceiraResponse> AtualizarPorId(int id, OperacaoFinanceiraAtualizarRequest request);
        ResultDTO<ICollection<OperacaoFinanceiraResponse>> Listar();
        ResultDTO<OperacaoFinanceiraResponse> ObterPorId(int id);
        ResultDTO<int> DeletarPorId(int id);
    }
}