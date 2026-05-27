using System;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Application.DTOS.Request.Oportunidade;
using TopSys.TopConWeb.Application.DTOS.Response.Oportunidade;
using TopSys.TopConWeb.Domain.Entities.Oportunidades;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IOportunidadeTipoApplicationService : IApplicationServiceBase<OportunidadeTipo>
    {
        void Adicionar(OportunidadeTipoInclusaoRequest oportunidadeTipoRequest, string userRequest);
        void Atualizar(OportunidadeTipoAlteracaoRequest oportunidadeTipoRequest, string userRequest);
        void Deletar(int codigo, string userRequest);
        OportunidadeTipoResponse ObterPorCodigo(int codigo);
        PagedList<OportunidadeTipoResponse> Listar(int pagina, int porPagina, Expression<Func<OportunidadeTipo, bool>> filter);
    }
}
