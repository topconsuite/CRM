using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using Topsys.TopConWeb.SharedKernel.Filters;
using TopSys.TopConWeb.Application.DTOS.Request.GrupoEconomico.Alteracao;
using TopSys.TopConWeb.Application.DTOS.Request.GrupoEconomico.Inclusao;
using TopSys.TopConWeb.Application.DTOS.Response.GrupoEconomico;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IGrupoEconomicoApplicationService : IApplicationServiceBase<GrupoEconomico>
    {
        void Adicionar(GrupoEconomicoInclusaoRequest grupoEconomicoRequest, string userRequest);
        void Atualizar(GrupoEconomicoAlteracaoRequest grupoEconomicoRequest, string userRequest);
        void Deletar(int idServico);
        GrupoEconomicoResponse ObterPorCodigo(int grupoEconomicoCodigo);
        PagedList<GrupoEconomicoResponse> Listar(int pagina, int porPagina, IFilter filter);
        IEnumerable<GrupoEconomicoResponse> Listar();

    }
}
