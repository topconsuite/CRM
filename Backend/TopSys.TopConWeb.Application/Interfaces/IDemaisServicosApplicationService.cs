using System;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Application.DTOS.Request.DemaisServicos.Alteracao;
using TopSys.TopConWeb.Application.DTOS.Request.DemaisServicos.Inclusao;
using TopSys.TopConWeb.Application.DTOS.Response.DemaisServicos;
using TopSys.TopConWeb.Domain.Entities;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IDemaisServicosApplicationService: IApplicationServiceBase<DemaisServicos>
    {
        void Adicionar(DemaisServicosInclusaoRequest demaisServicosRequest);
        void Atualizar(DemaisServicosAlteracaoRequest demaisServicosRequest);
        void Deletar(int idServico);
        PagedList<DemaisServicosResponse> Listar(int pagina, int porPagina, Expression<Func<DemaisServicos, bool>> filter);

    }
}
