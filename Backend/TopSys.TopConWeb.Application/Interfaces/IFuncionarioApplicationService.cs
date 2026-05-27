using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Response.Funcionario;
using TopSys.TopConWeb.Application.DTOS.Request.Funcionario;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IFuncionarioApplicationService : IApplicationServiceBase<Funcionario>
    {
        IEnumerable<FuncionarioAnalistaResponse> ListarAnalistas();
        ResultDTO<FuncionarioAdicionarResponse> Adicionar(FuncionarioAdicionarRequest[] request);
        ResultDTO<FuncionarioResponse> AtualizarPorId(int codigo, FuncionarioAtualizarRequest request);
        ResultDTO<FuncionarioResponse> AtualizarPorExternalId(string externalID, FuncionarioAtualizarRequest request);
        ResultDTO<List<FuncionarioResponse>> Listar();
        ResultDTO<FuncionarioResponse> ObterPorId(int codigo);
        ResultDTO<FuncionarioResponse> ObterPorExternalId(string externalId);
    }
}
