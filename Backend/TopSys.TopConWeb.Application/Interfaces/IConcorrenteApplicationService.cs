using System;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Application.DTOS.Request.Oportunidade;
using TopSys.TopConWeb.Application.DTOS.Response.Oportunidade;
using TopSys.TopConWeb.Domain.Entities.Oportunidades;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IConcorrenteApplicationService : IApplicationServiceBase<Concorrente>
    {
        void Adicionar(ConcorrenteInclusaoRequest concorrenteRequest, string userRequest);
        void Atualizar(ConcorrenteAlteracaoRequest concorrenteRequest, string userRequest);
        void Deletar(int codigo, string userRequest);
        ConcorrenteResponse ObterPorCodigo(int codigo);
        PagedList<ConcorrenteResponse> Listar(int pagina, int porPagina, Expression<Func<Concorrente, bool>> filter);
    }
}
