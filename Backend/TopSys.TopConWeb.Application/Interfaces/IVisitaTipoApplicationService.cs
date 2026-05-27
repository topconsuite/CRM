using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Application.DTOS.Request.Visita;
using TopSys.TopConWeb.Application.DTOS.Response.Visita;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IVisitaTipoApplicationService : IApplicationServiceBase<VisitaTipo>
    {
        void Adicionar(VisitaTipoInclusaoRequest tipoVisitaRequest, string userRequest);
        void Atualizar(VisitaTipoAlteracaoRequest tipoVisitaRequest, string userRequest);
        void Deletar(int codigo, string userRequest);
        VisitaTipoResponse ObterPorCodigo(int codigo);
        PagedList<VisitaTipoResponse> Listar(int pagina, int porPagina, Expression<Func<VisitaTipo, bool>> filter);
        IEnumerable<VisitaTipoResponse> ListarTipoVisitasAtivas();
    }
}
