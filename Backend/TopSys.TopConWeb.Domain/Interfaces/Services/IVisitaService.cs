using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities.Visitas;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface IVisitaService : IServiceBase<Visita>
    {
        void Adicionar(Visita proposta);
        Visita ObterPorId(int usina, int ano, int numero, bool tracking = false);
        PagedList<Visita> ListarEmOrdemDecrescente(int pagina, int porPagina, Expression<Func<Visita, bool>> filter);

        void AdicionarAnexo(string usuario, Guid id, int usina, int anoVisita, int numeroVisita, string anexo, string nome);
        ICollection<VisitaAnexo> ListarAnexos(int usina, int anoVisita, int numeroVisita);
        byte[] ObterAnexo(Guid id);
        VisitaAnexo ObterVisitaAnexoPorId(Guid id);
        void AtualizarDescricaoAnexo(VisitaAnexo anexo);
        void RemoverAnexo(Guid id);

        PagedList<VisitaHistorico> ListarHistoricoEmOrdemDecrescente(int pagina, int porPagina, Expression<Func<VisitaHistorico, bool>> filter);
        void AdicionarHistorico(VisitaHistorico visitaHistorico);

    }
}
