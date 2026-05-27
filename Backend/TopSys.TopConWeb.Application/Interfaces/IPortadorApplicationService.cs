using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSys.TopConWeb.Application.DTOS.Response.Portador;
using TopSys.TopConWeb.Application.DTOS.Request.PortadorCobranca;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Application.DTOS.Response.PortadorCobranca;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IPortadorApplicationService : IApplicationServiceBase<Portador>
    {
        IEnumerable<PortadorResponse> ListarVinculadosComContas();
        ResultDTO<PortadorCobrancaAdicionarResponse> Adicionar(PortadorCobrancaAdicionarRequest[] request);
        ResultDTO<PortadorCobrancaResponse> AtualizarPorId(int id, PortadorCobrancaAtualizarRequest request);
        ResultDTO<ICollection<PortadorCobrancaResponse>> Listar();
        ResultDTO<PortadorCobrancaResponse> ObterPorId(int id);
        ResultDTO<int> DeletarPorId(int id);
    }
}
