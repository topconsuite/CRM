using System.Collections.Generic;
using TopSys.TopConWeb.Application.DTOS.Request.MunicipioTributacao;
using TopSys.TopConWeb.Application.DTOS.Response.Integracao;
using TopSys.TopConWeb.Application.DTOS.Response.MunicipioTributacao;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IMunicipioTributacaoApplicationService : IApplicationServiceBase<Municipio>
    {
        ResultDTO<MunicipioTributacaoAdicionarResponse> Adicionar(MunicipioTributacaoAdicionarRequest[] request);
        ResultDTO<MunicipioTributacaoResponse> AtualizarPorId(int id, MunicipioTributacaoAtualizarRequest request);
        ResultDTO<MunicipioTributacaoResponse> AtualizarPorExternalId(string externalId, MunicipioTributacaoAtualizarRequest request);
        ResultDTO<MunicipioTributacaoResponse> AtualizarPorIbgeCode(int ibgeCode, MunicipioTributacaoAtualizarRequest request);
        ResultDTO<ICollection<MunicipioTributacaoResponse>> Listar(string uf);
        ResultDTO<MunicipioTributacaoResponse> ObterPorId(int id);
        ResultDTO<MunicipioTributacaoResponse> ObterPorExternalId(string externalId);
        ResultDTO<MunicipioTributacaoResponse> ObterPorIbgeCode(int ibgeCode);
    }
}
