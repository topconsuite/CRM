using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Application.DTOS.Request.Visita;
using TopSys.TopConWeb.Application.DTOS.Response.Lead;
using TopSys.TopConWeb.Application.DTOS.Response.Visita;
using TopSys.TopConWeb.Domain.Entities.Visitas;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface IVisitaApplicationService : IApplicationServiceBase<Visita>
    {

        VisitaAdicionarResponse Adicionar(VisitaAdicionarRequest request);
        void Atualizar(VisitaAtualizarRequest request, string usuario);
        PagedList<VisitaResponse> ListarEmOrdemDecrescente(int pagina, int porPagina, Expression<Func<Visita, bool>> filter);
        VisitaResponse ObterPorId(int usina, int ano, int numero);
        LeadResponse ObterLeadDeVisita(int usina, int ano, int numero);

        Guid AdicionarAnexo(string usuario, VisitaAnexoAdicionarRequest request);
        byte[] ObterAnexo(Guid id);
        VisitaAnexo ObterVisitaAnexoPorId(Guid id);
        void AtualizarDescricaoAnexo(VisitaAnexoAtualizarRequest anexo);
        void RemoverAnexo(Guid id);
        ICollection<VisitaAnexoResponse> ListarAnexos(int usina, int anoVisita, int numeroVisita);

        void AdicionarHistorico(VisitaHistoricoAdicionarRequest request, string usuario);
        PagedList<VisitaHistoricoResponse> ListarHistoricoEmOrdemDecrescente(int pagina, int porPagina, Expression<Func<VisitaHistorico, bool>> filter);

    }
}
