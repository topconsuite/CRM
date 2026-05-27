using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Domain.Entities.Lead;

namespace TopSys.TopConWeb.Domain.Interfaces.Services
{
    public interface ILeadService : IServiceBase<Lead>
    {
        void Adicionar(string usuario, Lead lead);
        PagedList<Lead> ListarEmOrdemDecrescente(int pagina, int porPagina, Expression<Func<Lead, bool>> filter);
        Lead ObterPorUsinaAnoNumero(int idUsina, int ano, int numero, bool tracking = false);
        void AdicionarAnexo(string usuario, Guid id, int usina, int anoLead, int numeroLead, string anexo, string nome);
        ICollection<LeadAnexo> ListarAnexos(int usina, int anoLead, int numeroLead);
        byte[] ObterAnexo(Guid id);
        LeadAnexo ObterLeadAnexoPorId(Guid id);
        void AtualizarDescricaoAnexo(LeadAnexo anexo);
        void RemoverAnexo(Guid id);
        PagedList<LeadInteracao> ListarInteracoes(int pagina, int porPagina, Expression<Func<LeadInteracao, bool>> filter);
        void AdicionarInteracao(string usuario, LeadInteracao leadInteracao);
    }
}
