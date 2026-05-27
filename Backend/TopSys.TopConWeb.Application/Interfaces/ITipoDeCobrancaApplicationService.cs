using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Request.TipoDeCobranca;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Application.DTOS.Response.TipoDeCobranca;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface ITipoDeCobrancaApplicationService
    {
        ResultDTO<TipoDeCobrancaAdicionarResponse> Adicionar(TipoDeCobrancaAdicionarRequest[] request);
        ResultDTO<TipoDeCobrancaResponse> AtualizarPorId(int id, TipoDeCobrancaAtualizarRequest request);
        ResultDTO<ICollection<TipoDeCobrancaResponse>> Listar();
        ResultDTO<TipoDeCobrancaResponse> ObterPorId(int id);
    }
}