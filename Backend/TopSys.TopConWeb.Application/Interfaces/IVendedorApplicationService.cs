using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application.DTOS.Request.Vendedor;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Application.DTOS.Response.Vendedor;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IVendedorApplicationService : IApplicationServiceBase<Vendedor>
    {
        ResultDTO<List<VendedorIntegracaoResponse>> Listar();
        ResultDTO<VendedorIntegracaoResponse> ObterPorId(int id);
        ResultDTO<VendedorIntegracaoResponse> ObterPorExternalId(string externalId);
        ResultDTO<VendedorIntegracaoAdicionarResponse> Adicionar(VendedorIntegracaoAdicionarRequest[] request);
        ResultDTO<VendedorIntegracaoResponse> AtualizarId(int id, VendedorIntegracaoAtualizarRequest request);
        ResultDTO<VendedorIntegracaoResponse> AtualizarExternalId(string externalId, VendedorIntegracaoAtualizarRequest request);
        ResultDTO<int> DeletarPorId(int id);
        ResultDTO<int> DeletarPorExternalId(string externalId);
    }
}
