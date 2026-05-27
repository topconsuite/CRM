using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Request.Banco;
using TopSys.TopConWeb.Application.DTOS.Response.Banco;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IBancoApplicationService : IApplicationServiceBase<Conta>
    {
        ResultDTO<BancoAdicionarResponse> Adicionar(BancoAdicionarRequest[] request);
        ResultDTO<BancoResponse> AtualizarPorId(int cod, int emp, BancoAtualizarRequest request);
        ResultDTO<ICollection<BancoResponse>> Listar();
        ResultDTO<BancoResponse> ObterPorId(int cod, int emp);
        ResultDTO<int> DeletarPorId(int cod, int emp);
    }
}
