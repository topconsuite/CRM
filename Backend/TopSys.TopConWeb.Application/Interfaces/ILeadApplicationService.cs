using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Topsys.TopConWeb.SharedKernel.Common;
using TopSys.TopConWeb.Application.DTOS.Request.Lead;
using TopSys.TopConWeb.Application.DTOS.Request.Lead.Alteracao;
using TopSys.TopConWeb.Application.DTOS.Request.Lead.Inclusao;
using TopSys.TopConWeb.Application.DTOS.Response.Lead;
using TopSys.TopConWeb.Application.DTOS.Response.Lead.LeadInseridaResponse;
using TopSys.TopConWeb.Application.DTOS.Response.Lead.LeadSimplesResponse;
using TopSys.TopConWeb.Application.DTOS.Response.Oportunidade;
using TopSys.TopConWeb.Domain.Entities.Lead;

namespace TopSys.TopConWeb.Application.Interfaces
{
    public interface ILeadApplicationService : IApplicationServiceBase<Lead>
    {
        LeadInseridaResponse Adicionar(string usuario, LeadInclusaoRequest leadRequest);
        void Atualizar(string usuario, LeadAlteracaoRequest lead);
        PagedList<LeadSimplesResponse> ListarEmOrdemDecrescente(int pagina, int porPagina, Expression<Func<Lead, bool>> filter);
        LeadResponse ObterPorUsinaAnoNumero(int idUsina, int ano, int numero);
        IEnumerable<LeadLogResponse> ListarLeadLogsPorId(int idUsina, int ano, int numero);
        Guid AdicionarAnexo(string usuario, LeadAnexoAdicionarRequest request);
        ICollection<LeadAnexoResponse> ListarAnexos(int usina, int anoLead, int numeroLead);
        byte[] ObterAnexo(Guid id);
        LeadAnexo ObterLeadAnexoPorId(Guid id);
        void AtualizarDescricaoAnexo(LeadAnexoAtualizarRequest anexo);
        void RemoverAnexo(Guid id);
        PagedList<LeadInteracaoResponse> ListarInteracoes(int pagina, int porPagina, Expression<Func<LeadInteracao, bool>> filter);
        void AdicionarInteracao(string usuario, LeadInteracaoAdicionarRequest request);

        OportunidadeResponse ObterOportunidadeDeLead(int usina, int ano, int numero);
    }
}
